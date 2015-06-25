namespace DataWings.Assertions
{
    /// <summary>
    /// Used during the assertion build-up to accumulate terms that finally
    /// will constitute a complete assertion.
    /// </summary>
    public interface IAccumulativeAssertion : IExecutableAssertion
    {
        /// <summary>
        /// Adds the column value pair to the acummulative assertion
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IAccumulativeAssertion WithColumnValuePair(string columnName, object value);
        
    }
}