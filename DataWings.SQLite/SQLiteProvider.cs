using System.Data.Common;
using DataWings.Common;

namespace DataWings.SQLite
{
    /// <summary>
    /// Implements the ISqlProvider contract for SQLite databases
    /// by using the ADO.NET providers available in the System.Data.SQLite
    /// namespace
    /// </summary>
    public class SQLiteProvider : SqlProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SQLiteProvider(string connectionString) : base(connectionString)
        {
        }

        public override SqlVendor Vendor
        {
            get { return SqlVendor.Provisioned; }
        }

        /// <summary>
        /// Gets the connection, in this case a System.Data.SQLite
        /// connection
        /// </summary>
        /// <param name="connString">The conn string.</param>
        /// <returns></returns>
        protected override DbConnection GetConnectionCore(string connString)
        {
            var fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            var connection = fact.CreateConnection();
            connection.ConnectionString = connString;
            return connection;
        }
    }
}
