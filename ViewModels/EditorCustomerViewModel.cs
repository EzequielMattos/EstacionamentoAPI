using System.ComponentModel.DataAnnotations;

namespace EstacionamentoAPI.ViewModels
{
    public class EditorCustomerViewModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O campo nome deve ter no máximo 100 caracteres!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O campo e-mail deve conter no máximo 100 caracteres!")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }

        [MaxLength(100, ErrorMessage = "O campo CNH deve ter no máximo 25 caracteres!")]
        public string CNH { get; set; }

        [Required(ErrorMessage = "O campo CPF é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O campo CPF deve ter no máximo 25 caracteres!")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo celular é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O campo celular deve ter no máximo 25 caracteres!")]
        public string Phone { get; set; }
    }
}
