using Cafeteria.Models;
using System.ComponentModel;

namespace Cafeteria.Services.Interfaces
{
    public interface IProdutoRepository 
    {
        #region CRUD
        Task Add(Produto produto);
        Task Update(int id, Produto produto);
        Task Delete(int id);
        Task<IEnumerable<Produto>> GetAll(int? id);
        #endregion
        Task<IEnumerable<Produto>> GetNome(string nome);
        Task<Produto> Get(int id);
        bool Exists(int id);
    }
}
