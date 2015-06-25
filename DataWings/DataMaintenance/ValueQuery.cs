using System;
using System.Collections.Generic;
using System.Text;
using DataWings.Common;

namespace DataWings.DataMaintenance
{
    /// <summary>
    /// Callback delegate for setting column name/value constraints on
    /// a query
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate IInitializedValueQuey WhereContraintAdder(string columnName, object value);

    /// <summary>
    /// Implements both IValueQuery and IInitializedValueQuey. I.e. this class
    /// contains functionality for both building up and executing value queries.
    /// </summary>
    public class ValueQuery : IValueQuery, IInitializedValueQuey
    {
        #region Constructor and Declarations

        private string _tableName;
        private readonly Dictionary<string, object> _whereConstraints = new Dictionary<string, object>();
        private ISqlProvider _sqlProvider;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sqlProvider"></param>
        public ValueQuery(string tableName, ISqlProvider sqlProvider)
        {
            _tableName = tableName;
            _sqlProvider = sqlProvider;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Returns a column name/value constraint. The column name
        /// has been set, and next in line is the value.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public IValueQueryWhere Where(string columnName)
        {
            return new ValueQueryWhere(AddWhereConstraint, columnName);
        }

        /// <summary>
        /// Returns a column name/value constraint. The column name
        /// has been set, and next in line is the value.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public IValueQueryWhere And(string columnName)
        {
            return new ValueQueryWhere(AddWhereConstraint, columnName);
        }

        /// <summary>
        /// Gets the value of the given column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public T GetValueForColumn<T>(string columnName)
        {
            return (T) GetValueForColumn(columnName);
        }

        /// <summary>
        /// Gets the value of the given column.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object GetValueForColumn(string columnName)
        {
            var sqlResult = GetSqlResult();
            return sqlResult.GetResult(columnName);
        }

        #endregion

        #region Internal and Private

        private IInitializedValueQuey AddWhereConstraint(string columName, object value)
        {
            if (_whereConstraints.ContainsKey(columName))
                throw new ArgumentException(string.Format("Constraint for column {0} already added.", columName));
            _whereConstraints.Add(columName, value);
            return this;
        }

        private ISqlResult GetSqlResult()
        {
            string sql = String.Format("SELECT * FROM {0} WHERE {1}",
                                           _tableName,
                                           GetWhereColumnValueString("=", "AND"));
            var rows = _sqlProvider.ExecuteQuery(sql, SelectOptions.Single);
            if (rows.Count > 0)
                return rows[0];
            return null;
        }

        private string GetWhereColumnValueString(string pairInjectionString, string injectionString)
        {
            var sb = new StringBuilder();
            var whereColumnPairs = new List<KeyValuePair<string, object>>();
            foreach (var pair in _whereConstraints)
            {
                whereColumnPairs.Add(pair);
            }
            for (int i = 0; i < whereColumnPairs.Count; i++)
            {
                var pair = whereColumnPairs[i];
                sb.AppendFormat("{0} {2} {1}", pair.Key, pair.Value.ToSqlValueString(), pairInjectionString);
                if (i < (whereColumnPairs.Count - 1)) sb.AppendFormat("{0} ", injectionString);
            }
            return sb.ToString();
        }

        #endregion


    }
}
