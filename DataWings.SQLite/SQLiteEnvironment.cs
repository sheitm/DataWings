using DataWings.Common;

namespace DataWings.SQLite
{
    /// <summary>
    /// Static gateway for setting SQLite as provisioned provider
    /// </summary>
    public static class SQLiteEnvironment
    {
        /// <summary>
        /// Initializes by setting SQLite as provisioned provider
        /// </summary>
        public static void Initialize()
        {
            ProvisionedProvider.SetFactory(new SQLiteProviderFactory());
        }
    }
}
