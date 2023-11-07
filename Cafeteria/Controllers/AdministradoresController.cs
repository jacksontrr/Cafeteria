using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Data;
using Cafeteria.Models;
using Microsoft.AspNetCore.Authorization;
using Cafeteria.Services.Interfaces;
using Cafeteria.Services.Implementations;

namespace Cafeteria.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradoresController : Controller
    {
        private readonly IAdministradorService _administradorService;

        public AdministradoresController(IAdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _administradorService.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                var verificar = await _administradorService.GetEmail(administrador.Email);
                if(verificar != null)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado");
                    return View(administrador);
                }
                await _administradorService.Add(administrador);
                return RedirectToAction(nameof(Index));
            }
            return View(administrador);
        }

        // GET: Administradores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var administrador = await _administradorService.Get(id.GetValueOrDefault());
            if (id == null || administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // POST: Administradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Administrador administrador)
        {
            if (id != administrador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(administrador);
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
            return View(administrador);
        }

        // GET: Administradores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // delete
            var administrador = await _administradorService.Get(id.GetValueOrDefault());
            if (administrador == null || id == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // POST: Administradores/Delete/5
        [HttpPost, ActionName("Delete")]
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
    }
}
