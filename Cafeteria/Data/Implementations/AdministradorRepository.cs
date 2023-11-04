using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Data.Implementations
{
    public class AdministradorRepository : IAdministradorRepository
    {

        private readonly CafeteriaContext _context;

        public AdministradorRepository(CafeteriaContext context)
        {
            _context = context;
        }

        public IEnumerable<Administrador>? GetAll()
        {
            try
            {
                var administradores = _context.Administradores;
                if (administradores != null)
                {
                    return administradores;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public IEnumerable<Administrador>? GetNome(string nome)
        {
            try
            {
                var administradores = _context.Administradores.Where(c => c.Nome.Contains(nome));
                if (administradores != null)
                {
                    return administradores;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public Administrador? Get(int id)
        {
            try
            {
                var administrador = _context.Administradores.FirstOrDefault(c => c.Id == id);
                if (administrador != null)
                {
                    return administrador;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public Administrador? GetEmail(string email)
        {
            try
            {
                var administrador = _context.Administradores.FirstOrDefault(c => c.Email == email);
                if (administrador != null)
                {
                    return administrador;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public Administrador? GetEmailSenha(string email, string senha)
        {
            try
            {
                var administrador = _context.Administradores.FirstOrDefault(c => c.Email == email);
                if (administrador != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(senha, administrador.Senha))
                    {
                        return administrador;
                    }
                }
                return null;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public void Add(Administrador administrador)
        {
            try
            {
                _context.Administradores.Add(administrador);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public void Update(int id, Administrador administrador)
        {
            try
            {
                var admin = _context.Administradores.FirstOrDefault(x => x.Id == id);
                if (admin != null)
                {
                    admin.Nome = administrador.Nome;
                    admin.Email = administrador.Email;
                    admin.Senha = administrador.Senha;
                    admin.CriptografarSenha();
                    _context.Administradores.Update(admin);
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
                var admin = _context.Administradores.FirstOrDefault(x => x.Id == id);
                if (admin != null)
                {
                    _context.Administradores.Remove(admin);
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }

        public bool Exists(int id)
        {
            return _context.Administradores.Any(x => x.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
