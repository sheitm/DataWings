using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DataWings.IntegrationTests
{
    [TestFixture]
    public class ExplorativeTests
    {
        [Test]
        public void HowDoesSQLiteWork_CreateTable()
        {
            Database.SetUp();

            string tableName = "MyFirstTable";
            string createTable = String.Format("CREATE TABLE {0}(Id{0} integer primary key, Date varcahr(50))", tableName);

            DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (var connection = fact.CreateConnection())
            {
                connection.ConnectionString = "Data Source=test.db3";
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = createTable;
                cmd.ExecuteNonQuery();

            }

            //using (SQLiteCommand mycommand = new SQLiteCommand(fact.CreateCommand()))
            //{
            //    int n;

            //    for (n = 0; n < 100000; n++)
            //    {
            //        mycommand.CommandText = String.Format("INSERT INTO [MyTable] ([MyId]) VALUES({0})", n + 1);
            //        mycommand.ExecuteNonQuery();
            //    }
            //}
        }
    }
}
