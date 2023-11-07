using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Data.Implementations
{
    public class FavoritoRepository : IFavoritoRepository
    {
        private readonly CafeteriaContext _context;

        public FavoritoRepository(CafeteriaContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task Add(Favorito favorito)
        {
            _context.Favoritos.Add(favorito);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task Delete(int id)
        {
            var favorito = await _context.Favoritos.FindAsync(id);
            if (favorito != null)
            {
                _context.Favoritos.Remove(favorito);
            }
            await _context.SaveChangesAsync();
            return;
        }

        #endregion

        public async Task<IEnumerable<Favorito>> GetClienteAll(int clienteId)
        {
            List<Favorito> list = new List<Favorito>();
            foreach (var item in await _context.Favoritos.Include(f => f.Produto).Where(f => f.ClienteId == clienteId).ToListAsync())
            {
                if (String.IsNullOrEmpty(item.Produto.Imagem))
                {
                    item.Produto.Imagem = "sem-imagem.png";
                }
                list.Add(item);
            }
            return list;
        }

        public async Task<Favorito> Get(int id)
        {
            return await _context.Favoritos.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Favorito> GetClienteProduto(Favorito favorito)
        {
            return await _context.Favoritos.FirstOrDefaultAsync(f => f.ClienteId == favorito.ClienteId && f.ProdutoId == favorito.ProdutoId);
        }

    }
}
