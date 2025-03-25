using Data.Connection;
using Domain.Tokens.Repositories;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace Data.Tokens.Repositories
{
    public sealed class TokenValidationRepository : ITokenValidationRepository
    {
        private readonly DbConnection _connection;

        public TokenValidationRepository(
            DbConnection connection
            ) => _connection = connection;

        public bool IsValid(string token)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            string query = new StringBuilder()
                .AppendLine("SELECT COUNT(0)")
                .AppendLine("FROM TTTokens")
                .AppendLine("WHERE token=@token")
                .AppendLine("AND revogado<>1")
                .AppendLine("AND expiracao>@expiration")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@token", token));
            command.Parameters.Add(new SqliteParameter("@expiration", DateTime.UtcNow.ToString()));
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }
}