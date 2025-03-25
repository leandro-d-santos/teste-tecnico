namespace Domain.Tokens.Repositories
{
    public interface ITokenValidationRepository
    {
        bool IsValid(string token);
    }
}