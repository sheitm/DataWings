using System;
using System.Collections.Generic;
using System.Text;
using DataWings.Common;

namespace DataWings.Assertions
{
    /// <summary>
    /// An assertion which can be built up with additional terms until an executive
    /// message is invoked.
    /// </summary>
    public class AccumulativeAssertion : IAccumulativeAssertion
    {
        #region Constructor and Declarations

        private readonly ISqlProvider sqlProvider;
        private ISqlResult sqlResult;

        private const string FAILED_EXISTS_MESSAGE = "No row with column values [{0}] in table {1} exists.";
        private const string FAILED_NOTEXISTS_MESSAGE = "Row with column value [{0}] in table {1} expected not to exist, but exists.";
        private const string FAILED_AREEQUALMESSAGE = "Expected {0} but got {1}.";

        private readonly List<KeyValuePair<string, object>> whereColumnPairs = new List<KeyValuePair<string, object>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AccumulativeAssertion"/> class.
        /// </summary>
        /// <param name="SqlProvider">The SQL provider.</param>
        public AccumulativeAssertion(ISqlProvider SqlProvider)
        {
            this.sqlProvider = SqlProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccumulativeAssertion"/> class.
        /// </summary>
        /// <param name="SqlProvider">The SQL provider.</param>
        /// <param name="tableName">Name of the table.</param>
        public AccumulativeAssertion(ISqlProvider SqlProvider, string tableName)
            : this(SqlProvider)
        {
            TableName = tableName;
        }

        #endregion

        #region IAssertion Members

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName { get; set; }

        /// <summary>
        /// Adds the where column.
        /// </summary>
        /// <param name="whereColumn">The where column.</param>
        /// <param name="val">The val.</param>
        public void AddWhereColumn(string whereColumn, object val)
        {
            whereColumnPairs.Add(new KeyValuePair<string, object>(whereColumn, val));
        }

        /// <summary>
        /// Gets the where column display string.
        /// </summary>
        /// <value>The where column display string.</value>
        public string WhereColumnDisplayString
        {
            get
            {
                return GetWhereColumnValueString(":", ",");
            }
        }

        #endregion

        #region IAccumulativeAssertion Members

        /// <summary>
        /// Adds the column value pair to the acummulative assertion
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IAccumulativeAssertion WithColumnValuePair(string columnName, object value)
        {
            AddWhereColumn(columnName, value);
            return this;
        }

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// Asserts that a single row matching the criteria exists.
        /// </summary>
        public void Exists()
        {
            Exists(null);
        }

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// Asserts that a single row matching the criteria exists.
        /// </summary>
        /// <param name="failedAssertionMessage">A message to be displayed if the assertion fails</param>
        public void Exists(string failedAssertionMessage)
        {
            ExistsCore(
                c => c > 0,
                FAILED_EXISTS_MESSAGE,
                failedAssertionMessage);
        }

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// Asserts that a single row matching the criteria does not
        /// exist.
        /// </summary>
        public void NotExists()
        {
            NotExists(null);
        }

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// Asserts that a single row matching the criteria does not
        /// exist.
        /// </summary>
        /// <param name="failedAssertionMessage">A message to be displayed if the assertion fails</param>
        public void NotExists(string failedAssertionMessage)
        {
            ExistsCore(
                c => c == 0,
                FAILED_NOTEXISTS_MESSAGE,
                failedAssertionMessage);
        }



        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// Asserts that the value in the db
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="compareWith"></param>
        public IExecutableAssertion AreEqual(string columnName, object compareWith)
        {
            AreEqual(columnName, compareWith, FAILED_AREEQUALMESSAGE);
            return this;
        }

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="compareWith"></param>
        /// <param name="failedAssertionMessage">A message to be displayed if the assertion fails</param>
        public IExecutableAssertion AreEqual(string columnName, object compareWith, string failedAssertionMessage)
        {
            EqualsCore(columnName, compareWith, failedAssertionMessage);
            return this;
        }

        public void Evaluate(Func<ISqlResult, bool> function)
        {
            AssertThat.IsTrue(function.Invoke(GetSqlResult()));
        }

        #endregion

        #region Private

        private decimal CountRows()
        {
            string sql = String.Format("SELECT count(*) FROM {0} WHERE {1}",
                                       TableName,
                                       GetWhereColumnValueString("=", "AND"));
            var rows = SqlProvider.ExecuteQuery(sql, SelectOptions.Single);
            if (rows == null || rows.Count == 0) return 0;

            // Problem: different vendors return different
            // data types for numerics. We standardize on
            // decimal
            object val = rows[0].GetSingleResult();
            if (val == null) return 0;
            return Decimal.Parse(val.ToString());
        }

        private ISqlResult GetSqlResult()
        {
            if (sqlResult == null)
            {
                string sql = String.Format("SELECT * FROM {0} WHERE {1}",
                                           TableName,
                                           GetWhereColumnValueString("=", "AND"));
                var rows = SqlProvider.ExecuteQuery(sql, SelectOptions.Single);
                if (rows.Count > 0)
                    sqlResult = rows[0];
            }
            return sqlResult;
        }

        private object GetValueFromDb(string columnName)
        {
            return GetSqlResult().GetResult(columnName);
        }

        private string GetWhereColumnValueString(string pairInjectionString, string injectionString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < whereColumnPairs.Count; i++)
            {
                var pair = whereColumnPairs[i];
                sb.AppendFormat("{0} {2} {1}", pair.Key, pair.Value.ToSqlValueString(), pairInjectionString);
                if (i < (whereColumnPairs.Count - 1)) sb.AppendFormat("{0} ", injectionString);
            }
            return sb.ToString();
        }

        protected virtual void ExistsCore(Func<decimal, bool> comparer, string messageTemplate, string additionalMessage)
        {
            AssertThat.IsTrue(
                comparer.Invoke(CountRows()),
                messageTemplate.ToFailedAssertionMessage(this, additionalMessage));
        }

        protected virtual void EqualsCore(string columnName, object compareWith, string failedAssertionMessage)
        {
            AssertThat.AreEqual(
                compareWith,
                GetValueFromDb(columnName),
                FAILED_AREEQUALMESSAGE);
            

        }

        protected ISqlProvider SqlProvider
        {
            get { return sqlProvider; }
        }

        #endregion


    }
}