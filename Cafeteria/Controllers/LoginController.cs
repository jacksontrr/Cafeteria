using Cafeteria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Cafeteria.Controllers
{
    public class LoginController : Controller
    {
        // tela de login
        public IActionResult Index()
        {
            ViewData["Layout"] = "_Login";
            return View();
        }

        // tela de cadastrar cliente
        public IActionResult Cadastrar()
        {
            ViewData["Layout"] = "_Login";
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Registro(ClienteRegistroViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var cliente = new Cliente
        //        {
        //            UserName = model.Email,
        //            Email = model.Email,
        //            Nome = model.Nome
        //        };

        //        var result = await UserManager.CreateAsync(cliente, model.Senha);

        //        if (result.Succeeded)
        //        {
        //            await SignInManager.SignInAsync(cliente, isPersistent: false);
        //            // Redirecionar para a página desejada após o registro bem-sucedido
        //            return RedirectToAction("Index", "Home");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    // Se o registro falhar, retorne a mesma página com erros
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(ClienteLoginViewModel model, string returnUrl = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await SignInManager.PasswordSignInAsync(model.Email, model.Senha, isPersistent: false, lockoutOnFailure: false);

        //        if (result.Succeeded)
        //        {
        //            // Redirecionar para a página desejada após o login bem-sucedido
        //            return LocalRedirect(returnUrl);
        //        }

        //        ModelState.AddModelError(string.Empty, "Falha na autenticação.");
        //    }

        //    // Se a autenticação falhar, retorne a mesma página com erros
        //    return View(model);
        //}

    }
}
