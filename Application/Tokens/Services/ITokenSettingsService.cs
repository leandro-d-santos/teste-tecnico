using Application.Tokens.Requests;
using Application.Tokens.Responses;

namespace Application.Tokens.Services
{
    public interface ITokenSettingsService
    {
        IList<TokenSearchResponse> FindAll();
        TokenResponse Create(TokenSettingsRequest request);
        void Revoke(int id);
    }
}