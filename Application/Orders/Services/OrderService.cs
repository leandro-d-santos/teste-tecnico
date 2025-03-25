using Application.Orders.Requests;
using Application.Orders.Responses;
using Domain.Orders.Models;
using Domain.Orders.Repositories;

namespace Application.Orders.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderService(
            IOrderRepository orderRepository
            )
        {
            this.orderRepository = orderRepository;
        }

        public ShortOrderResponse CreateAsync(CreateOrderRequest request)
        {
            Order order = request.ToModel();
            orderRepository.Add(order);
            return new ShortOrderResponse()
            {
                Id = order.Id!.Value,
                ClientId = order.ClientId,
                Status = order.Status!
            };
        }

        public void DeleteAsync(int id)
        {
            Order? order = orderRepository.FindById(id);
            ArgumentNullException.ThrowIfNull(order, "Pedido");
            orderRepository.Delete(id);
        }

        public OrderResponse? FindByIdAsync(int id)
        {
            Order? order = orderRepository.FindById(id);
            if (order is null)
            {
                return null;
            }
            return OrderResponse.FromModel(order);
        }

        public IList<OrderResponse> FindAsync(OrderSearchRequest request)
        {
            OrderSearch filter = request.ToModel(request);
            if (request.Offset is null)
            {
                filter.Offset = 0;
            }
            if (request.Limit is null)
            {
                filter.Limit = 50;
            }
            IList<Order> orders = orderRepository.Find(filter);
            return orders.Select(order => OrderResponse.FromModel(order)).ToList();
        }

        public ShortOrderResponse UpdateAsync(UpdateOrderRequest request)
        {
            Order? order = orderRepository.FindById(request.Id);
            ArgumentNullException.ThrowIfNull(order, "Pedido");
            order.Items = request.Items.Select(item => item.ToModel()).ToList();
            orderRepository.Update(order);
            return new ShortOrderResponse()
            {
                Id = order.Id!.Value,
                ClientId = order.ClientId,
                Status = order.Status!
            };
        }

        public void UpdateStatus(int id, string status)
        {
            Order? order = orderRepository.FindById(id);
            ArgumentNullException.ThrowIfNull(order, "Pedido");
            orderRepository.UpdateStatus(id, status);
        }
    }
}