using System;
using System.Reflection;
using DataWings.Assertions.Conventions;
using DataWings.Common;

namespace DataWings.Assertions
{
    /// <summary>
    /// Static gateway into the assertion functionality.
    /// </summary>
    public static class DbAssert
    {
        #region Public API

        /// <summary>
        /// Returns an assertion with the name of the connection specificaton
        /// set == connectionName.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static DbAssertion ForConnection(string connectionName)
        {
            return new DbAssertion(connectionName);
        }

        /// <summary>
        /// Initializes and returns an accumulative assertion where the name of the
        /// database table which the select will go against is set.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>An accumulative assertion ready for further build up</returns>
        public static IAccumulativeAssertion ForTable(string tableName)
        {
            return new DbAssertion().ForTable(tableName);
        }

        /// <summary>
        /// Initializes and returns an executable asserion based on the given
        /// sql. This sql will be executed as is, so no further accumulative 
        /// build up is available here.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>The executable assertion</returns>
        public static IExecutableAssertion WithSql(string sql)
        {
            var assertion = new SqlBasedAccumulativeAssertion(ConnectionExecutorFinder.GetSqlExecutor(null));
            assertion.SetSql(sql);
            return assertion;
        }

        #region Conventional tests

        public static void ExistsInDatabase(this object entity)
        {
            Exists(entity);
        }

        /// <summary>
        /// Checks that the entity is stored in the database as
        /// expected according to the conventions registered for
        /// the test.
        /// </summary>
        /// <param name="entity"></param>
        public static void Exists(object entity)
        {
            var finder = new ConventionFinder();
            ForTable(finder.GetTableName(entity))
                .WithColumnValuePair(finder.GetIdProperty(entity), finder.GetId(entity))
                .Exists();
        }

        /// <summary>
        /// Checks that the entity is not stored in the database
        /// according to the conventions registered for the test. 
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void NotExists(object entity)
        {
            var finder = new ConventionFinder();
            ForTable(finder.GetTableName(entity))
                .WithColumnValuePair(finder.GetIdProperty(entity), finder.GetId(entity))
                .NotExists();
        }

        /// <summary>
        /// Checks that the value for the named property of the entity
        /// equals the corresponding value stored in the database
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">Name of the property.</param>
        public static void ColumnEquals(object entity, string propertyName)
        {
            var method = entity.GetType()
                .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                .GetGetMethod();
            object valInObject = method.Invoke(entity, new object[0]);

            var finder = new ConventionFinder();
            ForTable(finder.GetTableName(entity))
                .WithColumnValuePair(finder.GetIdProperty(entity), finder.GetId(entity))
                .AreEqual(propertyName, valInObject);
        }

        public static void Evaluate(object entity, Func<ISqlResult, bool> evaluationFunction)
        {
            var finder = new ConventionFinder();
            ForTable(finder.GetTableName(entity))
                .WithColumnValuePair(finder.GetIdProperty(entity), finder.GetId(entity))
                .Evaluate(evaluationFunction);
        }

        #endregion

        #endregion
    }
}