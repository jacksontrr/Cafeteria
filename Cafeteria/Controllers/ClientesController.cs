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

namespace Cafeteria.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _clienteService.GetAll());
        }

        public async Task<IActionResult> Search(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.CurrentFilter = search;
                return View("Index", await _clienteService.GetNome(search));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var usuario = await _clienteService.Get(id.GetValueOrDefault());
            if (id == null || usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente usuario)
        {
            if (ModelState.IsValid)
            {
                await _clienteService.Add(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var usuario = await _clienteService.Get(id.GetValueOrDefault());
            if (id == null || usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _clienteService.Update(id, usuario);
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
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var usuario = await _clienteService.Get(id);
            if (usuario != null)
            {
                await _clienteService.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }

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
    }
}
