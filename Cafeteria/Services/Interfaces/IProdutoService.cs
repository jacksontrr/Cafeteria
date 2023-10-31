using Cafeteria.Models;

namespace Cafeteria.Services.Interfaces
{
    public interface IProdutoService
    {
        IEnumerable<Produto> GetAll();
        Produto Get(int id);
        void Add(Produto produto);
        void Update(int id, Produto produto);
        void Delete(int id);
        bool Exists(int id);
    }
}
