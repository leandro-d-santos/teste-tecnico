using Data.Connection;
using Domain.Tokens.Core;
using Domain.Tokens.Models;
using Domain.Tokens.Repositories;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace Data.Tokens.Repositories
{
    public sealed class TokenSettingsRepository : ITokenSettingsRepository
    {
        private readonly DbConnection _connection;

        public TokenSettingsRepository(
            DbConnection connection
            ) => _connection = connection;

        public void Add(TokenSettings tokenSettings)
        {
            tokenSettings.Token = GenerateToken.Generate();
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                string query = new StringBuilder()
                    .AppendLine("INSERT INTO TTTokens (")
                    .AppendLine("descricao")
                    .AppendLine(",expiracao")
                    .AppendLine(",token")
                    .AppendLine(",revogado")
                    .AppendLine(")")
                    .AppendLine("VALUES (")
                    .AppendLine("@description")
                    .AppendLine(",@expiration")
                    .AppendLine(",@token")
                    .AppendLine(",0")
                    .AppendLine(");")
                    .ToString();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(new SqliteParameter("@description", tokenSettings.Description));
                command.Parameters.Add(new SqliteParameter("@expiration", tokenSettings.Expiration.ToString()));
                command.Parameters.Add(new SqliteParameter("@token", tokenSettings.Token));
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public IList<TokenSearch> FindAll()
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            string query = new StringBuilder()
                .AppendLine("SELECT id")
                .AppendLine(",descricao")
                .AppendLine(",expiracao")
                .AppendLine(",revogado")
                .AppendLine("FROM TTTokens")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            List<TokenSearch> tokens = new();
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tokenSettings = new TokenSearch()
                    {
                        Id = reader.GetInt32(0),
                        Description = reader.GetString(1),
                        Expiration = reader.GetDateTime(2),
                        Revoked = reader.GetBoolean(3)
                    };
                    tokens.Add(tokenSettings);
                }
            }
            return tokens;
        }

        public string FindById(int id)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            string query = new StringBuilder()
                .AppendLine("SELECT token")
                .AppendLine("FROM TTTokens")
                .AppendLine("WHERE id=@id")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@id", id));
            object? result = command.ExecuteScalar();
            return result is null ?  "" : Convert.ToString(result)!;
        }

        public void Revoke(string token)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                string query = new StringBuilder()
                    .AppendLine("UPDATE TTTokens")
                    .AppendLine("SET revogado=1")
                    .AppendLine("WHERE token=@token")
                    .ToString();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(new SqliteParameter("@token", token));
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}