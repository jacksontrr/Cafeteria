using Cafeteria.Models;

namespace Cafeteria.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAll();
        Task<IEnumerable<Cliente>> GetNome(string nome);
        Task<Cliente> Get(int id);
        Task Add(Cliente cliente);
        Task Update(int id, Cliente cliente);
        Task Delete(int id);
        bool Exists(int id);
        Task SaveFavorite(Favorito favorito);
        Task<IEnumerable<Favorito>> GetClienteAll(int clienteId);
        Task DeleteFavorito(int id);
        Task<Favorito> GetFavoritoById(int id);
    }
}
