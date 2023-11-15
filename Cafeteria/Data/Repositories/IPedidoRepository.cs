using Cafeteria.Models;

namespace Cafeteria.Data.Repositories
{
    public interface IPedidoRepository
    {
        #region CRUD Pedido
        Task Add(Pedido pedido);
        Task Update(int id, Pedido pedido);
        Task Delete(int id);
        Task<IEnumerable<Pedido>> GetAll(int? clienteId);
        Task<Pedido> Get(int id);
        #endregion

        #region CRUD PedidoProduto
        Task AddPedidoProduto(PedidoProduto pedidoProduto);
        Task UpdatePedidoProduto(int id, PedidoProduto pedidoProduto);
        Task DeletePedidoProduto(int id);
        Task<IEnumerable<PedidoProduto>> GetAllPedidoProduto(int? pedidoId);
        Task<PedidoProduto> GetPedidoProduto(int id);
        #endregion
    }
}
