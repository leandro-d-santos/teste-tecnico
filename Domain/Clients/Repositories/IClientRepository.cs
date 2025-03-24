using Domain.Clients.Models;

namespace Domain.Clients.Repositories
{
    public interface IClientRepository
    {
        void Add(Client client);

        Client? FindById(int id);

        IList<Client> Find(ClientSearch filter);

        void Update(Client client);

        void Delete(int id);
    }
}