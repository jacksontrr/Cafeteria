using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;

namespace Cafeteria.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _produtoRepository;

        public PedidoService(IPedidoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        #region CRUD Pedido
        public async Task Add(Pedido pedido)
        {
            await _produtoRepository.Add(pedido);
        }

        public async Task Update(int id, Pedido pedido)
        {
            await _produtoRepository.Update(id, pedido);
        }

        public async Task Delete(int id)
        {
            await _produtoRepository.Delete(id);
        }

        public async Task<IEnumerable<Pedido>> GetAll(int? clienteId)
        {
            return await _produtoRepository.GetAll(clienteId);
        }

        public async Task<Pedido> Get(int id)
        {
            return await _produtoRepository.Get(id);
        }
        #endregion

        #region CRUD PedidoProduto
        public async Task AddPedidoProduto(PedidoProduto pedidoProduto)
        {
            await _produtoRepository.AddPedidoProduto(pedidoProduto);
        }

        public async Task UpdatePedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            await _produtoRepository.UpdatePedidoProduto(id, pedidoProduto);
        }

        public async Task DeletePedidoProduto(int id)
        {
            await _produtoRepository.DeletePedidoProduto(id);
        }

        public async Task<IEnumerable<PedidoProduto>> GetAllPedidoProduto(int? pedidoId)
        {
            return await _produtoRepository.GetAllPedidoProduto(pedidoId);
        }

        public async Task<PedidoProduto> GetPedidoProduto(int id)
        {
            return await _produtoRepository.GetPedidoProduto(id);
        }
        #endregion

        public async Task checkPedido(IEnumerable<Pedido> pedido)
        {
            foreach(var item in pedido)
            {
                var pedidoProdutos = await GetAllPedidoProduto(item.Id);
                if(pedidoProdutos.ToList().Count == 0)
                {
                    await Delete(item.Id);
                }
            }
        }
    }
}
