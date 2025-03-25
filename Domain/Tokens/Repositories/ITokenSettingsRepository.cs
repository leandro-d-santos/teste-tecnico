using Domain.Tokens.Models;

namespace Domain.Tokens.Repositories
{
    public interface ITokenSettingsRepository
    {

        IList<TokenSearch> FindAll();

        string FindById(int id);

        void Add(TokenSettings tokenSettings);

        void Revoke(string token);
    }
}