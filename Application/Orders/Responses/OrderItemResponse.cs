using Domain.Orders.Models;

namespace Application.Orders.Responses
{
    public class OrderItemResponse
    {
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }

        public static OrderItemResponse FromModel(OrderItem item)
        {
            return new OrderItemResponse()
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            };
        }
    }
}