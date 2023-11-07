using Cafeteria.Models;
using Cafeteria.ViewModels;

namespace Cafeteria.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> GetAll(int? clienteId);
        Task<IEnumerable<Produto>> GetNome(string nome);
        Task<Produto> Get(int id);
        Task Add(Produto produto);
        Task Update(int id, Produto produto);
        Task Delete(int id);
        bool Exists(int id);
        bool SaveFile(ref ProdutoViewModel produtoViewModel);
        void DeleteFile(string? fileName);
        bool CheckIfItHasBeenChanged(Produto Antigo, Produto Novo);
        Task SaveFavorite(Favorito favorito);
        Task<Favorito> GetFavorito(Favorito favorito);
        Task DeleteFavorite(int id);
    }
}
