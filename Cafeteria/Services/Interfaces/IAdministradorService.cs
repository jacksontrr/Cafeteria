using Cafeteria.Models;
using Cafeteria.ViewModels;

namespace Cafeteria.Services.Interfaces
{
    public interface IAdministradorService
    {
        Task<IEnumerable<Administrador>> GetAll();
        Task<IEnumerable<Administrador>> GetNome(string nome);
        Task<Administrador> Get(int id);
        Task Add(Administrador administrador);
        Task Update(int id, Administrador administrador);
        Task Delete(int id);
        bool Exists(int id);
        Task<Administrador> GetEmail(string email);

    }
}
