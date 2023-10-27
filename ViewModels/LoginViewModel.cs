using System.ComponentModel.DataAnnotations;

namespace EstacionamentoAPI.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo e-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "Digite um e-mail válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "A senha deve conter entre 6 e 16 caracteres!")]
        public string Senha { get; set; }
    }
}
