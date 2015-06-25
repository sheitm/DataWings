using System;

namespace DataWings.Common
{
    /// <summary>
    /// Static class with a single very simple function: holding onto
    /// the reference to a sqlprovider factory that can be set from
    /// the outside. Thus, you can extend DataWings with your own 
    /// provider factory and so support database engines that are not natively 
    /// supported by ThidHand
    /// </summary>
    public static class ProvisionedProvider
    {
        private static ISqlProviderFactory factory;

        /// <summary>
        /// Sets the factory.
        /// </summary>
        /// <param name="fact">The fact.</param>
        public static void SetFactory(ISqlProviderFactory fact)
        {
            factory = fact;
        }

        /// <summary>
        /// Resets by removing the factory.
        /// </summary>
        public static void Reset()
        {
            factory = null;
        }

        /// <summary>
        /// Gets a new provider initialized wth the given connection 
        /// string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static ISqlProvider GetProvider(string connectionString)
        {
            if (factory == null)
                throw new InvalidOperationException("Factory not set.");
            return factory.CreateProvider(connectionString);
        }
    }
}
