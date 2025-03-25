using Data.Connection;
using Domain.Clients.Models;
using Domain.Clients.Repositories;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Clients.Repositories
{
    public sealed class ClientRepository : IClientRepository
    {
        private readonly DbConnection _connection;

        public ClientRepository(
            DbConnection connection
            ) => _connection = connection;

        public void Add(Client client)
        {
            client.Status = "Ativo";
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            string query = new StringBuilder()
                .AppendLine("INSERT INTO TTClientes (")
                .AppendLine("nome")
                .AppendLine(",email")
                .AppendLine(",telefone")
                .AppendLine(",rua")
                .AppendLine(",numero")
                .AppendLine(",cidade")
                .AppendLine(",estado")
                .AppendLine(",cep")
                .AppendLine(",status")
                .AppendLine(")")
                .AppendLine("VALUES (")
                .AppendLine("@nome")
                .AppendLine(",@email")
                .AppendLine(",@telefone")
                .AppendLine(",@rua")
                .AppendLine(",@numero")
                .AppendLine(",@cidade")
                .AppendLine(",@estado")
                .AppendLine(",@cep")
                .AppendLine(",@status")
                .AppendLine(");")
                .AppendLine("SELECT last_insert_rowid();")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@nome", client.Name));
            command.Parameters.Add(new SqliteParameter("@email", client.Email));
            command.Parameters.Add(new SqliteParameter("@telefone", client.Phone));
            command.Parameters.Add(new SqliteParameter("@rua", client.Address.Street));
            command.Parameters.Add(new SqliteParameter("@numero", client.Address.Number));
            command.Parameters.Add(new SqliteParameter("@cidade", client.Address.City));
            command.Parameters.Add(new SqliteParameter("@estado", client.Address.State));
            command.Parameters.Add(new SqliteParameter("@cep", client.Address.PostalCode));
            command.Parameters.Add(new SqliteParameter("@status", "Ativo"));
            try
            {
                client.Id = Convert.ToInt32(command.ExecuteScalar());
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

        public void Delete(int id)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            string query = new StringBuilder()
                .AppendLine("DELETE FROM TTClientes")
                .AppendLine("WHERE id=@id")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@id", id));
            try
            {
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

        public Client? FindById(int id)
        {
            return Find(new ClientSearch() { Id = id }).FirstOrDefault();
        }

        public IList<Client> Find(ClientSearch filter)
        {
            var clients = new List<Client>();
            using (var connection = _connection.CreateConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                StringBuilder query = new(GetQuery());
                query.AppendLine(GetQueryFilter(filter, command));
                query.AppendLine(GetQueryLimits(filter, command));
                command.CommandText = query.ToString();
                using IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Client client = new()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        Phone = reader.GetString(3),
                        Address = new Address()
                        {
                            Street = reader.GetString(4),
                            Number = reader.GetString(5),
                            City = reader.GetString(6),
                            State = reader.GetString(7),
                            PostalCode = reader.GetString(8),
                        },
                        Status = reader.GetString(9),
                    };
                    clients.Add(client);
                }
            }
            return clients;
        }

        public void Update(Client client)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            string query = new StringBuilder()
                .AppendLine("UPDATE TTClientes")
                .AppendLine("SET nome=@nome")
                .AppendLine(",email=@email")
                .AppendLine(",telefone=@telefone")
                .AppendLine(",rua=@rua")
                .AppendLine(",numero=@numero")
                .AppendLine(",cidade=@cidade")
                .AppendLine(",estado=@estado")
                .AppendLine(",cep=@cep")
                .AppendLine(",status=@status")
                .AppendLine("WHERE id=@id")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@nome", client.Name));
            command.Parameters.Add(new SqliteParameter("@email", client.Email));
            command.Parameters.Add(new SqliteParameter("@telefone", client.Phone));
            command.Parameters.Add(new SqliteParameter("@rua", client.Address.Street));
            command.Parameters.Add(new SqliteParameter("@numero", client.Address.Number));
            command.Parameters.Add(new SqliteParameter("@cidade", client.Address.City));
            command.Parameters.Add(new SqliteParameter("@estado", client.Address.State));
            command.Parameters.Add(new SqliteParameter("@cep", client.Address.PostalCode));
            command.Parameters.Add(new SqliteParameter("@status", client.Status));
            command.Parameters.Add(new SqliteParameter("@id", client.Id));
            try
            {
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

        private static string GetQuery()
        {
            return new StringBuilder()
                    .AppendLine("SELECT id")
                    .AppendLine(",nome")
                    .AppendLine(",email")
                    .AppendLine(",telefone")
                    .AppendLine(",rua")
                    .AppendLine(",numero")
                    .AppendLine(",cidade")
                    .AppendLine(",estado")
                    .AppendLine(",cep")
                    .AppendLine(",status")
                    .AppendLine("FROM TTClientes")
                    .ToString();
        }

        private static string GetQueryFilter(ClientSearch filter, IDbCommand command)
        {
            StringBuilder builder = new();
            builder.AppendLine("WHERE 1=1");
            if (filter.Id.HasValue)
            {
                builder.AppendFormat("AND id=@id").AppendLine();
                command.Parameters.Add(new SqliteParameter("@id", filter.Id));
            }
            if (!String.IsNullOrEmpty(filter.Name))
            {
                builder.AppendFormat("AND nome LIKE @name").AppendLine();
                command.Parameters.Add(new SqliteParameter("@name", String.Format("%{0}%", filter.Name)));
            }
            if (!String.IsNullOrEmpty(filter.Email))
            {
                builder.AppendFormat("AND email LIKE @email").AppendLine();
                command.Parameters.Add(new SqliteParameter("@email", String.Format("%{0}%", filter.Email)));
            }
            if (!String.IsNullOrEmpty(filter.Status))
            {
                builder.AppendFormat("AND status=@status").AppendLine();
                command.Parameters.Add(new SqliteParameter("@status", filter.Status));
            }
            if (!String.IsNullOrEmpty(filter.Phone))
            {
                builder.AppendFormat("AND telefone LIKE @phone").AppendLine();
                command.Parameters.Add(new SqliteParameter("@phone", String.Format("%{0}%", filter.Phone)));
            }
            return builder.ToString();
        }

        private static string GetQueryLimits(ClientSearch filter, IDbCommand command)
        {
            StringBuilder builder = new("LIMIT @limit OFFSET @offset");
            command.Parameters.Add(new SqliteParameter("@limit", filter.Limit));
            command.Parameters.Add(new SqliteParameter("@offset", filter.Offset));
            return builder.ToString();
        }
    }
}