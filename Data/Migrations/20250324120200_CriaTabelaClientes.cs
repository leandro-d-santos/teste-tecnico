using System.Data;
using System.Text;

namespace Data.Migrations
{
    internal class CriaTabelaClientes : IMigration
    {
        public string Id => "20250324120200_CriaTabelaClientes";

        public string Description => "Cria tabela de clientes";

        public void Execute(IDbConnection connection)
        {
            string query = new StringBuilder()
                .AppendLine("CREATE TABLE IF NOT EXISTS TTClientes (")
                .AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendLine(",nome TEXT NOT NULL")
                .AppendLine(",email TEXT NOT NULL")
                .AppendLine(",telefone TEXT NOT NULL")
                .AppendLine(",rua TEXT NOT NULL")
                .AppendLine(",numero TEXT NOT NULL")
                .AppendLine(",cidade TEXT NOT NULL")
                .AppendLine(",estado TEXT NOT NULL")
                .AppendLine(",cep TEXT NOT NULL")
                .AppendLine(",status TEXT NOT NULL")
                .AppendLine(")")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }
    }
}