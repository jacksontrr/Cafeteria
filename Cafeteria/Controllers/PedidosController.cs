﻿using Cafeteria.Models;
using Cafeteria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria.Controllers
{

    public class PedidosController : Controller
    {
        private readonly ICarrinhoService _cartService;
        private readonly IPedidoService _pedidoService;
        private readonly IFormaPagamentoService _formaPagamento;


        public PedidosController(ICarrinhoService cartService, IPedidoService pedidoService, IFormaPagamentoService formaPagamento)
        {
            _cartService = cartService;
            _pedidoService = pedidoService;
            _formaPagamento = formaPagamento;
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Index()
        {
            var pedidos = await _pedidoService.GetAll(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value));
            // verificar se existe pedidoProdutos se não excluir o pedido
            // await _pedidoService.checkPedido(pedidos);

            return View(pedidos);
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> Solicit(string formaPagamento)
        {
            var produtos = _cartService.GetCart().Items;

            if (produtos.ToList().Count == 0)
            {
                return RedirectToAction("Index", "Produtos");
            }

            var pedido = new Pedido
            {
                ClienteId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value),
                DataPedido = DateTime.Now,
                Status = "Pendente"
            };
            await _pedidoService.Add(pedido);

            var pagamento = new Pagamento
            {
                FormaPagamento = formaPagamento,
                PedidoId = pedido.Id
            };
            await _formaPagamento.Add(pagamento);
            foreach (var item in produtos)
            {
                var pedidoProduto = new PedidoProduto
                {
                    PedidoId = pedido.Id,
                    ProdutoId = item.Product.Id,
                    Quantidade = item.Quantity
                };
                await _pedidoService.AddPedidoProduto(pedidoProduto);
            }
            _cartService.Delete();
            return RedirectToAction("Index", "Pedidos");
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListPending()
        {
            var pedidos = await _pedidoService.GetAll(null);
            return View(pedidos);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Finish(int id)
        {
            try
            {
                if(id == 0)
                {
                    return Json(new { success = false });
                }
                var pedido = await _pedidoService.Get(id);
                pedido.Status = "Finalizado";
                await _pedidoService.Update(id, pedido);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> InPreparation(int id)
        {
            try
            {
                if (id == 0)
                {
                    return Json(new { success = false });
                }
                var pedido = await _pedidoService.Get(id);
                pedido.AdministradorId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                pedido.Status = "Em Preparo";
                await _pedidoService.Update(id, pedido);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
    }
}
