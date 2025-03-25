namespace Domain.Tokens.Models
{
    public class TokenSettings
    {
        public string Description { get; set; }
        public DateTime Expiration { get; set; }
        public string? Token {  get; set; }
    }
}