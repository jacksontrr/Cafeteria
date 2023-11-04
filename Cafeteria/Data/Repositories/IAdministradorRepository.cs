using Cafeteria.Models;

namespace Cafeteria.Data.Repositories
{
    public interface IAdministradorRepository
    {
        IEnumerable<Administrador>? GetAll();
        IEnumerable<Administrador>? GetNome(string nome);
        Administrador? GetEmailSenha(string email, string senha);
        Administrador? GetEmail(string email);
        Administrador? Get(int id);
        void Add(Administrador administrador);
        void Update(int id, Administrador administrador);
        void Delete(int id);
        bool Exists(int id);
        void SaveChanges();
    }
}
