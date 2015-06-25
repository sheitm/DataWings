namespace DataWings.Common
{
    /// <summary>
    /// Resposible for creating instance of ISqlProvider
    /// </summary>
    public interface ISqlProviderFactory
    {
        /// <summary>
        /// Creates a new provider. This provider is initialized with
        /// the given connection string
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        ISqlProvider CreateProvider(string connectionString);
    }
}
