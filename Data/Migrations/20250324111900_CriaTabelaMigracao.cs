using System.Data;
using System.Text;

namespace Data.Migrations
{
    internal class CriaTabelaMigracao : IMigration
    {
        public string Id => "20250324111900_CriaTabelaMigracao";

        public string Description => "Cria tabela de migração";

        public void Execute(IDbConnection connection)
        {
            string query = new StringBuilder()
                .AppendLine("CREATE TABLE IF NOT EXISTS TTMigrations (")
                .AppendLine("id TEXT PRIMARY KEY")
                .AppendLine(",descricao TEXT NOT NULL")
                .AppendLine(");")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }
    }
}