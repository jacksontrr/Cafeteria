using Cafeteria.Models;
using System.ComponentModel;

namespace Cafeteria.Services.Interfaces
{
    public interface IProdutoRepository 
    {
        IEnumerable<Produto> GetAll();
        Produto Get(int id);
        void Add(Produto produto);
        void Update(int id, Produto produto);
        void Delete(int id);
        bool Exists(int id);
        void SaveChanges();
    }
}
