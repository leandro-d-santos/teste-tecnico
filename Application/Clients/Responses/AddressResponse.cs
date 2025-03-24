using Domain.Clients.Models;

namespace Application.Clients.Responses
{
    public sealed class AddressResponse
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Number { get; set; }

        public static AddressResponse FromModel(Address address)
        {
            return new AddressResponse()
            {
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Number = address.Number,
                Street = address.Street,
            };
        }
    }
}