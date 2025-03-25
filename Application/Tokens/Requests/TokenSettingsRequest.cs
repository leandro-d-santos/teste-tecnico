using Domain.Tokens.Models;

namespace Application.Tokens.Requests
{
    public class TokenSettingsRequest
    {
        public string Description { get; set; }
        public DateTime Expiration { get; set; }

        public TokenSettings ToModel()
        {
            return new TokenSettings
            {
                Description = Description,
                Expiration = Expiration
            };
        }
    }
}