using System.Data;
using System.Text;

namespace Data.Migrations
{
    internal class CriaTabelaTokens : IMigration
    {
        public string Id => "20250324184000_CriaTabelaTokens";

        public string Description => "Cria tabela de tokens";

        public void Execute(IDbConnection connection)
        {
            string query = new StringBuilder()
                .AppendLine("CREATE TABLE IF NOT EXISTS TTTokens (")
                .AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendLine(",token TEXT NOT NULL")
                .AppendLine(",descricao TEXT NOT NULL")
                .AppendLine(",expiracao TEXT NOT NULL")
                .AppendLine(",revogado INTEGER NOT NULL")
                .AppendLine(");")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }
    }
}