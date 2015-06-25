namespace DataWings.Common
{
    /// <summary>
    /// Represents a single row returned from the database
    /// </summary>
    public interface ISqlResult
    {
        /// <summary>
        /// Gets the single result, i.e. the first column/value pair
        /// encountered. This method is probably most useful in situations
        /// when one knows that only one row is returned
        /// </summary>
        /// <returns>The first column/value pair encountered</returns>
        object GetSingleResult();

        /// <summary>
        /// Gets the single result, i.e. the first column/value pair
        /// encountered. This method is probably most useful in situations
        /// when one knows that only one row is returned
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <returns>The first column/value pair encountered</returns>
        T GetSingleResult<T>();

        /// <summary>
        /// Gets the result keyed at the given key. This key should be
        /// a column name. The key is case insensitive (in keeping with
        /// sql standards).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value</returns>
        object GetResult(string key);

        /// <summary>
        /// Gets the result keyed at the given key. This key should be
        /// a column name. The key is case insensitive (in keeping with
        /// sql standards).
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The value</returns>
        T GetResult<T>(string key);
    }
}