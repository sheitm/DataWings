namespace DataWings.DataMaintenance
{
    public class Column : IColumnValuePair, IHalfFinishedColumnValuePair
    {
        public static IHalfFinishedColumnValuePair Named(string columnName)
        {
            return new Column(columnName);
        }

        private string _columnName;
        private object _value;

        private Column(string columnName)
        {
            _columnName = columnName;
        }

        public string ColumnName
        {
            get { return _columnName; }
        }

        public object Value
        {
            get { return _value; }
        }

        public IColumnValuePair Eq(object value)
        {
            _value = value;
            return this;
        }
    }
}
