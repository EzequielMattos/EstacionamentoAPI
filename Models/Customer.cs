using Microsoft.AspNetCore.Identity;

namespace EstacionamentoAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CNH { get; set; }
        public string CPF { get; set; }
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Car> Cars { get; set; } = new();
    }
}
