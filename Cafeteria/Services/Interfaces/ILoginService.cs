using Cafeteria.Models;
using Cafeteria.ViewModels;

namespace Cafeteria.Services.Interfaces
{
    public interface ILoginService
    {
        Task<Cliente> GetCliente(string email, string senha);
        Task<Cliente> RegistrarCliente(Cliente cliente);
        Task<Cliente> GetEmailCliente(string email);
        Task<Cliente> GetIdEmailCliente(int id, string email);
        Task<Cliente> GetIdCliente(int id);
        Task<Cliente> UpdateCliente(int id, Cliente cliente, UsuarioSalvaViewModel model);

        Task<Administrador> RegistrarAdministrador(Administrador administrador);
        Task<Administrador> GetAdministrador(string email, string senha);
        Task<Administrador> GetIdEmailAdministrador(int id, string email);
        Task<Administrador> GetIdAdministrador(int id);
        Task<Administrador> GetEmailAdministrador(string email);
        Task<Administrador> UpdateAdministrador(int id, Administrador administrador, UsuarioSalvaViewModel model);
    }
}
