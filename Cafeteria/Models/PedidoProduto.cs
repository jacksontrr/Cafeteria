using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafeteria.Models
{
    public class PedidoProduto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Pedido")]
        public int PedidoId { get; set; }
        [Required]
        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name = "Quantidade")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Apenas números são permitidos.")]
        [DataType(DataType.Text)]
        public int Quantidade { get; set; }
        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }
        [ForeignKey("ProdutoId")]
        public Produto? Produto { get; set; }
        
    }
}
