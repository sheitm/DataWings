using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;

namespace DataWings.Common
{
    /// <summary>
    /// Abstract base implementation of sql provider functionality
    /// </summary>
    public abstract class SqlProviderBase : ISqlProvider
    {
        private string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProviderBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlProviderBase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Executes the select and returns the list of rows
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="selectOptions">The select options.</param>
        /// <returns>The list of rows</returns>
        public IList<ISqlResult> ExecuteQuery(string sql, SelectOptions selectOptions)
        {
            if (SelectOptions.Single == selectOptions)
                return ExecuteSingleSelect(sql);
            return ExecuteMultiSelect(sql);
        }

        public void ExecuteNonQuery(string sql)
        {
            AboutToExecuteSql(sql);

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            SqlExecuted(sql);
        }

        public string ConnectionString
        {
            get { return connectionString; }
        }

        private IList<ISqlResult> ExecuteSingleSelect(string sql)
        {
            AboutToExecuteSql(sql);

            List<ISqlResult> finalResult;
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return new List<ISqlResult>();
                    var result = new SqlResult();
                    result.SetData(reader);
                    finalResult = new List<ISqlResult> { result };
                }
            }
            SqlExecuted(sql);
            return finalResult;
        }

        private IList<ISqlResult> ExecuteMultiSelect(string sql)
        {
            AboutToExecuteSql(sql);

            IList<ISqlResult> result;
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    result = new List<ISqlResult>();
                    while (reader.Read())
                    {
                        var sqlRes = new SqlResult();
                        sqlRes.SetData(reader);
                        result.Add(sqlRes);
                    }
                }
            }
            SqlExecuted(sql);
            return result;
        }


        private DbConnection GetConnection()
        {
            return GetConnectionCore(connectionString);
        }

        /// <summary>
        /// Gets the connection. Must be overridden by subclasses.
        /// </summary>
        /// <param name="connString">The conn string.</param>
        /// <returns></returns>
        protected abstract DbConnection GetConnectionCore(string connString);

        /// <summary>
        /// Is invoked whenever an sql statement has been executed by
        /// the underlying physical database connection.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        protected virtual void SqlExecuted(string sql)
        {}

        private void AboutToExecuteSql(string sql)
        {
            if (sql == null)
                throw new ArgumentNullException("sql");
            if (connectionString == null)
                throw new InvalidOperationException("Connection string not set!");
            Trace.WriteLine("DataWings: " + sql);
            AboutToExecuteSqlCore(sql);
        }

        /// <summary>
        /// Is invoked just before an sql is about to be invoked.
        /// Provides a hook that can be overridden for custom
        /// behavior.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        protected virtual void AboutToExecuteSqlCore(string sql)
        { }
    }
}
