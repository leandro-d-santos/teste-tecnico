using Domain.Orders.Models;

namespace Application.Orders.Requests
{
    public class OrderItemRequest
    {
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }

        public OrderItem ToModel()
        {
            return new OrderItem()
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = Price
            };
        }
    }
}