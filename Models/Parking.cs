namespace EstacionamentoAPI.Models
{
    public class Parking
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public Customer Customer { get; set; }
        public bool Available { get; set; }
    }
}
