namespace DataWings.DataMaintenance
{
    /// <summary>
    /// An accumulator session is a construct that can be built up
    /// (accumulated) with any number of IDataAccumulatorBatch objects
    /// each containing data that should be written to the database
    /// </summary>
    public interface IDataAccumulatorSession
    {
        /// <summary>
        /// Creates a new IDataAccumulatorBatch and adds it to the 
        /// inner data structure. This instance of IDataAccumulatorBatch
        /// represents a batch of operations that are to be invoked
        /// against the table with the given name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        IDataAccumulatorBatch ForTable(string tableName);

        /// <summary>
        /// Commits the accumulative session, i.e. performs the actual
        /// changes in the database
        /// </summary>
        void Commit();
    }

    /// <summary>
    /// Can be built up (accumulated) with any number of row definitions,
    /// each specifying data that should be inserted in the table 
    /// </summary>
    public interface IDataAccumulatorBatch : IDataAccumulatorSession
    {
        /// <summary>
        /// Creates a new accumulative row and adds it to the inner
        /// data structure
        /// </summary>
        /// <param name="idColumnName">Name of the id column.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IDataAccumulatorRow Row(string idColumnName, object id);

        void Values(params IColumnValuePair[] columnValuePairs);
    }

    /// <summary>
    /// An accumulative row is a construct that can be built up
    /// (accumulated) by colum name/value pairs that should be
    /// inserted as a single row in the table of the batch in
    /// which the row is contained.
    /// </summary>
    public interface IDataAccumulatorRow : IDataAccumulatorBatch
    {

        /// <summary>
        /// Sets update mode on the row so that an update statement will
        /// be generated and executed instead of an insert (insert is the
        /// default)
        /// </summary>
        /// <returns></returns>
        IDataAccumulatorRow ForUpdate();

        /// <summary>
        /// Sets the mode so that the row will be deleted upon execution
        /// </summary>
        /// <returns></returns>
        IDataAccumulatorRow ForDelete();

        /// <summary>
        /// Add the column name/value pair
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IDataAccumulatorRow Data(string columnName, object value);

        /// <summary>
        /// Exactly equivalent with the Data() method with the same
        /// signature. "D" is just a useful shorthand.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IDataAccumulatorRow D(string columnName, object value);

        /// <summary>
        /// The incoming parameter must be on this format:
        /// "FirstName='Billy';LastName='Hansen';MiddleName='Jean';Sex='M'"
        /// 
        /// The string is parsed and split into individual column/value
        /// pairs that are used to populate the row.
        /// </summary>
        /// <param name="columnValueString"></param>
        /// <returns></returns>
        IDataAccumulatorRow Data(string columnValueString);


        /// <summary>
        /// Exactly equivalent with the Data() method with the same
        /// signature. "D" is just a useful shorthand.
        /// </summary>
        /// <param name="columnValueString">The column value string.</param>
        /// <returns></returns>
        IDataAccumulatorRow D(string columnValueString);

        /// <summary>
        /// Deletes the data row from the database before inserting
        /// </summary>
        /// <returns></returns>
        IDataAccumulatorRow DeleteFirst();

        /// <summary>
        /// Creates and returns a return value command
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        IReturnValueCommand ReturnValue(string columnName);

        IReturnValue BindColumn(string propertyName);
    }

    public interface IBindableValue
    {
        
    }

    /// <summary>
    /// A way of getting values from newly inserted or updated rows back
    /// and accessible for subsequent commands against other rows that are
    /// dependent on the new inserted/updated row.
    /// </summary>
    public interface IReturnValueCommand
    {
        /// <summary>
        /// Make the value accesible immediatly witout the use of a key.
        /// This means that the return value will be lost forever if
        /// another return value command is set up subsequently.
        /// </summary>
        /// <returns>The parent row</returns>
        IDataAccumulatorRow ForImmediateUse();

        /// <summary>
        /// This return value command will be keyed at the given
        /// key, and this key thus functions as the name for the return
        /// value, and the concrete value is then accesible in all
        /// subsequent queries through this key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IDataAccumulatorRow AtKey(string key);
    }
}