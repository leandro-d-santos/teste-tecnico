using Application.Tokens.Requests;
using Application.Tokens.Responses;
using Domain.Tokens.Models;
using Domain.Tokens.Repositories;

namespace Application.Tokens.Services
{
    public class TokenSettingsService : ITokenSettingsService
    {
        private readonly ITokenSettingsRepository _tokenSettingsRepository;
        public TokenSettingsService(ITokenSettingsRepository tokenSettingsRepository) => _tokenSettingsRepository = tokenSettingsRepository;

        public IList<TokenSearchResponse> FindAll()
        {
            IList<TokenSearch> tokens = _tokenSettingsRepository.FindAll();
            return MapDomainToViewModel(tokens);
        }

        public TokenResponse Create(TokenSettingsRequest request)
        {
            TokenSettings settings = request.ToModel();
            _tokenSettingsRepository.Add(settings);
            return new TokenResponse() { Token = settings.Token! };
        }

        public void Revoke(int id)
        {
            string token = _tokenSettingsRepository.FindById(id);
            if (String.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token não encontrado.");
            }
            _tokenSettingsRepository.Revoke(token);
        }

        private static IList<TokenSearchResponse> MapDomainToViewModel(IList<TokenSearch> tokens)
        {
            return tokens.Select(token =>
                 new TokenSearchResponse()
                 {
                     Id = token.Id,
                     Description = token.Description,
                     Expiration = token.Expiration,
                     Revoked = token.Revoked,
                 }
            ).ToList();
        }

    }
}