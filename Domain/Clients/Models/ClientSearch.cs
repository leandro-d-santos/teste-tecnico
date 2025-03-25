namespace Domain.Clients.Models
{
    public class ClientSearch
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}