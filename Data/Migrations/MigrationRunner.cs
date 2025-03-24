using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace Data.Migrations
{
    public sealed class MigrationRunner
    {
        private readonly Connection.DbConnection _connection;

        public MigrationRunner(
            Connection.DbConnection connection
            )
        {
            _connection = connection;
        }

        public void Run()
        {
            IList<IMigration> commands = [
                new CriaTabelaClientes(),
                new CriaTabelaPedidos(),
                new CriaTabelaTokens(),
                ];

            using (var connection = _connection.CreateConnection())
            {
                connection.Open();
                InitialMigration(connection);
                if (commands.Count <= 0)
                {
                    return;
                }
                foreach (var command in commands)
                {
                    RunMigration(command, connection);
                }
            }
        }

        private static void InitialMigration(IDbConnection connection)
        {
            IMigration migration = new CriaTabelaMigracao();
            try
            {
                if (Exists(migration.Id, connection))
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("no such table: TTMigrations"))
                {
                    throw new InvalidOperationException(ex.Message);
                }
                migration.Execute(connection);
                InsertMigrationHistory(migration, connection);
            }
        }

        private static void RunMigration(IMigration migration, IDbConnection connection)
        {
            IDbTransaction t = connection.BeginTransaction();
            try
            {
                IDbConnection tConnection = t.Connection!;
                if (Exists(migration.Id, tConnection))
                {
                    t.Dispose();
                    return;
                }
                migration.Execute(tConnection);
                InsertMigrationHistory(migration, tConnection);
                t.Commit();
            }
            catch (Exception ex)
            {
                t.Rollback();
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                t.Dispose();
            }
        }

        private static bool Exists(string migrationId, IDbConnection connection)
        {
            string query = new StringBuilder()
                .AppendLine("SELECT COUNT(0)")
                .AppendLine("FROM TTMigrations")
                .AppendLine("WHERE id=@id")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@id", migrationId));
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        private static void InsertMigrationHistory(IMigration migration, IDbConnection connection)
        {
            string query = new StringBuilder()
                .AppendLine("INSERT INTO TTMigrations (")
                .AppendLine("id")
                .AppendLine(",descricao")
                .AppendLine(")")
                .AppendLine("VALUES (")
                .AppendLine("@id")
                .AppendLine(",@description")
                .AppendLine(");")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@id", migration.Id));
            command.Parameters.Add(new SqliteParameter("@description", migration.Description));
            command.ExecuteNonQuery();
        }
    }
}