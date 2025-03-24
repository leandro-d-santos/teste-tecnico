namespace Application.Clients.Requests
{
    public sealed class UpdateClientRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressRequest Address { get; set; }
    }
}