using Domain.Orders.Models;

namespace Application.Orders.Requests
{
    public class OrderSearchRequest
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public int? ProductId { get; set; }
        public string? Status { get; set; }

        public OrderSearch ToModel(OrderSearchRequest request)
        {
            return new OrderSearch()
            {
                Id = request.Id,
                ClientId = request.ClientId,
                ProductId = request.ProductId,
                Status = request.Status
            };
        }
    }
}