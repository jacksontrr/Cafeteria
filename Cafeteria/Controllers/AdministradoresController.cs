using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Models;
using Microsoft.AspNetCore.Authorization;
using Cafeteria.Services.Interfaces;
using Cafeteria.ViewModels;
using Cafeteria.Utilities;

namespace Cafeteria.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradoresController : Controller
    {
        private readonly IAdministradorService _administradorService;
        private readonly ILoginService _loginService;

        public AdministradoresController(IAdministradorService administradorService, ILoginService loginService)
        {
            _administradorService = administradorService;
            _loginService = loginService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _administradorService.GetAll());
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

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
                var verificarAdministrador = await _loginService.GetEmailAdministrador(usuario.Email);
                var verificarCliente = await _loginService.GetEmailCliente(usuario.Email);
                if (verificarAdministrador != null || verificarCliente != null)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado");
                    return View(usuario);
                }

                var administrador = new Administrador
                {
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    Senha = PasswordUtilities.PasswordHash(usuario.Senha),
                };

                await _administradorService.Add(administrador);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            var administrador = await _administradorService.Get(id.GetValueOrDefault());
            if (administrador == null)
            {
                return NotFound();
            }
            var usuario = new UsuarioSalvaViewModel
            {
                Id = administrador.Id,
                Nome = administrador.Nome,
                Email = administrador.Email
            };
            return View(usuario);
        }

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
                    if (usuario.Senha != usuario.ConfirmacaoSenha)
                    {
                        ModelState.AddModelError("Senha", "Senhas não conferem");
                        ModelState.AddModelError("ConfirmacaoSenha", "Senhas não conferem");
                        return View(usuario);
                    }
                    var administrador = await _administradorService.Get(id);
                    administrador.Nome = usuario.Nome;
                    administrador.Email = usuario.Email;
                    administrador.Senha = PasswordUtilities.PasswordHash(usuario.Senha);
                    await _administradorService.Update(id, administrador);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_administradorService.Exists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Deletar(int? id)
        {

            var administrador = await _administradorService.Get(id.GetValueOrDefault());
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var administrador = await _administradorService.Get(id);
            if (administrador != null)
            {
                await _administradorService.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Pesquisar(string nome)
        {
            ViewBag.CurrentFilter = nome;
            if (nome == null)
            {
                return RedirectToAction("Index");
            }
            var administradores = await _administradorService.GetNome(nome);
            
            return View(nameof(Index), administradores);
        }
    }
}
