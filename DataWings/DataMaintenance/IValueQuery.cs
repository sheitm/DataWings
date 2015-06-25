namespace DataWings.DataMaintenance
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValueQuery
    {
        /// <summary>
        /// Returns a column name/value constraint. The column name
        /// has been set, and next in line is the value.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        IValueQueryWhere Where(string columnName);
    }

    /// <summary>
    /// A fully initilized value query; initialized in the sense that
    /// a complete sql query string may be generated on the basis of
    /// the internal structure of the query.
    /// </summary>
    public interface IInitializedValueQuey
    {
        /// <summary>
        /// Returns a column name/value constraint. The column name
        /// has been set, and next in line is the value.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        IValueQueryWhere And(string columnName);

        /// <summary>
        /// Gets the value of the given column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName"></param>
        /// <returns></returns>
        T GetValueForColumn<T>(string columnName);

        /// <summary>
        /// Gets the value of the given column.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        object GetValueForColumn(string columnName);
    }

    /// <summary>
    /// Used to build of column name + value where constraints
    /// </summary>
    public interface IValueQueryWhere
    {
        /// <summary>
        /// API for specifying the value constraint of a column/value
        /// constraint pair. Returns a fully initialize value query
        /// (ready for execution):
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IInitializedValueQuey Eq(object value);
    }
}
