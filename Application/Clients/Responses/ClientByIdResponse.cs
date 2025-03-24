using Domain.Clients.Models;

namespace Application.Clients.Responses
{
    public sealed class ClientByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public AddressResponse Address { get; set; }

        public static ClientByIdResponse FromModel(Client client)
        {
            return new ClientByIdResponse()
            {
                Id = client.Id!.Value,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                Status = client.Status,
                Address = AddressResponse.FromModel(client.Address)
            };
        }
    }
}