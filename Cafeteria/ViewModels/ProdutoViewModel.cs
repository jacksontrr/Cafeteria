using Cafeteria.Models;

namespace Cafeteria.ViewModels
{
    public class ProdutoViewModel : Produto
    {
        public IFormFile? Arquivo { get; set; }

    }
}
