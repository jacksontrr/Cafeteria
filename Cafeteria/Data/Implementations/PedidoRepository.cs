using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Data.Implementations
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly CafeteriaContext _context;

        public PedidoRepository(CafeteriaContext context)
        {
            _context = context;
        }

        #region CRUD Pedido
        public async Task Add(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Pedido pedido)
        {
            var pedidoEncontrado = _context.Pedidos.FirstOrDefault(x => x.Id == id);
            if (pedidoEncontrado == null)
            {
                return;
            }
            pedidoEncontrado.ClienteId = pedido.ClienteId;
            pedidoEncontrado.DataPedido = pedido.DataPedido;
            pedidoEncontrado.Status = pedido.Status;

            _context.Pedidos.Update(pedidoEncontrado);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var pedido = _context.Pedidos.FirstOrDefault(x => x.Id == id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Pedido>> GetAll(int? clienteId)
        {
            // Iniciar a lista de pedidos
            List<Pedido> pedidos;

            if (clienteId.HasValue)
            {
                pedidos = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.PedidoProdutos)
                    .ThenInclude(pp => pp.Produto)
                    .Where(p => p.ClienteId == clienteId)
                    .ToListAsync();
            }
            else
            {
                pedidos = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.PedidoProdutos)
                    .ThenInclude(pp => pp.Produto)
                    .ToListAsync();
            }

            return pedidos;
        }

        public async Task<Pedido> Get(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.PedidoProdutos)
                .ThenInclude(pp => pp.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(pedido == null)
            {
                return null;
            }

            return pedido;
        }

        #endregion

        #region CRUD PedidoProduto
        public async Task AddPedidoProduto(PedidoProduto pedidoProduto)
        {
            _context.PedidoProdutos.Add(pedidoProduto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            var pedidoProdutoEncontrado = _context.PedidoProdutos.FirstOrDefault(x => x.Id == id);
            if (pedidoProdutoEncontrado == null)
            {
                return;
            }
            pedidoProdutoEncontrado.PedidoId = pedidoProduto.PedidoId;
            pedidoProdutoEncontrado.ProdutoId = pedidoProduto.ProdutoId;
            pedidoProdutoEncontrado.Quantidade = pedidoProduto.Quantidade;

            _context.PedidoProdutos.Update(pedidoProdutoEncontrado);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePedidoProduto(int id)
        {
            var pedidoProduto = _context.PedidoProdutos.FirstOrDefault(x => x.Id == id);
            if (pedidoProduto != null)
            {
                _context.PedidoProdutos.Remove(pedidoProduto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PedidoProduto>> GetAllPedidoProduto(int? pedidoId)
        {
            // Iniciar a lista de pedidos
            List<PedidoProduto> pedidoProdutos;

            if (pedidoId.HasValue)
            {
                pedidoProdutos = await _context.PedidoProdutos
                    .Include(pp => pp.Pedido)
                    .Include(pp => pp.Produto)
                    .Where(pp => pp.PedidoId == pedidoId)
                    .ToListAsync();
            }
            else
            {
                pedidoProdutos = await _context.PedidoProdutos
                    .Include(pp => pp.Pedido)
                    .Include(pp => pp.Produto)
                    .ToListAsync();
            }

            return pedidoProdutos;
        }

        public async Task<PedidoProduto> GetPedidoProduto(int id)
        {
            var pedidoProduto = await _context.PedidoProdutos
                .Include(pp => pp.Pedido)
                .Include(pp => pp.Produto)
                .FirstOrDefaultAsync(pp => pp.Id == id);

            if (pedidoProduto == null)
            {
                return null;
            }

            return pedidoProduto;
        }
        
        #endregion
    }
}
