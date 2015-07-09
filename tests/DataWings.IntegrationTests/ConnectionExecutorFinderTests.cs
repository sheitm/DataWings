using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DataWings.Common;
using NUnit.Framework;

namespace DataWings.IntegrationTests
{
    [TestFixture]
    public class ConnectionExecutorFinderTests
    {
        [Test]
        [ConnectionFromConfigFile(SqlVendor.Oracle, Key = "CAB", Name = "CAB")]
        public void GetSqlExecutor()
        {
            var provider = ConnectionExecutorFinder.GetSqlExecutor("CAB");

            ////var mi = typeof (ConnectionExecutorFinderTests).GetMethod("GetSqlExecutor",
            ////    BindingFlags.Instance | BindingFlags.Public);
            //var mi = typeof(ConnectionExecutorFinderTests).GetMethod("GetSqlExecutor");

            //var attrib = Attribute.GetCustomAttributes(mi, typeof(AbstractConnectionAttribute));
        }
    }
}
