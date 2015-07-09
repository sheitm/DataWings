using System;
using DataWings.Common;

namespace DataWings.DataMaintenance
{
    public interface IReturnValue : IColumnValue
    {
        IDataAccumulatorRow To(string propertyName);

        IDataAccumulatorRow ToLast();
    }

    public class ReturnValue : IReturnValue
    {
        private readonly IDataAccumulatorRow _row;

        public ReturnValue(string columnName, IDataAccumulatorRow row)
        {
            ColumnName = columnName;
            _row = row;
        }

        public string ColumnName { get; private set; }

        public string ReturnValueName { get; set; }

        public IDataAccumulatorRow To(string returnValueName)
        {
            ReturnValueName = returnValueName;
            return _row;
        }

        public IDataAccumulatorRow ToLast()
        {
            return _row;
        }

        public string Key
        {
            get { return ColumnName; }
        }

        public void SetValue(object val, bool shouldFormat)
        {
            throw new NotImplementedException();
        }

        public string GetValue(SqlVendor vendor)
        {
            return Value;
        }

        public string Value
        {
            get
            {
                if (ReturnValueName == null)
                    return DataSession.GetLastReturnValue().ToSqlValueString();
                return DataSession.GetReturnValueAt(ReturnValueName).ToSqlValueString();
            }
        }
    }
}
