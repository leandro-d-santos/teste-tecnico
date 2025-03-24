using Domain.Orders.Models;

namespace Application.Orders.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; }
        public IList<OrderItemResponse> Items { get; set; }
        public double TotalValue { get; set; }

        public static OrderResponse FromModel(Order order)
        {
            return new OrderResponse()
            {
                Id = order.Id!.Value,
                ClientId = order.ClientId,
                Status = order.Status,
                Items = order.Items.Select(item => OrderItemResponse.FromModel(item)).ToList(),
                TotalValue = order.Items.Sum(item => item.Quantity * item.Price)
            };
        }
    }
}