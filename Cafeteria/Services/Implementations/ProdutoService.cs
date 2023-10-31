using Cafeteria.Data;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;

namespace Cafeteria.Services.Implementations
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public void Add(Produto produto)
        {
            _produtoRepository.Add(produto);
            _produtoRepository.SaveChanges();
        }

        public void Delete(int id)
        {
            _produtoRepository.Delete(id);
            _produtoRepository.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _produtoRepository.Exists(id);
        }

        public Produto Get(int id)
        {
            return _produtoRepository.Get(id);
        }

        public IEnumerable<Produto> GetAll()
        {
            return _produtoRepository.GetAll();
        }

        public void Update(int id, Produto produto)
        {
            _produtoRepository.Update(id, produto);
            _produtoRepository.SaveChanges();
        }
    }
}
