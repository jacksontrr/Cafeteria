using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Data;
using Cafeteria.Models;
using Microsoft.AspNetCore.Hosting;
using Cafeteria.ViewModels;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;
using Cafeteria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Cafeteria.Utilities;

namespace Cafeteria.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly ICarrinhoService _cart;

        public ProdutosController(IProdutoService produtoService, ICarrinhoService cart)
        {
            _produtoService = produtoService;
            _cart = cart;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["CartQuantity"] = _cart.GetCart().Count;
            int? clienteId = null;
            if (User.Identity.IsAuthenticated && User.IsInRole("Cliente"))
            {
                clienteId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            }
            try
            {
                var produtos = await _produtoService.GetAll(clienteId);
                return View(produtos);

            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        public async Task<IActionResult> Search(string search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    ViewBag.CurrentFilter = search;
                    return View("Index", await _produtoService.GetNome(search));
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["CartQuantity"] = _cart.GetCart().Count;
            var produto = await _produtoService.Get(id.GetValueOrDefault());

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Favoritar(int id)
        {
            string url = "";
            try
            {
                int clienteId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var produto = await _produtoService.Get(id);
                if (produto != null)
                {
                    var favorito = new Favorito
                    {
                        ClienteId = clienteId,
                        ProdutoId = id
                    };

                    var encontrado = await _produtoService.GetFavorito(favorito);
                    if (encontrado != null)
                    {
                        await _produtoService.DeleteFavorite(encontrado.Id);
                        url = Request.Cookies["LastRequest"];

                        return Redirect(url);
                    }
                    await _produtoService.SaveFavorite(favorito);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
            url = Request.Cookies["LastRequest"];
            return Redirect(url);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(ProdutoViewModel produtoViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    (bool error, produtoViewModel) = await _produtoService.SaveFile(produtoViewModel);
                    if (error)
                    {
                        ModelState.AddModelError("Arquivo", "Por favor, envie uma imagem.");
                        return View("Cadastrar", produtoViewModel);
                    }
                    Produto produto = new Produto
                    {
                        Nome = produtoViewModel.Nome,
                        Descricao = produtoViewModel.Descricao,
                        Preco = produtoViewModel.Preco,
                        Imagem = produtoViewModel.Imagem
                    };
                    await _produtoService.Add(produto);
                    return RedirectToAction(nameof(ListarProdutos));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Arquivo", e.Message);
                    return View("Cadastrar", produtoViewModel);
                }
            }
            return View(produtoViewModel);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int? id)
        {
            var produto = await _produtoService.Get(id.GetValueOrDefault());

            if (id == null || produto == null)
            {
                return NotFound();
            }

            ProdutoViewModel produtoViewModel = new ProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Imagem = produto.Imagem
            };

            return View(produtoViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, ProdutoViewModel produtoViewModel)
        {
            Produto produto = await _produtoService.Get(id);

            if (id != produtoViewModel.Id || produto == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (produtoViewModel.Arquivo != null || produtoViewModel.RemoverImagem)
                    {
                        (bool error, produtoViewModel) = await _produtoService.SaveFile(produtoViewModel);
                        if (error)
                        {
                            ModelState.AddModelError("Arquivo", "Por favor, envie uma imagem.");
                            return View("Editar", produtoViewModel);
                        }
                        _produtoService.DeleteFile(produto.Imagem);
                    }
                    Produto NovoProduto = new()
                    {
                        Descricao = produtoViewModel.Descricao,
                        Imagem = produtoViewModel.Imagem,
                        Nome = produtoViewModel.Nome,
                        Preco = produtoViewModel.Preco
                    };
                    if (_produtoService.CheckIfItHasBeenChanged(produto, NovoProduto))
                    {
                        await _produtoService.Update(id, NovoProduto);

                        return RedirectToAction(nameof(ListarProdutos));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_produtoService.Exists(produtoViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(produtoViewModel);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Deletar(int? id)
        {
            var produto = await _produtoService.Get(id.GetValueOrDefault());

            if (id == null || produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var produto = await _produtoService.Get(id);

                if (produto != null)
                {
                    await _produtoService.Delete(id);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }


            return RedirectToAction(nameof(ListarProdutos));
        }

        // ListarProdutos
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListarProdutos()
        {
            var produto = await _produtoService.GetAll(null);
            IEnumerable<ProdutoViewModel> produtos = produto.Select(p => new ProdutoViewModel
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Imagem = p.Imagem
            });

            return View(produtos);
        }
    }
}
