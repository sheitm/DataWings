using System.Data.Common;
using System.Data.SqlClient;

namespace DataWings.Common.Sql
{
    /// <summary>
    /// Implements the ISqlProvider contract for Microsoft SQL Server databases
    /// by using the ADO.NET providers available in the System.Data.SqlClient
    /// namespace
    /// </summary>
    public class SqlServerProvider : SqlProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlServerProvider(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Gets the connection. Must be overridden by subclasses.
        /// </summary>
        /// <param name="connString">The conn string.</param>
        /// <returns></returns>
        protected override DbConnection GetConnectionCore(string connString)
        {
            return new SqlConnection(connString);
        }
    }
}