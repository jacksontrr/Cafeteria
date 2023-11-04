using Cafeteria.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Cafeteria.ViewModels;
using Cafeteria.Services.Interfaces;
using NuGet.Common;
using NuGet.Configuration;

namespace Cafeteria.Controllers
{
    public class LoginController : Controller
    {

        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        // tela de login
        public IActionResult Index()
        {
            ViewData["Layout"] = "_Login";

            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index", "Administrador");
            }

            if (User.IsInRole("Cliente"))
            {
                return RedirectToAction("Index", "Produtos");
            }

            return View();
        }

        // tela de login
        public IActionResult Administrador()
        {
            ViewData["Layout"] = "_Login";

            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index", "Administrador");
            }

            if (User.IsInRole("Cliente"))
            {
                return RedirectToAction("Index", "Produtos");
            }

            return View();
        }

        // tela de cadastrar cliente
        public IActionResult Cadastrar()
        {
            ViewData["Layout"] = "_Login";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UsuarioCadastroViewModel model, bool admin)
        {
            ViewData["Layout"] = "_Login";
            if (ModelState.IsValid)
            {

                if (model.Senha != model.ConfirmacaoSenha)
                {
                    ModelState.AddModelError("Senha", "As senhas não coincidem");
                    ModelState.AddModelError("ConfirmacaoSenha", "As senhas não coincidem");
                    return View("Cadastrar", model);
                }
                if (_loginService.GetEmailCliente(model.Email) != null || _loginService.GetEmailAdministrador(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado");
                    return View("Cadastrar", model);
                }
                Cliente cliente = new Cliente();
                cliente.Nome = model.Nome;
                cliente.Email = model.Email;
                cliente.Senha = model.Senha;

                cliente = _loginService.RegistrarCliente(cliente);

                UsuarioViewModel usuario = new UsuarioViewModel
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Senha = cliente.Senha
                };

                await SignInAsync(usuario, true, false);

                return RedirectToAction("Index", "Produtos");
            }
            return View("Cadastrar", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UsuarioViewModel model)
        {
            ViewData["Layout"] = "_Login";

            if (ModelState.IsValid)
            {
                var cliente = _loginService.GetCliente(model.Email, model.Senha);
                if (cliente != null)
                {
                    UsuarioViewModel usuario = new UsuarioViewModel
                    {
                        Id = cliente.Id,
                        Nome = cliente.Nome,
                        Email = cliente.Email,
                        Senha = cliente.Senha
                    };
                    await SignInAsync(usuario, true, false);
                    return RedirectToAction("Index", "Produtos");
                }
                ModelState.AddModelError("Email", "Email ou senha incorretos");
                return View("Index", model);

            }
            return View("Index", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Administrador(UsuarioViewModel model)
        {
            ViewData["Layout"] = "_Login";

            if (ModelState.IsValid)
            {
                var administrador = _loginService.GetAdministrador(model.Email, model.Senha);
                if (administrador != null)
                {
                    UsuarioViewModel usuario = new UsuarioViewModel
                    {
                        Id = administrador.Id,
                        Nome = administrador.Nome,
                        Email = administrador.Email,
                        Senha = administrador.Senha
                    };
                    await SignInAsync(model, true, true);

                    return RedirectToAction("Index", "Administrador");
                }

                ModelState.AddModelError("Email", "Email ou senha incorretos");
                return View("Index", model);

            }
            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Produtos");
        }

        [HttpGet]
        public IActionResult AcessoNegado()
        {
            ViewData["Layout"] = "_Layout";
            return View();
        }

        protected async Task SignInAsync(UsuarioViewModel user, bool isPersistent, bool admin)
        {

            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Cliente")
                };
                if (admin)
                {
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Nome),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("Id", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, "Administrador")
                    };
                }


                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = isPersistent,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
                };


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
