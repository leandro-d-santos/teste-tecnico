using Application.Clients.Requests;
using Application.Clients.Responses;
using Domain.Clients.Models;
using Domain.Clients.Repositories;

namespace Application.Clients.Services
{
    public sealed class ClientService : IClientService
    {
        private readonly IClientRepository clientRepository;

        public ClientService(
            IClientRepository clientRepository
            )
        {
            this.clientRepository = clientRepository;
        }

        public ClientResponse CreateAsync(CreateClientRequest request)
        {
            Client client = request.ToModel();
            clientRepository.Add(client);
            return new ClientResponse()
            {
                Id = client.Id!.Value,
                Name = client.Name,
                Status = client.Status!
            };
        }

        public void DeleteAsync(int id)
        {
            Client? client = clientRepository.FindById(id);
            ArgumentNullException.ThrowIfNull(client, "Cliente");
            clientRepository.Delete(id);
        }

        public ClientByIdResponse? FindByIdAsync(int id)
        {
            Client? client = clientRepository.FindById(id);
            if (client is null)
            {
                return null;
            }
            return ClientByIdResponse.FromModel(client);
        }

        public IList<ClientSearchResponse> FindAsync(ClientSearchRequest request)
        {
            ClientSearch filter = request.ToModel(request);
            if (request.Offset is null)
            {
                filter.Offset = 0;
            }
            if (request.Limit is null)
            {
                filter.Limit = 50;
            }
            IList<Client> clients = clientRepository.Find(filter);
            return MapClientsToViewModel(clients);
        }

        public ClientResponse UpdateAsync(UpdateClientRequest request)
        {
            Client? client = clientRepository.FindById(request.Id);
            ArgumentNullException.ThrowIfNull(client, "Cliente");
            client.Name = request.Name;
            client.Phone = request.Phone;
            client.Email = request.Email;
            client.Address = request.Address.ToModel();
            clientRepository.Update(client);
            return new ClientResponse()
            {
                Id = client.Id!.Value,
                Name = client.Name,
                Status = client.Status!
            };
        }

        public IList<ClientSearchResponse> MapClientsToViewModel(IList<Client> clients)
        {
            return clients.Select(client =>
            {
                return new ClientSearchResponse()
                {
                    Id = client.Id.Value,
                    Name = client.Name,
                    Status = client.Status,
                    Email = client.Email,
                    Phone = client.Phone
                };
            }).ToList();
        }
    }
}