using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafeteria.Models
{
    public class Pagamento
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Pedido")]
        public int PedidoId { get; set; }
        [Required]
        [Display(Name = "Forma de Pagamento")]
        public string FormaPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
        
        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }
    }
}
