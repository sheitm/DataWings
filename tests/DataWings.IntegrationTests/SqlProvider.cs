using System.Data.Common;
using DataWings.Common;
using DataWings.SQLite;

namespace DataWings.IntegrationTests
{
    class SqlProvider : SQLiteProvider, ISqlProviderFactory
    {
        private static SqlProvider current;

        public static SqlProvider GetInstance(string connString)
        {
            if (current == null)
                current = new SqlProvider(connString);
            return current;
        }

        public SqlProvider(string connectionString) : base(connectionString)
        {
        }

        protected override void SqlExecuted(string sql)
        {
            Database.ReceiveExecutedSql(sql);
        }

        public ISqlProvider CreateProvider(string connectionString)
        {
            return this;
        }
    }
}
