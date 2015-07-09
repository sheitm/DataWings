using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DataWings.Common;

namespace DataWings.DataMaintenance
{
    /// <summary>
    /// Represents a single data row to be inserted into the database.
    /// Can be built up (accumulated) with any number of row definitions,
    /// each specifying data that should be inserted in the table 
    /// </summary>
    public class DataRow : IDataAccumulatorRow
    {
        #region Constructor and Declarations

        private DataBatch parent;
        private readonly Dictionary<string, IColumnValue> _columnValues = new Dictionary<string, IColumnValue>();
        private readonly List<ReturnValueCommand> _returnValueCommands = new List<ReturnValueCommand>();
        private KeyValuePair<string, string> idKeyValuePair;
        private bool deleteFirst = false;
        private bool update = false;
        private bool delete = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRow"/> class.
        /// </summary>
        /// <param name="parent"></param>
        public DataRow(DataBatch parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRow"/> class.
        /// </summary>
        /// <param name="idColumnName">Name of the id column.</param>
        /// <param name="id">The id.</param>
        /// <param name="parent">The parent.</param>
        public DataRow(string idColumnName, object id, DataBatch parent) : this(parent)
        {
            Data(idColumnName, id);
            idKeyValuePair = new KeyValuePair<string, string>(idColumnName, id.ToSqlValueString());
        }

        #endregion

        #region Public API

        /// <summary>
        /// Writes the data accumulated in this row to the database.
        /// </summary>
        public void DoWrite(ISqlProvider provider)
        {
            if (delete)
            {
                Delete(provider);
            }
            else
            {
                string statement = update ? GetUpdateStatement(provider.Vendor) : GetInsertStatement(provider.Vendor);
                provider.ExecuteNonQuery(statement);
            }

            string appendableSql = GetSelectStatementForReturnValueCommand();
            foreach (var command in _returnValueCommands)
            {
                command.SetValue(appendableSql, provider);
            }
        }

        /// <summary>
        /// Deletes the row from the database if it has been marked as
        /// deletable through DeleteFirst()
        /// </summary>
        public void DoDelete(ISqlProvider provider)
        {
            if (deleteFirst)
            {
                Delete(provider);
            }
        }

        #endregion

        #region IDataAccumulatorRow Members


        /// <summary>
        /// Creates a new IDataAccumulatorBatch and adds it to the
        /// inner data structure. This instance of IDataAccumulatorBatch
        /// represents a batch of operations that are to be invoked
        /// against the table with the given name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public IDataAccumulatorBatch ForTable(string tableName)
        {
            return parent.ForTable(tableName);
        }

        /// <summary>
        /// Commits the accumulative session, i.e. performs the actual
        /// changes in the database
        /// </summary>
        public void Commit()
        {
            parent.Commit();
        }

        /// <summary>
        /// Creates a new accumulative row and adds it to the inner
        /// data structure
        /// </summary>
        /// <param name="idColumnName">Name of the id column.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IDataAccumulatorRow Row(string idColumnName, object id)
        {
            return parent.Row(idColumnName, id);
        }

        public void Values(params IColumnValuePair[] columnValuePairs)
        {
            if (columnValuePairs == null)
                throw new ArgumentException("Can not be null: columnValuePairs.");

            foreach (var pair in columnValuePairs)
            {
                Data(pair.ColumnName, pair.Value);
            }
            Commit();
        }

        /// <summary>
        /// Sets update mode on the row so that an update statement will
        /// be generated and executed instead of an insert (insert is the
        /// default)
        /// </summary>
        /// <returns></returns>
        public IDataAccumulatorRow ForUpdate()
        {
            delete = false;
            update = true;
            return this;
        }

        /// <summary>
        /// Sets the mode so that the row will be deleted upon execution
        /// </summary>
        /// <returns></returns>
        public IDataAccumulatorRow ForDelete()
        {
            update = false;
            delete = true;
            return this;
        }

        /// <summary>
        /// Add the column name/value pair
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IDataAccumulatorRow Data(string columnName, object value)
        {
            AddDataCore(columnName, value, true);
            return this;
        }

        /// <summary>
        /// Exactly equivalent with the Data() method with the same
        /// signature. "D" is just a useful shorthand.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDataAccumulatorRow D(string columnName, object value)
        {
            return Data(columnName, value);
        }

        /// <summary>
        /// The incoming parameter must be on this format:
        /// "FirstName='Billy';LastName='Hansen';MiddleName='Jean';Sex='M'"
        /// The string is parsed and split into individual column/value
        /// pairs that are used to populate the row.
        /// </summary>
        /// <param name="columnValueString"></param>
        /// <returns></returns>
        public IDataAccumulatorRow Data(string columnValueString)
        {
            if (columnValueString == null)
                throw new ArgumentNullException("columnValueString");

            foreach (var pair in columnValueString.ToColumnValuePairs())
            {
                AddDataCore(pair.Key, pair.Value, false);
            }
            return this;
        }

        /// <summary>
        /// Exactly equivalent with the Data() method with the same
        /// signature. "D" is just a useful shorthand.
        /// </summary>
        /// <param name="columnValueString">The column value string.</param>
        /// <returns></returns>
        public IDataAccumulatorRow D(string columnValueString)
        {
            return Data(columnValueString);
        }

        /// <summary>
        /// Deletes the data row from the database before inserting
        /// </summary>
        /// <returns></returns>
        public IDataAccumulatorRow DeleteFirst()
        {
            deleteFirst = true;
            return this;
        }

        /// <summary>
        /// Creates and returns a return value command
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public IReturnValueCommand ReturnValue(string columnName)
        {
            return new ReturnValueCommand(columnName, this);
        }

        public IReturnValue BindColumn(string propertyName)
        {
            var returnValue = new ReturnValue(propertyName, this);
            _columnValues.Add(propertyName, returnValue);
            return returnValue;
        }

        #endregion

        #region Private

        internal void AddReturnValueCommand(ReturnValueCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            _returnValueCommands.Add(command);
            DataSession.AddReturnValueCommand(command);
        }

        private void Delete(ISqlProvider provider)
        {
            provider.ExecuteNonQuery(GetDeleteStatement());
        }

        private string GetDeleteStatement()
        {
            return String.Format("DELETE FROM {0} WHERE {1} = {2}", 
                                 parent.TableName, 
                                 idKeyValuePair.Key, 
                                 idKeyValuePair.Value);
        }

        private string GetUpdateStatement(SqlVendor vendor)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("UPDATE {0} SET ", parent.TableName);
            int i = 0;
            // Can't include unique id column name in update because
            // it is considered an error in some database engines
            // (SQL Server, for instance).
            var query = _columnValues.Where(pair => pair.Key.ToUpper() != idKeyValuePair.Key.ToUpper());
            int queryCount = query.Count();
            foreach (var pair in query)
            {
                var columnValue = pair.Value;
                sb.AppendFormat("{0} = {1}", columnValue.Key, columnValue.GetValue(vendor));
                if (i < (queryCount - 1))
                {
                    sb.Append(", ");
                }
                i++;
            }
            sb.AppendFormat(" WHERE {0} = {1}", idKeyValuePair.Key, idKeyValuePair.Value);
            return sb.ToString();
        }

        private string GetSelectStatementForReturnValueCommand()
        {
            var sb = new StringBuilder();
            sb.Append("SELECT {0} FROM ");
            sb.AppendFormat("{0} WHERE {1} = {2}", parent.TableName, idKeyValuePair.Key, idKeyValuePair.Value);
            return sb.ToString();
        }

        private string GetInsertStatement(SqlVendor vendor)
        {
            var headerSb = new StringBuilder();
            var valuesSb = new StringBuilder();

            headerSb.AppendFormat("INSERT INTO {0} (", parent.TableName);
            valuesSb.Append(" VALUES(");
            int i = 0;
            foreach (var columnValue in _columnValues.Values)
            {
                headerSb.Append(columnValue.Key);
                valuesSb.Append(columnValue.GetValue(vendor));
                if (i < (_columnValues.Count-1))
                {
                    headerSb.Append(", ");
                    valuesSb.Append(", ");
                }
                i++;
            }
            headerSb.Append(")");
            valuesSb.Append(")");
            headerSb.Append(valuesSb);
            return headerSb.ToString();
        }

        private void AddDataCore(string columnName, object value, bool shouldFormatValue)
        {
            if (columnName == null)
                throw new ArgumentNullException("columnName");

            string key = columnName.ToUpper();
            if (_columnValues.ContainsKey(key))
                throw new ArgumentException("Column named " + columnName + " already added");

            var columnValue = new ColumnValue { Key = key };
            columnValue.SetValue(value, shouldFormatValue);
            _columnValues.Add(key, columnValue);
        }

        #endregion
    }

    public interface IColumnValue
    {
        string Key { get; }
        void SetValue(object val, bool shouldFormat);
        string GetValue(SqlVendor vendor);
    }

    public class ColumnValue : IColumnValue
    {
        private object _value;
        private bool _shouldFormat;

        public string Key { get; set; }
        public void SetValue(object val, bool shouldFormat)
        {
            _value = val;
            _shouldFormat = shouldFormat;
        }

        public string GetValue(SqlVendor vendor)
        {
            if (SqlVendor.Oracle == vendor && _value is DateTime)
            {
                var dt = (DateTime) _value;
                return string.Format(
                    "to_date('{0}/{1}/{2} {3}:{4}:{5}', 'YYYY/MM/DD HH:MI:SS')",
                    dt.Year,
                    dt.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    dt.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    dt.Hour.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    dt.Minute.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    dt.Second.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
            }

            return _shouldFormat ? _value.ToSqlValueString() : _value.ToString();
        }
    }
}