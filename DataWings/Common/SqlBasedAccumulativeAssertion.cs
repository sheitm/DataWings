using System;
using System.Diagnostics;
using DataWings.Assertions;

namespace DataWings.Common
{
    /// <summary>
    /// An assertion that is based directly on a finished sql statement. This
    /// assertion can not be built up any further
    /// </summary>
    public class SqlBasedAccumulativeAssertion : AccumulativeAssertion
    {
        #region Constructor and Declarations

        private string sqlStatement;
        private ISqlResult executionResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBasedAccumulativeAssertion"/> class.
        /// </summary>
        /// <param name="SqlProvider">The SQL provider.</param>
        public SqlBasedAccumulativeAssertion(ISqlProvider SqlProvider)
            : base(SqlProvider)
        { }

        #endregion

        /// <summary>
        /// Sets the SQL.
        /// </summary>
        /// <param name="sqlStmt">The SQL STMT.</param>
        public void SetSql(string sqlStmt)
        {
            sqlStatement = sqlStmt;
        }

        protected override void EqualsCore(string columnName, object compareWith, string failedAssertionMessage)
        {
            AssertThat.AreEqual(compareWith, ExecutionResult.GetResult(columnName.ToUpper()), failedAssertionMessage);
        }

        protected override void ExistsCore(Func<decimal, bool> comparer, string messageTemplate, string additionalMessage)
        {
            AssertThat.IsNotNull(ExecutionResult,additionalMessage);
        }

        private ISqlResult ExecutionResult
        {
            get
            {
                if (executionResult == null)
                {
                    Debug.Assert(sqlStatement != null);

                    var rows = SqlProvider.ExecuteQuery(sqlStatement, SelectOptions.Single);
                    if (rows.Count > 0)
                        executionResult = rows[0];
                }
                return executionResult;
            }
        }

    }
}