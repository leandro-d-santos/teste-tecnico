using Application.Clients.Requests;
using Application.Clients.Responses;

namespace Application.Clients.Services
{
    public interface IClientService
    {
        ClientResponse CreateAsync(CreateClientRequest request);

        IList<ClientSearchResponse> FindAsync(ClientSearchRequest request);

        ClientByIdResponse? FindByIdAsync(int id);

        ClientResponse UpdateAsync(UpdateClientRequest request);

        void DeleteAsync(int id);
    }
}