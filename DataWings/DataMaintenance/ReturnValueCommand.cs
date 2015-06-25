using DataWings.Common;

namespace DataWings.DataMaintenance
{
    /// <summary>
    /// A way of getting values from newly inserted or updated rows back
    /// and accessible for subsequent commands against other rows that are
    /// dependent on the new inserted/updated row.
    /// </summary>
    public class ReturnValueCommand : IReturnValueCommand
    {
        private DataRow _row;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="columnName">The name of the column to return</param>
        /// <param name="row">The parent row</param>
        public ReturnValueCommand(string columnName, DataRow row)
        {
            ColumnName = columnName;
            _row = row;
        }

        /// <summary>
        /// Make the value accesible immediatly witout the use of a key.
        /// This means that the return value will be lost forever if
        /// another return value command is set up subsequently.
        /// </summary>
        /// <returns>The parent row</returns>
        public IDataAccumulatorRow ForImmediateUse()
        {
            _row.AddReturnValueCommand(this);
            return _row;
        }

        /// <summary>
        /// This return value command will be keyed at the given
        /// key, and this key thus functions as the name for the return
        /// value, and the concrete value is then accesible in all
        /// subsequent queries through this key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDataAccumulatorRow AtKey(string key)
        {
            Key = key;
            return ForImmediateUse();
        }

        /// <summary>
        /// The column from which to obtain the value.
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// The name of this return value command
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// The value from the row in the database.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Sets the value by querying the data base (through the given provider)
        /// </summary>
        /// <param name="appendableSelectStatement"></param>
        /// <param name="provider"></param>
        public void SetValue(string appendableSelectStatement, ISqlProvider provider)
        {
            string sql = string.Format(appendableSelectStatement, ColumnName);
            var result = provider.ExecuteQuery(sql, SelectOptions.Single)[0];
            Value = result.GetSingleResult();
        }
    }
}
