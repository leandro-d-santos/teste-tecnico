using Data.Connection;
using Domain.Clients.Models;
using Domain.Orders.Models;
using Domain.Orders.Repositories;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace Data.Orders.Repositories
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly DbConnection _connection;

        public OrderRepository(
            DbConnection connection
            ) => _connection = connection;

        public void Add(Order order)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                InsertOrder(order, transaction.Connection!);
                InsertOrderItems(order, transaction.Connection!);
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

        private static void InsertOrderItems(Order order, IDbConnection connection)
        {
            foreach (OrderItem item in order.Items)
            {
                string query = new StringBuilder()
                .AppendLine("INSERT INTO TTItensPedido (")
                .AppendLine("pedidoId")
                .AppendLine(",produtoId")
                .AppendLine(",quantidade")
                .AppendLine(",precoUnitario")
                .AppendLine(")")
                .AppendLine("VALUES (")
                .AppendLine("@pedidoId")
                .AppendLine(",@produtoId")
                .AppendLine(",@quantidade")
                .AppendLine(",@precoUnitario")
                .AppendLine(");")
                .ToString();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(new SqliteParameter("@pedidoId", order.Id));
                command.Parameters.Add(new SqliteParameter("@produtoId", item.ProductId));
                command.Parameters.Add(new SqliteParameter("@quantidade", item.Quantity));
                command.Parameters.Add(new SqliteParameter("@precoUnitario", item.Price));
                command.ExecuteNonQuery();
            }
        }

        private static void InsertOrder(Order order, IDbConnection connection)
        {
            order.Status = "Ativo";
            string query = new StringBuilder()
                .AppendLine("INSERT INTO TTPedidos (")
                .AppendLine("clienteId")
                .AppendLine(",status")
                .AppendLine(")")
                .AppendLine("VALUES (")
                .AppendLine("@clienteId")
                .AppendLine(",@status")
                .AppendLine(");")
                .AppendLine("SELECT last_insert_rowid();")
                .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@clienteId", order.ClientId));
            command.Parameters.Add(new SqliteParameter("@status", "Em Processamento"));
            order.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                DeleteOrderItems(id, transaction.Connection!);
                DeleteOrder(id, transaction.Connection!);
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

        private static void DeleteOrderItems(int id, IDbConnection connection)
        {
            string query = new StringBuilder()
                            .AppendLine("DELETE FROM TTItensPedido")
                            .AppendLine("WHERE pedidoId=@id")
                            .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.ExecuteNonQuery();
        }

        private static void DeleteOrder(int id, IDbConnection connection)
        {
            string query = new StringBuilder()
                            .AppendLine("DELETE FROM TTPedidos")
                            .AppendLine("WHERE id=@id")
                            .ToString();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.ExecuteNonQuery();
        }

        public Order? FindById(int id)
        {
            return Find(new OrderSearch() { Id = id }).FirstOrDefault();
        }

        public IList<Order> Find(OrderSearch filter)
        {
            var orders = new Dictionary<int, Order>();
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
                    int orderId = reader.GetInt32(0);
                    Order? order;
                    if (!orders.TryGetValue(orderId, out order))
                    {
                        order = new Order()
                        {
                            Id = orderId,
                            ClientId = reader.GetInt32(1),
                            Status = reader.GetString(2),
                            Items = []
                        };
                        orders.Add(orderId, order);
                    }
                    OrderItem item = new()
                    {
                        ProductId = reader.GetInt32(3),
                        Quantity = reader.GetDouble(4),
                        Price = reader.GetDouble(5),
                    };
                    order.Items.Add(item);
                }
            }
            return orders.Values.ToList();
        }

        public void Update(Order order)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                DeleteOrderItems(order.Id!.Value, transaction.Connection!);
                InsertOrderItems(order, transaction.Connection!);
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
                    .AppendLine("SELECT TTP.id")
                    .AppendLine(",TTP.clienteId")
                    .AppendLine(",TTP.status")
                    .AppendLine(",TTIP.produtoId")
                    .AppendLine(",TTIP.quantidade")
                    .AppendLine(",TTIP.precoUnitario")
                    .AppendLine("FROM TTPedidos TTP")
                    .AppendLine("LEFT JOIN TTItensPedido TTIP")
                    .AppendLine("ON TTIP.pedidoId=TTP.id")
                    .ToString();
        }

        private static string GetQueryFilter(OrderSearch filter, IDbCommand command)
        {
            StringBuilder builder = new();
            builder.AppendLine("WHERE 1=1");
            if (filter.Id.HasValue)
            {
                builder.AppendFormat("AND TTP.id=@id").AppendLine();
                command.Parameters.Add(new SqliteParameter("@id", filter.Id));
            }
            if (filter.ClientId.HasValue)
            {
                builder.AppendFormat("AND TTP.clienteId=@clientId").AppendLine();
                command.Parameters.Add(new SqliteParameter("@clientId", filter.ClientId));
            }
            if (!String.IsNullOrEmpty(filter.Status))
            {
                builder.AppendFormat("AND status=@status").AppendLine();
                command.Parameters.Add(new SqliteParameter("@status", filter.Status));
            }
            if (filter.ProductId.HasValue)
            {
                builder.AppendFormat("AND TTIP.produtoId=@productId").AppendLine();
                command.Parameters.Add(new SqliteParameter("@productId", filter.ProductId));
            }
            return builder.ToString();
        }

        private static string GetQueryLimits(OrderSearch filter, IDbCommand command)
        {
            StringBuilder builder = new("LIMIT @limit OFFSET @offset");
            command.Parameters.Add(new SqliteParameter("@limit", filter.Limit));
            command.Parameters.Add(new SqliteParameter("@offset", filter.Offset));
            return builder.ToString();
        }

        public void UpdateStatus(int id, string status)
        {
            using var connection = _connection.CreateConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                string query = new StringBuilder()
                           .AppendLine("UPDATE TTPedidos")
                           .AppendLine("SET status=@status")
                           .AppendLine("WHERE id=@id")
                           .ToString();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(new SqliteParameter("@status", status));
                command.Parameters.Add(new SqliteParameter("@id", id));
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