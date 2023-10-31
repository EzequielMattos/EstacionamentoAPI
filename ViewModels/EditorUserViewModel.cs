using System.ComponentModel.DataAnnotations;

namespace EstacionamentoAPI.ViewModels
{
    public class EditorUserViewModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O campo nome deve conter no máximo 100 caracteres!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O campo e-mail deve conter no máximo 100 caracteres!")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        [StringLength(6, MinimumLength = 10, ErrorMessage = "Este campo deve conter entre 6 e 10 caracteres!")]
        public string Password { get; set; }
    }
}
