using Cafeteria.Models;

namespace Cafeteria.Data.Repositories
{
    public interface IClienteRepository
    {
        #region CRUD
        Task<IEnumerable<Cliente>> GetAll();
        Task Add(Cliente cliente);
        Task Update(int id, Cliente cliente);
        Task Delete(int id);
        #endregion
        #region Login
        Task<Cliente> GetEmailSenha(string email, string senha);
        #endregion
        Task<IEnumerable<Cliente>> GetNome(string nome);
        Task<Cliente> Get(int id);
        Task<Cliente> GetEmail(string email);
        bool Exists(int id);
        Task<Cliente> GetIdEmail(int id, string email);
    }
}
