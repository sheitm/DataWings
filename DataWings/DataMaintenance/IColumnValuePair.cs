namespace DataWings.DataMaintenance
{
    public interface IColumnValuePair
    {
        string ColumnName { get; }

        object Value { get; }
    }

    public interface IHalfFinishedColumnValuePair
    {
        IColumnValuePair Eq(object value);
    }
}
