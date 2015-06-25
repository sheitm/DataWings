namespace DataWings.DataAdversary
{
    /// <summary>
    /// Wraps a named table and is able to receive further information
    /// about the concrete database row which is to be "tampered with"
    /// by the adversary 
    /// </summary>
    public interface IAccumulativeAdversary
    {
        /// <summary>
        /// Sets the columnName/columnValue pair that uniquely identify
        /// the concrete row which the adversary is to alter
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IExecutableAdversary IdentifiedBy(string columnName, object value);
    }

    /// <summary>
    /// Defines the API for executing the actual adversary action.
    /// </summary>
    public interface IExecutableAdversary
    {
        /// <summary>
        /// Assumes that the rdb data type of the named column is some
        /// sort of number. Increases the number by 1.
        /// </summary>
        /// <param name="rowVersionColumnName">Name of the row version column.</param>
        void IncRowVersion(string rowVersionColumnName);


    }
}
