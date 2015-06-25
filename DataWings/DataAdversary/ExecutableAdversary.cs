using System;
using DataWings.Common;

namespace DataWings.DataAdversary
{
    /// <summary>
    /// Implements both IAccumulativeAdversary and IExecutableAdversary, and thereby functions
    /// as both an accumulator for data needed to perform the adversary action, and
    /// as the entity that executes the adversary action.
    /// </summary>
    public class ExecutableAdversary : IAccumulativeAdversary, IExecutableAdversary
    {
        #region Constructor and Declarations

        private string tableName;
        private string columnName;
        private object id;
        private ISqlProvider SqlProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutableAdversary"/> class.
        /// </summary>
        /// <param name="SqlProvider">The SQL provider.</param>
        /// <param name="tableName">Name of the table.</param>
        public ExecutableAdversary(ISqlProvider SqlProvider, string tableName)
        {
            this.SqlProvider = SqlProvider;
            this.tableName = tableName;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Sets the columnName/columnValue pair that uniquely identify
        /// the concrete row which the adversary is to alter
        /// </summary>
        /// <param name="cName">Name of the c.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IExecutableAdversary IdentifiedBy(string cName, object value)
        {
            columnName = cName;
            id = value;
            return this;
        }

        /// <summary>
        /// Assumes that the rdb data type of the named column is some
        /// sort of number. Increases the number by 1.
        /// </summary>
        /// <param name="rowVersionColumnName">Name of the row version column.</param>
        public void IncRowVersion(string rowVersionColumnName)
        {
            string updateSql = String.Format("UPDATE {0} SET {1} = {1} + 1 WHERE {2} = {3}",
                tableName,
                rowVersionColumnName,
                columnName,
                id.ToSqlValueString());
            SqlProvider.ExecuteNonQuery(updateSql);
        }

        #endregion

    }
}
