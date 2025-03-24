namespace Domain.Orders.Models
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}