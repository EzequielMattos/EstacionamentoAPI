namespace EstacionamentoAPI.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string LicensePlate { get; set; }
        public List<Customer> Customers { get; set; } = new();
    }
}
