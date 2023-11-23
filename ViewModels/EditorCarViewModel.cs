using System.ComponentModel.DataAnnotations;

namespace EstacionamentoAPI.ViewModels
{
    public class EditorCarViewModel
    {
        [Required(ErrorMessage = "O campo modelo é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O campo modelo deve ter no máximo 100 caracteres!")]
        public string Model { get; set; }

        [Required(ErrorMessage = "O campo marca é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O campo marca deve ter no máximo 100 caracteres!")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "O campo placa é obrigatório!")]
        [MaxLength(10, ErrorMessage = "O campo placa deve ter no máximo 10 caracteres!")]
        public string LicensePlate { get; set; }

        [Required(ErrorMessage = "O campo cliente é obrigatório!")]
        public int IdCustomer { get; set; }
    }
}
