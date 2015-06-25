using DataWings.Common;

namespace DataWings.Assertions
{
    public class DbAssertion
    {
        private string connectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbAssertion"/> class.
        /// </summary>
        public DbAssertion()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbAssertion"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        public DbAssertion(string connectionName)
        {
            this.connectionName = connectionName;
        }

        /// <summary>
        /// Initializes and returns an accumulative assertion where the name of the
        /// database table which the select will go against is set.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>An accumulative assertion ready for further build up</returns>
        public IAccumulativeAssertion ForTable(string tableName)
        {
            return new AccumulativeAssertion(ConnectionExecutorFinder.GetSqlExecutor(connectionName), tableName);
        }

        /// <summary>
        /// Initializes and returns an executable asserion based on the given
        /// sql. This sql will be executed as is, so no further accumulative 
        /// build up is available here.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>The executable assertion</returns>
        public IExecutableAssertion WithSql(string sql)
        {
            var assertion = new SqlBasedAccumulativeAssertion(ConnectionExecutorFinder.GetSqlExecutor(connectionName));
            assertion.SetSql(sql);
            return assertion;
        }
    }
}
