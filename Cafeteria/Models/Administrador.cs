﻿using System.ComponentModel.DataAnnotations;

namespace Cafeteria.Models
{
    public class Administrador
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Senha { get; set; }

        public void CriptografarSenha()
        {
            Senha = BCrypt.Net.BCrypt.HashPassword(Senha);
        }
    }
}
