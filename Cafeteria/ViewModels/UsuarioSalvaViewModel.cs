using Cafeteria.Models;
using System.ComponentModel.DataAnnotations;

namespace Cafeteria.ViewModels
{
    public class UsuarioSalvaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email está em formato inválido.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        [Required(ErrorMessage = "O campo Confirmação Senha é obrigatório.")]
        [Compare("Senha", ErrorMessage = "As senhas inseridas não correspondem.")]
        [Display(Name = "Confirmar Senha")]
        [DataType(DataType.Password)]
        public string ConfirmacaoSenha { get; set; }
    }
}
