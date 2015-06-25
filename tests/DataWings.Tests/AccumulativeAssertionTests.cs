using System.Collections.Generic;
using NUnit.Framework;
using DataWings.Assertions;
using DataWings.Common;
using DataWings.Tests.Stubs;

namespace DataWings.Tests
{
    [TestFixture]
    public class AccumulativeAssertionTests
    {
        [SetUp]
        public void SetUp()
        {
            StubSqlProvider.Clear();
        }

        [Test]
        [TestConnectionAttribute(SqlVendor.Oracle)]
        public void Exists_HappyDays_SendsExpectedSqlToSqlExecutor()
        {
            decimal returnObject = 1;
            
            StubSqlProvider.Current.SetObjectToReturn(returnObject);
            
            var assertion = new AccumulativeAssertion(StubSqlProvider.Current);
            assertion.AddWhereColumn("FirstName", "Petter");

            assertion.Exists();

            string expectedSql = "SELECT count(*) FROM  WHERE FirstName = 'Petter'";
            Assert.AreEqual(expectedSql, StubSqlProvider.Current.Sql);
        }

        [Test]
        public void AreEqual_HappyDays_WorksAsExpected()
        {
            var valueMap = new Dictionary<string, object> { { "LastName", "Hansen" } };
            StubSqlProvider.Current.SetValueMap(valueMap);

            var assertion = new AccumulativeAssertion(StubSqlProvider.Current);
            assertion.AddWhereColumn("Id", 1111);

            assertion.AreEqual("LastName", "Hansen");
        }
    }
}