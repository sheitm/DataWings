using System;
using System.Collections.Generic;
using System.IO;
using DataWings.Common;

namespace DataWings.IntegrationTests
{
    internal static class Database
    {
        private const string DATABASE = "test.db3";
        public const string CONNECTION_STRING = "Data Source=test.db3";
        private const string CREATE_TABLE =
                "CREATE TABLE Person(IdPerson integer primary key, FirstName varchar(50), LastName varchar(50), Version integer)";

        private static SqlProvider provider;
        public static event EventHandler<SqlStatementEventArgs> SqlExecuted;

        public static void SetUp()
        {
            if (File.Exists(DATABASE))
            {
                File.Delete(DATABASE);
            }
            provider = SqlProvider.GetInstance(CONNECTION_STRING);
            provider.ExecuteNonQuery(CREATE_TABLE);
            ProvisionedProvider.SetFactory(provider);
        }

        public static IList<ISqlResult> Select(string sql, SelectOptions options)
        {
            return provider.ExecuteQuery(sql, options);
        }

        public static void ReceiveExecutedSql(string sql)
        {
            if (SqlExecuted != null)
            {
                SqlExecuted(null, new SqlStatementEventArgs{Sql = sql});
            }
        }
    }

    internal class SqlStatementEventArgs : EventArgs
    {
        public string Sql { get; set; }
    }
}
