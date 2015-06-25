using DataWings.Common;

namespace DataWings.SQLite
{
    /// <summary>
    /// Creates instances of SQLiteProvider
    /// </summary>
    public class SQLiteProviderFactory : ISqlProviderFactory
    {

        /// <summary>
        /// Creates a new provider. This provider is initialized with
        /// the given connection string
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public ISqlProvider CreateProvider(string connectionString)
        {
            return new SQLiteProvider(connectionString);
        }
    }
}
