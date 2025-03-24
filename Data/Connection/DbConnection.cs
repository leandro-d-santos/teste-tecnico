using Data.Migrations;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Data.Connection
{
    public class DbConnection
    {
        private readonly string _connectionString;

        public DbConnection(string? connectionString)
        {
            ArgumentNullException.ThrowIfNull(connectionString);
            _connectionString = connectionString;
            RunMigrations();
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        private void RunMigrations()
        {
            MigrationRunner migrationRunner = new(this);
            migrationRunner.Run();
        }
    }
}