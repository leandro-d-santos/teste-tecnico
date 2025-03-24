namespace Application.Orders.Requests
{
    public sealed class UpdateOrderRequest
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string? Status { get; set; }
        public IList<OrderItemRequest> Items { get; set; }
    }
}