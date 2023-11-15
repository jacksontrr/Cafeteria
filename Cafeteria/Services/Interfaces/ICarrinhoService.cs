using Cafeteria.Models;
using Cafeteria.Utilities;

namespace Cafeteria.Services.Interfaces
{
    public interface ICarrinhoService
    {
        Cart GetCart();
        void CreateUpdate(Produto produto, int quantity);
        void Delete();
        void DeleteProductCart(int Produto);
        void UpdateProductCart(Produto Produto, int quantity);
    }
}
