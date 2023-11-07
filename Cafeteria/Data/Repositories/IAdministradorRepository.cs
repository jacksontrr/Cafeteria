using Cafeteria.Models;

namespace Cafeteria.Data.Repositories
{
    public interface IAdministradorRepository
    {
        #region CRUD
        Task Update(int id, Administrador administrador);
        Task Delete(int id);
        Task<IEnumerable<Administrador>> GetAll();
        Task Add(Administrador administrador);
        #endregion
        #region Login
        Task<Administrador> GetEmailSenha(string email, string senha);
        #endregion
        Task<IEnumerable<Administrador>> GetNome(string nome);
        Task<Administrador> GetEmail(string email);
        Task<Administrador> Get(int id);
        Task<Administrador> GetIdEmail(int id, string email);
        bool Exists(int id);
    }
}
