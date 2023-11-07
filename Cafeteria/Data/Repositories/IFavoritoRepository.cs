using Cafeteria.Models;

namespace Cafeteria.Data.Repositories
{
    public interface IFavoritoRepository
    {
        #region CRUD
        Task<IEnumerable<Favorito>> GetClienteAll(int clienteId);
        Task Add(Favorito favorito);
        Task Delete(int id);
        #endregion
        Task<Favorito> Get(int id);
        Task<Favorito> GetClienteProduto(Favorito favorito);
        
    }
}
