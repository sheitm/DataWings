using System;
using NUnit.Framework;
using DataWings.Common;
using DataWings.Common.Oracle;
using DataWings.Common.Sql;

namespace DataWings.Tests
{
    [TestFixture]
    [ConnectionFromConfigFile(SqlVendor.Oracle, Name="OnClass", Key="K1")]
    public class ConnectionExecutorFinderTests
    {
        [Test]
        public void GetExecutorByWalkingTheStack_NonNamed_FindsTheClassConnection()
        {
            var finder = new ConnectionExecutorFinder(null);
            var executor = finder.GetExecutorByWalkingTheStack();
            Assert.IsTrue(executor is OracleProvider);
            Assert.AreEqual("K1", executor.ConnectionString);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Unable to locate connection string attribute")]
        public void GetExecutorByWalkingTheStack_NonExistingName_ThrowsInvalidOperationException()
        {
            var finder = new ConnectionExecutorFinder("Non_Existing_Name");
            var executor = finder.GetExecutorByWalkingTheStack();
            Console.WriteLine(executor);
        }

        [Test]
        [ConnectionFromConfigFile(SqlVendor.Oracle, Name="OnMethod", Key = "K2")]
        public void GetExecutorByWalkingTheStack_NamedFromClass_IgnoresMethodAndGetsFromClass()
        {
            var finder = new ConnectionExecutorFinder("OnClass");
            var executor = finder.GetExecutorByWalkingTheStack();
            Assert.IsTrue(executor is OracleProvider);
            Assert.AreEqual("K1", executor.ConnectionString);
        }

        [Test]
        [ConnectionFromConfigFile(SqlVendor.SqlServer, Name = "OnMethod2", Key = "K3")]
        public void GetExecutorByWalkingTheStack_NotNamedInFinder_FindsDirectlyOnMethod()
        {
            var finder = new ConnectionExecutorFinder(null);
            var executor = finder.GetExecutorByWalkingTheStack();
            Assert.IsTrue(executor is SqlServerProvider);
            Assert.AreEqual("K3", executor.ConnectionString);
        }
    }
}
