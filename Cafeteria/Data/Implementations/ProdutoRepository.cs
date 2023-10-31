using Cafeteria.Data;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;

namespace Cafeteria.Services.Implementations
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CafeteriaContext _context;
        
        public ProdutoRepository(CafeteriaContext context)
        {
            _context = context;
        }

        public void Add(Produto produto)
        {
            _context.Produtos.Add(produto);
        }
        public void Update(int id, Produto produto)
        {
            var produtoEncontrado = _context.Produtos.FirstOrDefault(x => x.Id == id);
            if (produtoEncontrado == null)
            {
                return;
            }
            produtoEncontrado.Nome = produto.Nome;
            _context.Produtos.Update(produtoEncontrado);
        }
        public void Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);
            if(produto != null)
            {
                _context.Produtos.Remove(produto);
            }
        }
        public bool Exists(int id)
        {
            return _context.Produtos.FirstOrDefault(x => x.Id == id) != null;
        }
        public Produto Get(int id)
        {
            return _context.Produtos.FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<Produto> GetAll()
        {
            return _context.Produtos;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
