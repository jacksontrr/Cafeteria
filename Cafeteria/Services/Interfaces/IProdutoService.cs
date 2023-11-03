using Cafeteria.Models;
using Cafeteria.ViewModels;

namespace Cafeteria.Services.Interfaces
{
    public interface IProdutoService
    {
        IEnumerable<Produto> GetAll();
        IEnumerable<Produto> GetNome(string nome);
        Produto Get(int id);
        void Add(Produto produto);
        void Update(int id, Produto produto);
        void Delete(int id);
        bool Exists(int id);
        bool SaveFile(ref ProdutoViewModel produtoViewModel);
        void DeleteFile(string? fileName);
        bool CheckIfItHasBeenChanged(Produto Antigo, Produto Novo);
    }
}
