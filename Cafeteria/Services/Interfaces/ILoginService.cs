using Cafeteria.Models;

namespace Cafeteria.Services.Interfaces
{
    public interface ILoginService
    {
        Administrador RegistrarAdministrador(Administrador administrador);
        Administrador? GetAdministrador(string email, string senha);
        Cliente? GetCliente(string email, string senha);
        Cliente RegistrarCliente(Cliente cliente);

        Cliente? GetEmailCliente(string email);
        Administrador? GetEmailAdministrador(string email);
    }
}
