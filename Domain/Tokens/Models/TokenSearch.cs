namespace Domain.Tokens.Models
{
    public class TokenSearch
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Expiration { get; set; }
        public bool Revoked { get; set; }
    }
}