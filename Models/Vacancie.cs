using EstacionamentoAPI.Models.Enums;

namespace EstacionamentoAPI.Models
{
    public class Vacancie
    {
        public int Id { get; set; }
        public int NumberVacancie { get; set; }
        public Car Car { get; set; }
        public Customer Customer { get; set; }
        public EAvaliable Available { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
