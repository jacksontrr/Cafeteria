using Cafeteria.Data.Repositories;
using Cafeteria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Cafeteria.Models;
using Cafeteria.Utilities;
using Cafeteria.ViewModels;

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

        #region Cliente
        public async Task<Cliente> GetCliente(string email, string senha)
        {
            return await _clienteRepository.GetEmailSenha(email, senha);
        }
        public async Task<Cliente> RegistrarCliente(Cliente cliente)
        {
            cliente.Senha = PasswordUtilities.PasswordHash(cliente.Senha);
            await _clienteRepository.Add(cliente);
            return cliente;
        }
        public async Task<Cliente> GetEmailCliente(string email)
        {
            return await _clienteRepository.GetEmail(email);
        }
        public async Task<Cliente> GetIdEmailCliente(int id, string email)
        {
            return await _clienteRepository.GetIdEmail(id, email);
        }
        public async Task<Cliente> UpdateCliente(int id, Cliente cliente, UsuarioSalvaViewModel model)
        {

            if (cliente.Email == model.Email)
            {
                cliente.Nome = model.Nome;
                cliente.Email = model.Email;
                cliente.Senha = model.Senha;
            }
            else
            {
                var taskCliente = GetEmailCliente(model.Email);
                var taskAdministrador = GetEmailAdministrador(model.Email);
                await Task.WhenAll(taskCliente, taskAdministrador);
                if (taskCliente.Result != null || taskAdministrador.Result != null)
                {
                    return null;
                }
                else
                {
                    cliente.Nome = model.Nome;
                    cliente.Email = model.Email;
                    cliente.Senha = model.Senha;
                }
            }
            await _clienteRepository.Update(id, cliente);
            return cliente;
        }
        public async Task<Cliente> GetIdCliente(int id)
        {
            return await _clienteRepository.Get(id);
        }
        #endregion

        #region Administrador
        public async Task<Administrador> GetAdministrador(string email, string senha)
        {
            var administrador = await _administradorRepository.GetEmailSenha(email, senha);
            return administrador;
        }
        public async Task<Administrador> RegistrarAdministrador(Administrador administrador)
        {
            administrador.Senha = PasswordUtilities.PasswordHash(administrador.Senha);
            await _administradorRepository.Add(administrador);
            return administrador;
        }
        public async Task<Administrador> GetEmailAdministrador(string email)
        {
            return await _administradorRepository.GetEmail(email);
        }
        public async Task<Administrador> GetIdEmailAdministrador(int id, string email)
        {
            return await _administradorRepository.GetIdEmail(id, email);
        }
        public async Task<Administrador> UpdateAdministrador(int id, Administrador administrador, UsuarioSalvaViewModel model)
        {
            if (administrador.Email == model.Email)
            {
                administrador.Nome = model.Nome;
                administrador.Email = model.Email;
                administrador.Senha = model.Senha;
            }
            else
            {
                var taskCliente = GetEmailCliente(model.Email);
                var taskAdministrador = GetEmailAdministrador(model.Email);
                await Task.WhenAll(taskCliente, taskAdministrador);
                if (taskCliente.Result != null || taskAdministrador.Result != null)
                {
                    return null;
                }
                else
                {
                    administrador.Nome = model.Nome;
                    administrador.Email = model.Email;
                    administrador.Senha = model.Senha;
                }
            }
            await _administradorRepository.Update(id, administrador);
            return administrador;
        }
        public async Task<Administrador> GetIdAdministrador(int id)
        {
            return await _administradorRepository.Get(id);
        }
        #endregion
    }
}
