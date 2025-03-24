using System.Data;
using System.Text;

namespace Data.Migrations
{
    internal class CriaTabelaPedidos : IMigration
    {
        public string Id => "20250324120300_CriaTabelaPedidos";

        public string Description => "Cria tabela de pedidos";

        public void Execute(IDbConnection connection)
        {
            string query = new StringBuilder()
                .AppendLine("CREATE TABLE IF NOT EXISTS TTPedidos (")
                .AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendLine(",clienteId INTEGER NOT NULL")
                .AppendLine(",status TEXT NOT NULL")
                .AppendLine(",FOREIGN KEY (clienteId) REFERENCES TTClientes(id)")
                .AppendLine(");")
                .AppendLine("")
                .AppendLine("CREATE TABLE IF NOT EXISTS TTItensPedido (")
                .AppendLine("id INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendLine(",pedidoId INTEGER NOT NULL")
                .AppendLine(",produtoId INTEGER NOT NULL")
                .AppendLine(",quantidade INTEGER NOT NULL")
                .AppendLine(",precoUnitario REAL NOT NULL")
                .AppendLine(",FOREIGN KEY (pedidoId) REFERENCES TTPedidos(id)")
                .AppendLine(");")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }
    }
}