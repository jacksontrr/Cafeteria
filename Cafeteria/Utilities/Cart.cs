using Cafeteria.Models;

namespace Cafeteria.Utilities
{
    public class Cart
    {
        private List<CartItem> _items = new List<CartItem>();

        public decimal Total => _items.Sum(i => i.Product.Preco * i.Amount);

        public IEnumerable<CartItem> Items => _items.AsReadOnly();

        public int Count => _items.Count;



        public void AddItem(Produto product, int amount)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (item == null)
            {
                _items.Add(new CartItem { Product = product, Amount = amount });
            }
            else
            {
                item.Amount += amount;
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
    }
    public class CartItem
    {
        public Produto Product { get; set; }
        public int Amount { get; set; }
    }

}
