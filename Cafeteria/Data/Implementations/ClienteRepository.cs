using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;

namespace Cafeteria.Data.Implementations
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly CafeteriaContext _context;

        public ClienteRepository(CafeteriaContext context)
        {
            _context = context;
        }

        public IEnumerable<Cliente>? GetAll()
        {
            try
            {
                var clientes = _context.Clientes;
                if (clientes != null)
                {
                    return clientes;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public IEnumerable<Cliente>? GetNome(string nome)
        {
            try
            {
                var clientes = _context.Clientes.Where(c => c.Nome.Contains(nome));
                if (clientes != null)
                {
                    return clientes;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public Cliente? Get(int id)
        {
            try
            {
                var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);
                if (cliente != null)
                {
                    return cliente;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public Cliente? GetEmail(string email)
        {
            try
            {
                var cliente = _context.Clientes.FirstOrDefault(c => c.Email == email);
                if (cliente != null)
                {
                    return cliente;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public Cliente? GetEmailSenha(string email, string senha)
        {
            try
            {
                var cliente = _context.Clientes.FirstOrDefault(c => c.Email == email);
                if (cliente != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(senha, cliente.Senha))
                    {
                        return cliente;
                    }
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public void Add(Cliente cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public void Update(int id, Cliente cliente)
        {
            try
            {
                var clienteToUpdate = _context.Clientes.FirstOrDefault(c => c.Id == id);
                if (clienteToUpdate != null)
                {
                    clienteToUpdate.Nome = cliente.Nome;
                    clienteToUpdate.Email = cliente.Email;
                    clienteToUpdate.Senha = cliente.Senha;
                    clienteToUpdate.CriptografarSenha();
                    _context.Clientes.Update(clienteToUpdate);
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var clienteToDelete = _context.Clientes.FirstOrDefault(x => x.Id == id);
                if (clienteToDelete != null)
                {
                    _context.Clientes.Remove(clienteToDelete);
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public bool Exists(int id)
        {
            return _context.Clientes.Any(c => c.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
