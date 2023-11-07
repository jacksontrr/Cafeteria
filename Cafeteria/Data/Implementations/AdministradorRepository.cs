using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Cafeteria.Services.Implementations;
using Cafeteria.Utilities;
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

        #region CRUD

        public async Task Add(Administrador administrador)
        {
            _context.Administradores.Add(administrador);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Administrador administrador)
        {
            var admin = _context.Administradores.FirstOrDefault(x => x.Id == id);
            if (admin != null)
            {
                admin.Nome = administrador.Nome;
                admin.Email = administrador.Email;
                admin.Senha = PasswordUtilities.PasswordHash(administrador.Senha);
                _context.Administradores.Update(admin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var admin = _context.Administradores.FirstOrDefault(x => x.Id == id);
            if (admin != null)
            {
                _context.Administradores.Remove(admin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Administrador>> GetAll()
        {
            return await _context.Administradores.ToListAsync();
        }

        #endregion

        #region Login

        public async Task<Administrador> GetEmailSenha(string email, string senha)
        {
            var administrador = await _context.Administradores.FirstOrDefaultAsync(c => c.Email == email);
            if (administrador != null)
            {

                if (PasswordUtilities.PasswordVerify(senha, administrador.Senha))
                {
                    return administrador;
                }
            }
            return null;
        }

        #endregion

        public async Task<IEnumerable<Administrador>> GetNome(string nome)
        {
            List<Administrador> list = new List<Administrador>();
            foreach (var administrador in _context.Administradores)
            {
                string nomeDB = CharacterTreatment.RemoveDiacritics(administrador.Nome).ToLower();
                nome = CharacterTreatment.RemoveDiacritics(nome).ToLower();
                if (nomeDB.Contains(nome))
                {
                    list.Add(administrador);
                }
            }
            return list;
        }

        public async Task<Administrador> Get(int id)
        {
            return await _context.Administradores.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Administrador> GetIdEmail(int id, string email)
        {
            return await _context.Administradores.FirstOrDefaultAsync(c => c.Id == id && c.Email == email);
        }

        public async Task<Administrador> GetEmail(string email)
        {
            return await _context.Administradores.FirstOrDefaultAsync(c => c.Email == email);
        }

        public bool Exists(int id)
        {
            return _context.Administradores.Any(x => x.Id == id);
        }
    }
}
