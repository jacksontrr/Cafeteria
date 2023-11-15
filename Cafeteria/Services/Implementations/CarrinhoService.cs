using Cafeteria.Models;
using Cafeteria.Services.Interfaces;
using Cafeteria.Utilities;

namespace Cafeteria.Services.Implementations
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly Cart _cart;

        public CarrinhoService()
        {
            _cart = new Cart();
        }

        public void CreateUpdate(Produto produto, int quantity)
        {
            _cart.AddItem(produto, quantity);           
        }

        public void Delete()
        {
            _cart.Clear();
        }

        public void DeleteProductCart(int Produto)
        {
            _cart.RemoveItem(Produto);
        }

        public Cart GetCart()
        {
            return _cart;
        }

        public async void UpdateProductCart(Produto produto, int quantity)
        {
            _cart.UpdateItem(produto, quantity);
        }
    }
}
