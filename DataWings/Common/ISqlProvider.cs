using System.Collections.Generic;

namespace DataWings.Common
{
    /// <summary>
    /// Specifies how the select against the underlying database should be performed.
    /// </summary>
    public enum SelectOptions
    {
        /// <summary>
        /// Return the first row (if any)
        /// </summary>
        Single,

        /// <summary>
        /// Return all available rows
        /// </summary>
        All
    }

    /// <summary>
    /// The type of the underlying database
    /// </summary>
    public enum SqlVendor
    {
        /// <summary>
        /// Not set yet
        /// </summary>
        NotSet,

        /// <summary>
        /// Microsoft SQL Server
        /// </summary>
        SqlServer,

        /// <summary>
        /// Oracle
        /// </summary>
        Oracle,

        /// <summary>
        /// The provider must be set from the outside.
        /// Thus, you can extend DataWings with your
        /// own provider and so support database engines
        /// that are not natively supported by ThidHand
        /// </summary>
        Provisioned
    }

    /// <summary>
    /// Defines the interface to be implemented by any provider in direct contact
    /// with the underlying database
    /// </summary>
    public interface ISqlProvider
    {
        /// <summary>
        /// Executes the select and returns the list of rows
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="selectOptions">The select options.</param>
        /// <returns>The list of rows</returns>
        IList<ISqlResult> ExecuteQuery(string sql, SelectOptions selectOptions);

        /// <summary>
        /// Executes a query that does not return a result. Typically an
        /// update, insert or delete.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        void ExecuteNonQuery(string sql);

        /// <summary>
        /// Gets the connection string used by the inner connection.
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; }

        SqlVendor Vendor { get; }
    }
}