using Domain.Clients.Models;

namespace Application.Clients.Requests
{
    public class ClientSearchRequest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public ClientSearch ToModel(ClientSearchRequest request)
        {
            return new ClientSearch()
            {
                Id = request.Id,
                Name = request.Name,
                Status = request.Status,
                Email = request.Email,
                Phone = request.Phone
            };
        }

    }
}