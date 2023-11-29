using Cafeteria.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cafeteria.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string Senha { get; set; }
        public string? ConfirmacaoSenha { get; set; }

        public string GerarToken()
        {
            return BCrypt.Net.BCrypt.HashString(Email + "_" + Senha + "_" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        public UsuarioViewModel(Cliente cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Email = cliente.Email;
            Senha = cliente.Senha;
        }

        public UsuarioViewModel(Administrador administrador)
        {
            Id = administrador.Id;
            Nome = administrador.Nome;
            Email = administrador.Email;
            Senha = administrador.Senha;
        }

        public UsuarioViewModel()
        {
        }
    }
}
