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
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "O campo Confirmação Senha é obrigatório.")]
        public string ConfirmacaoSenha { get; set; }
    }
}
