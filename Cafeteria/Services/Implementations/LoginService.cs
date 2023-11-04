using Cafeteria.Data.Repositories;
using Cafeteria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Cafeteria.Models;

namespace Cafeteria.Services.Implementations
{
    public class LoginService : ILoginService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IAdministradorRepository _administradorRepository;

        public LoginService(IClienteRepository clienteRepository, IAdministradorRepository administradorRepository)
        {
            _clienteRepository = clienteRepository;
            _administradorRepository = administradorRepository;
        }

        public Administrador? GetAdministrador(string email, string senha)
        {
            var administrador = _administradorRepository.GetEmailSenha(email, senha);
            return administrador;
        }

        public Cliente? GetCliente(string email, string senha)
        {
            var cliente = _clienteRepository.GetEmailSenha(email, senha);
            return cliente;
        }

        public Administrador RegistrarAdministrador(Administrador administrador)
        {
            administrador.Senha = BCrypt.Net.BCrypt.HashPassword(administrador.Senha);
            _administradorRepository.Add(administrador);
            _administradorRepository.SaveChanges();
            return administrador;
        }

        public Cliente RegistrarCliente(Cliente cliente)
        {
            cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);
            _clienteRepository.Add(cliente);
            _clienteRepository.SaveChanges();
            return cliente;
        }

        public Cliente? GetEmailCliente(string email)
        {
            var cliente = _clienteRepository.GetEmail(email);
            if(cliente == null)
            {
                cliente = null;
            }
            return cliente;
        }

        public Administrador? GetEmailAdministrador(string email)
        {
            var administrador = _administradorRepository.GetEmail(email);
            if(administrador == null)
            {
                administrador = null;
            }
            return administrador;
        }
    }
}
