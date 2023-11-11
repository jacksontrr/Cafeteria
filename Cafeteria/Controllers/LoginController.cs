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
using Cafeteria.Services.Implementations;

namespace Cafeteria.Controllers
{
    public class LoginController : Controller
    {

        private readonly ILoginService _loginService;
        private readonly ICarrinhoService _cartService;

        public LoginController(ILoginService loginService, ICarrinhoService cartService)
        {
            _loginService = loginService;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            ViewData["Layout"] = "_Login";

            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index", "Administradores");
            }

            if (User.IsInRole("Cliente"))
            {
                return RedirectToAction("Index", "Produtos");
            }

            return View();
        }

        public IActionResult Administrador()
        {
            ViewData["Layout"] = "_Login";

            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index", "Administradores");
            }

            if (User.IsInRole("Cliente"))
            {
                return RedirectToAction("Index", "Produtos");
            }

            return View();
        }

        public IActionResult Cadastrar()
        {
            ViewData["Layout"] = "_Login";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UsuarioSalvaViewModel model)
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
                // buscar verificar cliente e administrador ao mesmo tempo 
                var taskCliente = _loginService.GetEmailCliente(model.Email);
                var taskAdministrador = _loginService.GetEmailAdministrador(model.Email);

                await Task.WhenAll(taskCliente, taskAdministrador);

                if (taskCliente.Result != null || taskAdministrador.Result != null)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado");
                    return View("Cadastrar", model);
                }
                Cliente cliente = new Cliente();
                cliente.Nome = model.Nome;
                cliente.Email = model.Email;
                cliente.Senha = model.Senha;

                cliente = await _loginService.RegistrarCliente(cliente);

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
        public async Task<IActionResult> Alterar(int id, string emailOld, UsuarioSalvaViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Senha != model.ConfirmacaoSenha)
                {
                    ModelState.AddModelError("Senha", "As senhas não coincidem");
                    ModelState.AddModelError("ConfirmacaoSenha", "As senhas não coincidem");
                    return View("Configuracao", model);
                }

                var cliente = await _loginService.GetIdEmailCliente(id, emailOld);
                var administrador = await _loginService.GetIdEmailAdministrador(id, emailOld);

                if (cliente != null)
                {
                    cliente = await _loginService.UpdateCliente(id, cliente, model);
                    if (cliente == null)
                    {
                        ModelState.AddModelError("Email", "Email já cadastrado");
                        return View("Configuracao", model);
                    }
                    else
                    {
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        await SignInAsync(new UsuarioViewModel(cliente), true, false);
                        return RedirectToAction("Index", "Produtos");
                    }
                }
                else
                {
                    
                    if (administrador != null)
                    {
                        administrador = await _loginService.UpdateAdministrador(id, administrador, model);
                        if (administrador == null)
                        {
                            ModelState.AddModelError("Email", "Email já cadastrado");
                            return View("Configuracao", model);
                        }
                        else
                        {
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            await SignInAsync(new UsuarioViewModel(administrador), true, false);
                            return RedirectToAction("Index", "Produtos");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ConfirmacaoSenha", "Não conseguiu encontrar a conta");
                        return View("Configuracao", model);
                    }
                }
            }
            return View("Configuracao", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UsuarioViewModel model)
        {
            ViewData["Layout"] = "_Login";

            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _loginService.GetCliente(model.Email, model.Senha);
                    if (cliente != null)
                    {
                        await SignInAsync(new UsuarioViewModel(cliente), true, false);
                        return RedirectToAction("Index", "Produtos");
                    }

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Email", "Erro ao buscar no banco de dados.");
                    return View("Index", model);
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
                var administrador = await _loginService.GetAdministrador(model.Email, model.Senha);
                if (administrador != null)
                {
                    await SignInAsync(new UsuarioViewModel(administrador), true, true);
                    return RedirectToAction("Index", "Administradores");
                }

                ModelState.AddModelError("Email", "Email ou senha incorretos");
                return View("Administrador", model);

            }
            return View("Administrador", model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _cartService.Delete();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Produtos");
        }

        [HttpGet]
        public IActionResult AcessoNegado()
        {
            ViewData["Layout"] = "_Layout";
            return View();
        }

        public async Task<IActionResult> Configuracao()
        {
            Models.Administrador administrador;
            UsuarioSalvaViewModel usuarioSalvaViewModel = new UsuarioSalvaViewModel();
            try
            {
                var usuario = await _loginService.GetIdEmailCliente(int.Parse(User.FindFirstValue("Id")), User.FindFirstValue(ClaimTypes.Email));
                if (usuario == null)
                {
                    administrador = await _loginService.GetIdEmailAdministrador(int.Parse(User.FindFirstValue("Id")), User.FindFirstValue(ClaimTypes.Email));
                    if (administrador == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        usuarioSalvaViewModel = new UsuarioSalvaViewModel
                        {
                            Id = administrador.Id,
                            Nome = administrador.Nome,
                            Email = administrador.Email
                        };
                    }
                }
                else
                {
                    usuarioSalvaViewModel = new UsuarioSalvaViewModel
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Problem(e.Message);
            }


            return View(usuarioSalvaViewModel);
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

                _cartService.Delete();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
