using Domain.Clients.Models;

namespace Application.Clients.Requests
{
    public sealed class CreateClientRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressRequest Address { get; set; }

        public Client ToModel()
        {
            return new Client()
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
                Address = Address.ToModel(),
            };
        }
    }
}