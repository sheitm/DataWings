using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace DataWings.Common.Oracle
{
    /// <summary>
    /// Implements the ISqlProvider contract for Oracle databases
    /// by using the ADO.NET providers available in the System.Data.OracleClient
    /// namespace
    /// </summary>
    public class OracleProvider : SqlProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OracleProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public OracleProvider(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Gets the connection. Must be overridden by subclasses.
        /// </summary>
        /// <param name="connString">The conn string.</param>
        /// <returns></returns>
        protected override DbConnection GetConnectionCore(string connString)
        {
            return new OracleConnection(connString);
        }
    }
}