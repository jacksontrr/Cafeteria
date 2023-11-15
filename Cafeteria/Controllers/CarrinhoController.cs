using Cafeteria.Services.Interfaces;
using Cafeteria.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class CarrinhoController : Controller
    {
        private readonly ICarrinhoService _cartService;
        private readonly IProdutoService _produtoService;

        public CarrinhoController(ICarrinhoService cartService, IProdutoService produtoService)
        {
            _cartService = cartService;
            _produtoService = produtoService;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var produto = await _produtoService.Get(productId);
            _cartService.CreateUpdate(produto, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveProductCart(int productId)
        {
            _cartService.DeleteProductCart(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductCart(int productId, int quantity)
        {
            var produto = await _produtoService.Get(productId);
            _cartService.UpdateProductCart(produto, quantity);
            return RedirectToAction("Index");
        }
        

        public IActionResult ClearCart()
        {
            _cartService.Delete();
            return RedirectToAction("Index", "Carrinho");
        }
    }
}
