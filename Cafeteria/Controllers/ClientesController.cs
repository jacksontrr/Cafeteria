using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Data;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;
using Cafeteria.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Cafeteria.ViewModels;
using Cafeteria.Utilities;

namespace Cafeteria.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IAdministradorService _administradorService;

        public ClientesController(IClienteService clienteService, IAdministradorService administradorService)
        {
            _clienteService = clienteService;
            _administradorService = administradorService;
        }

        #region Cliente

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Favoritos()
        {
            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            var usuario = await _clienteService.Get(id);
            if (usuario != null)
            {
                return View(await _clienteService.GetClienteAll(id));
            }
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> RemoverFavorito(int id)
        {
            var favorito = await _clienteService.GetFavoritoById(id);
            if (favorito != null)
            {
                await _clienteService.DeleteFavorito(id);
                return RedirectToAction(nameof(Favoritos));
            }

            return RedirectToAction(nameof(Favoritos));
        }

        #endregion

        #region Administrador

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListarClientes()
        {
            return View(await _clienteService.GetAll());
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(UsuarioSalvaViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                if (usuario.Senha != usuario.ConfirmacaoSenha)
                {
                    ModelState.AddModelError("Senha", "Senhas não conferem");
                    ModelState.AddModelError("ConfirmacaoSenha", "Senhas não conferem");
                    return View(usuario);
                }
                var verificarAdministrador = await _administradorService.GetEmail(usuario.Email);
                var verificarCliente = await _clienteService.GetEmail(usuario.Email);
                if (verificarAdministrador != null || verificarCliente != null)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado");
                    return View(usuario);
                }
                var cliente = new Cliente
                {
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    Senha = PasswordUtilities.PasswordHash(usuario.Senha),
                };
                await _clienteService.Add(cliente);
                return RedirectToAction(nameof(ListarClientes));
            }
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Pesquisar(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                ViewBag.CurrentFilter = nome;
                return View("ListarClientes", await _clienteService.GetNome(nome));
            }
            else
            {
                return RedirectToAction("ListarClientes");
            }
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Detalhes(int? id)
        {
            var usuario = await _clienteService.Get(id.GetValueOrDefault());
            if (id == null || usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int? id)
        {

            var usuario = await _clienteService.Get(id.GetValueOrDefault());
            if (id == null || usuario == null)
            {
                return NotFound();
            }
            UsuarioSalvaViewModel cliente = new UsuarioSalvaViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
            return View(cliente);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, UsuarioSalvaViewModel usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var verificarAdministrador = await _administradorService.GetEmail(usuario.Email);
                    var verificarCliente = await _clienteService.GetEmail(usuario.Email);
                    if (verificarAdministrador != null || verificarCliente != null)
                    {
                        ModelState.AddModelError("Email", "Email já cadastrado");
                        return View(usuario);
                    }
                    var cliente = new Cliente
                    {
                        Id = usuario.Id,
                        Email = usuario.Email,
                        Nome = usuario.Nome,
                        Senha = PasswordUtilities.PasswordHash(usuario.Senha),
                    };
                    await _clienteService.Update(id, cliente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_clienteService.Exists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListarClientes));
            }
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Deletar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _clienteService.Get(id.GetValueOrDefault());
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var usuario = await _clienteService.Get(id);
            if (usuario != null)
            {
                await _clienteService.Delete(id);
            }
            return RedirectToAction(nameof(ListarClientes));
        }

        #endregion
    }
}
