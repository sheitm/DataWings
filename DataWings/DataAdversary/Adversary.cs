using DataWings.Common;

namespace DataWings.DataAdversary
{
    /// <summary>
    /// Static gateway to the adversary functionality
    /// </summary>
    public static class Adversary
    {
        /// <summary>
        /// Returns an accumultaive adversary which is designed to
        /// receive further information identifying the concrete
        /// row to tamper with 
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static IAccumulativeAdversary ForTable(string tableName)
        {
            return new AdversaryInstance().ForTable(tableName);
        }

        /// <summary>
        /// Returns an assertion with the name of the connection specificaton
        /// set == connectionName.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static AdversaryInstance ForConnection(string connectionName)
        {
            return new AdversaryInstance(connectionName);
        }
    }

    public class AdversaryInstance
    {
        private string connectionKey;

        public AdversaryInstance()
        {}

        public AdversaryInstance(string connectionKey)
        {
            this.connectionKey = connectionKey;
        }

        /// <summary>
        /// Returns an accumultaive adversary which is designed to
        /// receive further information identifying the concrete
        /// row to tamper with 
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public IAccumulativeAdversary ForTable(string tableName)
        {
            var executor = ConnectionExecutorFinder.GetSqlExecutor(connectionKey);
            return new ExecutableAdversary(executor, tableName);
        }
    }
}
