using Domain.Clients.Models;

namespace Application.Clients.Requests
{
    public sealed class AddressRequest
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Number { get; set; }

        public Address ToModel()
        {
            return new Address
            {
                Street = Street,
                City = City,
                State = State,
                PostalCode = PostalCode,
                Number = Number
            };
        }
    }
}