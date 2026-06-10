using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    // Dados enviados na tela de login.
    public class LoginDto
    {
        [Required]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }

    // Resposta do login com o token JWT.
    public class LoginRespostaDto
    {
        public string Token { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
    }
}
