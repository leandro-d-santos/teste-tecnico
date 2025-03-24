using Domain.Orders.Models;

namespace Domain.Orders.Repositories
{
    public interface IOrderRepository
    {
        void Add(Order order);

        Order? FindById(int id);

        IList<Order> Find(OrderSearch filter);

        void Update(Order order);

        void UpdateStatus(int id, string status);

        void Delete(int id);
    }
}