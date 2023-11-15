using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafeteria.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        
        [Display(Name = "Administrador")]
        public int? AdministradorId { get; set; }
        
        public string? Status { get; set; }
        
        public DateTime? DataPedido { get; set; }
        
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
        
        [ForeignKey("AdministradorId")]
        public Administrador? Administrador { get; set; }

        public ICollection<PedidoProduto> PedidoProdutos { get; set; }
    }
}
