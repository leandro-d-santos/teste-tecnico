using Application.Orders.Requests;
using Application.Orders.Responses;

namespace Application.Orders.Services
{
    public interface IOrderService
    {
        ShortOrderResponse CreateAsync(CreateOrderRequest request);

        IList<OrderResponse> FindAsync(OrderSearchRequest request);

        OrderResponse? FindByIdAsync(int id);

        ShortOrderResponse UpdateAsync(UpdateOrderRequest request);

        void UpdateStatus(int id, string status);

        void DeleteAsync(int id);
    }
}