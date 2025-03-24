using System.Data;

namespace Data.Migrations
{
    internal interface IMigration
    {
        public string Id { get; }

        public string Description { get; }

        public void Execute(IDbConnection connection);
    }
}