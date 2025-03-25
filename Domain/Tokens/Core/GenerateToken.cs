namespace Domain.Tokens.Core
{
    public static class GenerateToken
    {

        public static string Generate()
        {
            return Guid.NewGuid().ToString("N");
        }

    }
}