using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cafeteria.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id {  get; set; }
        public string? Nome { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string Senha { get; set; } 
        public string? ConfirmacaoSenha { get; set; }

        public string GerarToken()
        {
            return BCrypt.Net.BCrypt.HashString(Email +"_"+ Senha + "_" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

    }
}
