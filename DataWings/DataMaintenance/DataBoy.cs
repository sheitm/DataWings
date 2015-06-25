using DataWings.Common;

namespace DataWings.DataMaintenance
{
    /// <summary>
    /// Static gateway into the Test data functionality
    /// </summary>
    public static class DataBoy
    {
        /// <summary>
        /// Returns an assertion with the name of the connection specificaton
        /// set == connectionName.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static DataSession ForConnection(string connectionName)
        {
            return new DataSession(connectionName);
        }

        /// <summary>
        /// Returns a data accumulator session. This session has been initialized
        /// with an inner IDataAccumulatorBatch (i.e. a table) with the given
        /// table name
        /// </summary>
        /// <param name="tableName">Name of the table the the accumulative session is initialized with</param> 
        /// <returns>A data accumulator session</returns>
        public static IDataAccumulatorBatch ForTable(string tableName)
        {
            return new DataSession().ForTable(tableName);
        }

        /// <summary>
        /// Executes the non query (== a query without return resuts) directly
        /// against the database at once.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        public static void ExecuteNonQuery(string sqlStatement)
        {
            ConnectionExecutorFinder.GetSqlExecutor(null).ExecuteNonQuery(sqlStatement);
        }

        /// <summary>
        /// Executes the non query (== a query without return resuts) directly
        /// against the database at once.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        public static void ExecuteNonQuery(string sqlStatement, string connectionKey)
        {
            ConnectionExecutorFinder.GetSqlExecutor(connectionKey).ExecuteNonQuery(sqlStatement);
        }

        /// <summary>
        /// Returns a value query ready for further build up and then execution
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IValueQuery QueryForTable(string tableName)
        {
            return new ValueQuery(tableName, ConnectionExecutorFinder.GetSqlExecutor(null));
        }
    }
}