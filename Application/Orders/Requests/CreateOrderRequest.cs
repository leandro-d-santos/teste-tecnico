using Domain.Orders.Models;

namespace Application.Orders.Requests
{
    public sealed class CreateOrderRequest
    {
        public int ClientId { get; set; }
        public string? Status { get; set; }
        public IList<OrderItemRequest> Items { get; set; }

        public Order ToModel()
        {
            return new Order()
            {
                ClientId = ClientId,
                Status = Status,
                Items = Items.Select(item => item.ToModel()).ToList()
            };
        }
    }
}