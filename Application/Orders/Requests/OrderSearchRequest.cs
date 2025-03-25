using Domain.Orders.Models;

namespace Application.Orders.Requests
{
    public class OrderSearchRequest
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public int? ProductId { get; set; }
        public string? Status { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }

        public OrderSearch ToModel(OrderSearchRequest request)
        {
            return new OrderSearch()
            {
                Id = request.Id,
                ClientId = request.ClientId,
                ProductId = request.ProductId,
                Status = request.Status,
                Offset = request.Offset.GetValueOrDefault(0),
                Limit = request.Limit.GetValueOrDefault(0)
            };
        }
    }
}