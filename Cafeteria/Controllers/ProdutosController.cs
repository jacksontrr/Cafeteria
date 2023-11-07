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

namespace Cafeteria.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            int? clienteId = null;
            if (User.Identity.IsAuthenticated && User.IsInRole("Cliente"))
            {
                clienteId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            }
            var produtos = await _produtoService.GetAll(clienteId);
            return produtos != null ?
                        View(produtos) :
                        Problem("Entity set 'CafeteriaContext.Produtos'  is null.");
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
            } catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var produto = await _produtoService.Get(id.GetValueOrDefault());

            if (id == null || produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    bool error = _produtoService.SaveFile(ref produtoViewModel);
                    if (error)
                    {
                        ModelState.AddModelError("Arquivo", "Por favor, envie uma imagem.");
                        return View("Create", produtoViewModel);
                    }
                    Produto produto = new Produto
                    {
                        Nome = produtoViewModel.Nome,
                        Descricao = produtoViewModel.Descricao,
                        Preco = produtoViewModel.Preco,
                        Imagem = produtoViewModel.Imagem
                    };
                    await _produtoService.Add(produto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Arquivo", e.Message);
                    return View("Create", produtoViewModel);
                }
            }
            return View(produtoViewModel);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProdutoViewModel produtoViewModel)
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
                    _produtoService.DeleteFile(produto.Imagem);
                    bool error = _produtoService.SaveFile(ref produtoViewModel);
                    if (error)
                    {
                        ModelState.AddModelError("Arquivo", "Por favor, envie uma imagem.");
                        return View("Edit", produtoViewModel);
                    }

                    await _produtoService.Update(id, new Produto
                    {
                        Nome = produtoViewModel.Nome,
                        Descricao = produtoViewModel.Descricao,
                        Preco = produtoViewModel.Preco,
                        Imagem = produtoViewModel.Imagem
                    });

                    if (_produtoService.CheckIfItHasBeenChanged(produto, await _produtoService.Get(id)))
                    {
                        return RedirectToAction(nameof(Index));
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

        // GET: Produtos/Delete/5
        public IActionResult Delete(int? id)
        {
            var produto = _produtoService.Get(id.GetValueOrDefault());

            if (id == null || produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
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


            return RedirectToAction(nameof(Index));
        }

        // Ação de favoritar
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Favoritar(int id)
        {
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
                        return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
        }

    }
}
