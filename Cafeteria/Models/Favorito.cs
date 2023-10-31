using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafeteria.Models
{
    public class Favorito
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        [Required]
        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
        [ForeignKey("ProdutoId")]
        public Produto? Produto { get; set; }
    }
}
