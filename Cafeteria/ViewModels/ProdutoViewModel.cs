using Cafeteria.Models;
using System.ComponentModel.DataAnnotations;

namespace Cafeteria.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo Nome deve ter entre 3 e 50 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O campo Preço é obrigatório.")]
        [Range(0.01, 9999999999999999.99, ErrorMessage = "O campo Preço deve ser maior que zero.")]
        [DataType(DataType.Currency)]
        public decimal Preco { get; set; }
        [StringLength(255)]
        public string? Imagem { get; set; }
        [Display(Name = "Imagem")]
        [DataType(DataType.Upload)]       
        public IFormFile? Arquivo { get; set; }
        public bool RemoverImagem { get; set; }

    }
}
