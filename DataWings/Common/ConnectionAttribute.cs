using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using DataWings.Common.Oracle;
using DataWings.Common.Sql;

namespace DataWings.Common
{
    /// <summary>
    /// Specifies the method by which the connection string
    /// should be obtained
    /// </summary>
    public enum ConnectionAccessor
    {
        /// <summary>
        /// The connection string is registered in the connectionStrings
        /// section of the configuration file
        /// </summary>
        FromConfigFile,

        /// <summary>
        /// The connection string is read from a named file.
        /// </summary>
        FromNamedFile
    }


    /// <summary>
    /// Abstract base class for connection attributes. Provides all relevant "house hold"
    /// code, so that inheritors only have to concentrate on implementing the core 
    /// functionality: GetConnectionString()
    /// </summary>
    public abstract class AbstractConnectionAttribute : Attribute
    {
        private readonly Dictionary<SqlVendor, Func<string, ISqlProvider>> executorConstructors =
            new Dictionary<SqlVendor, Func<string, ISqlProvider>>
                {
                    {SqlVendor.Oracle, connString => new OracleProvider(connString)},
                    {SqlVendor.SqlServer, connString => new SqlServerProvider(connString)},
                    {SqlVendor.Provisioned, ProvisionedProvider.GetProvider}};

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractConnectionAttribute"/> class.
        /// </summary>
        /// <param name="vendor">The vendor.</param>
        protected AbstractConnectionAttribute(SqlVendor vendor)
        {
            Vendor = vendor;
        }

        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the alternative connection. This connection string
        /// will be used if GetConnectionString() fails.
        /// </summary>
        /// <value>The alternative connection.</value>
        public string AlternativeConnection { get; set; }

        /// <summary>
        /// Gets or sets the vendor.
        /// </summary>
        /// <value>The vendor.</value>
        public SqlVendor Vendor { get; set; }

        /// <summary>
        /// Gets the executor. The exact type of this executor will be
        /// determined by the Vendor property, and executor will be
        /// constructed with the connection string (either the one
        /// returned by the GetConnectionString() or the AlternativeConnection).
        /// 
        /// Is declared as virtual because it is practical to override
        /// this method in some test scenarios. Normal subclasses should
        /// not override this method.
        /// </summary>
        /// <returns></returns>
        public virtual ISqlProvider GetExecutor()
        {
            string connectionString;
            try
            {
                connectionString = GetConnectionString();
            }
            catch (Exception)
            {
                if (!String.IsNullOrEmpty(AlternativeConnection))
                {
                    connectionString = AlternativeConnection;
                }
                else
                {
                    throw;
                }
            }
            return executorConstructors[Vendor].Invoke(connectionString);
        }

        /// <summary>
        /// Must be overridden by subclasses. Returns the connection string
        /// to use. Implementations should throw an exception if the connection
        /// string can not be determined for some reason, because this abstract
        /// base class contains functionality for handling such cases by using
        /// the AlternativeConnection instead.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetConnectionString();


    }

    /// <summary>
    /// Fetches the connection string from the connectionStrings section of the
    /// configuration file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class ConnectionFromConfigFile : AbstractConnectionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionFromConfigFile"/> class.
        /// </summary>
        /// <param name="vendor">The vendor.</param>
        public ConnectionFromConfigFile(SqlVendor vendor)
            : base(vendor)
        {
        }

        /// <summary>
        /// The name of the connection string to be used from the
        /// connectionStrings section of the configuration file.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Must be overridden by subclasses. Returns the connection string
        /// to use. Fetches the connection string from the connectionStrings 
        /// section of the configuration file.
        /// 
        /// This connection string is from the element with name exactly
        /// matching the Key, so if Key is not set an exception will be
        /// raised here.
        /// </summary>
        /// <returns></returns>
        protected override string GetConnectionString()
        {
            if (String.IsNullOrEmpty(Key))
                throw new InvalidOperationException("ConnectionFromConfigFile can not be used when Key is not set");

            return ConfigurationManager.ConnectionStrings[Key].ConnectionString;
        }
    }

    /// <summary>
    /// Must be constructed with the connection string hardcoded
    /// in via constructor.
    /// </summary>
    public class ConnectionAttribute : AbstractConnectionAttribute
    {
        private string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionAttribute"/> class.
        /// </summary>
        /// <param name="vendor">The vendor.</param>
        /// <param name="connectionString">The connection string.</param>
        public ConnectionAttribute(SqlVendor vendor, string connectionString) : base(vendor)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Must be overridden by subclasses. Returns the connection string
        /// to use. Implementations should throw an exception if the connection
        /// string can not be determined for some reason, because this abstract
        /// base class contains functionality for handling such cases by using
        /// the AlternativeConnection instead.
        /// </summary>
        /// <returns></returns>
        protected override string GetConnectionString()
        {
            return connectionString;
        }
    }

    /// <summary>
    /// Fetches the connection string from a named file. The file is assumed to contain
    /// the connection string in the first line of text
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class ConnectionFromFile : AbstractConnectionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionFromFile"/> class.
        /// </summary>
        /// <param name="vendor">The vendor.</param>
        public ConnectionFromFile(SqlVendor vendor)
            : base(vendor)
        {
        }

        /// <summary>
        /// Gets or sets the file path of the file containing the connection string.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        /// <summary>
        /// Must be overridden by subclasses. Fetches the connection string from a 
        /// named file. The file is assumed to contain the connection string in the 
        /// first line of text
        /// </summary>
        /// to use. 
        /// </summary>
        /// <returns></returns>
        protected override string GetConnectionString()
        {
            if (FilePath == null)
                throw new InvalidOperationException("ConnectionFromFile can not be used when FilePath is not set");
            if (!File.Exists(FilePath))
                throw new FileNotFoundException(String.Format("{0} not found", FilePath));

            using (var reader = new StreamReader(FilePath))
            {
                return reader.ReadLine();
            }
        }
    }
}
