namespace Domain.Orders.Models
{
    public class OrderSearch
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public int? ProductId { get; set; }
        public string? Status { get; set; }
    }
}