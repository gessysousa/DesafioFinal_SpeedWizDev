using System.ComponentModel.DataAnnotations;

namespace biblioteca.Auth.Interfaces
{
    public class JwtUsuarioCredenciais
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
