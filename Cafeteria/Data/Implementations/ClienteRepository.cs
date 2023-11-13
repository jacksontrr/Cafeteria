using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Cafeteria.Utilities;
using Microsoft.EntityFrameworkCore;


namespace Cafeteria.Data.Implementations
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly CafeteriaContext _context;

        public ClienteRepository(CafeteriaContext context)
        {
            _context = context;
        }

        #region CRUD
        public async Task Add(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Cliente cliente)
        {
            var clienteToUpdate = _context.Clientes.FirstOrDefault(c => c.Id == id);
            if (clienteToUpdate != null)
            {
                clienteToUpdate.Nome = cliente.Nome;
                clienteToUpdate.Email = cliente.Email;
                clienteToUpdate.Senha = PasswordUtilities.PasswordHash(cliente.Senha);
                _context.Clientes.Update(clienteToUpdate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var clienteToDelete = _context.Clientes.FirstOrDefault(x => x.Id == id);
            if (clienteToDelete != null)
            {
                _context.Clientes.Remove(clienteToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Cliente>> GetAll()
        {
            return await _context.Clientes.ToListAsync();
        }

        #endregion

        #region Login
        
        public async Task<Cliente> GetEmailSenha(string email, string senha)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
            if (cliente != null)
            {
                if (PasswordUtilities.PasswordVerify(senha, cliente.Senha))
                {
                    return cliente;
                }
            }
            return null;
        }

        #endregion

        public async Task<IEnumerable<Cliente>> GetNome(string nome)
        {
            List<Cliente> list = new List<Cliente>();
            foreach (var administrador in await _context.Clientes.ToListAsync())
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

        public async Task<Cliente> Get(int id)
        {
            return await _context.Clientes.Include(f => f.Favoritos).Include(p => p.Pedidos).ThenInclude(pp => pp.PedidoProdutos).ThenInclude(p => p.Produto).FirstOrDefaultAsync(x => x.Id == id); 
        }
        public async Task<Cliente> GetIdEmail(int id, string email)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id && c.Email == email);
        }

        public async Task<Cliente> GetEmail(string email)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
        }

        public bool Exists(int id)
        {
            return _context.Clientes.Any(c => c.Id == id);
        }
    }
}
