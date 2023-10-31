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
        [Required]
        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }
        [Required]
        [Display(Name = "Administrador")]
        public int AdministradorId { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name = "Quantidade")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Apenas números são permitidos.")]
        [DataType(DataType.Text)]
        public int Quantidade { get; set; }
        public string? Status { get; set; }
        public DateTime? DataPedido { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
        [ForeignKey("ProdutoId")]
        public Produto? Produto { get; set; }
        [ForeignKey("AdministradorId")]
        public Administrador? Administrador { get; set; }
    }
}
