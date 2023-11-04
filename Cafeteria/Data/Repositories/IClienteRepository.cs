using Cafeteria.Models;

namespace Cafeteria.Data.Repositories
{
    public interface IClienteRepository
    {
        IEnumerable<Cliente>? GetAll();
        IEnumerable<Cliente>? GetNome(string nome);
        Cliente? Get(int id);
        Cliente? GetEmailSenha(string email, string senha);
        Cliente? GetEmail(string email);
        void Add(Cliente cliente);
        void Update(int id, Cliente cliente);
        void Delete(int id);
        bool Exists(int id);
        void SaveChanges();
    }
}
