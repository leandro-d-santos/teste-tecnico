namespace Domain.Orders.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public int ClientId { get; set; }
        public string? Status { get; set; }
        public IList<OrderItem> Items { get; set; }
    }
}