using System.ComponentModel.DataAnnotations;

namespace Cafeteria.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Senha { get; set; }

        public ICollection<Pedido>? Pedidos { get; set; }
        public ICollection<Favorito>? Favoritos { get; set; }

    }
}
