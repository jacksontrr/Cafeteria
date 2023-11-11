using Cafeteria.Models;

namespace Cafeteria.Utilities
{
    public class Cart
    {
        private List<CartItem> _items = new List<CartItem>();

        public decimal Total => _items.Sum(i => i.Product.Preco * i.Quantity);

        public IEnumerable<CartItem> Items => _items.AsReadOnly();

        public int Count => _items.Count;


        public void AddItem(Produto product, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (item == null)
            {
                _items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void RemoveItem(int productId)
        {
            _items.RemoveAll(i => i.Product.Id == productId);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void UpdateItem(Produto produto, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == produto.Id);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }
    }
    public class CartItem
    {
        public Produto Product { get; set; }
        public int Quantity { get; set; }
    }

}
