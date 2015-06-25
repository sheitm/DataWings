using System;
using System.Collections.Generic;
using DataWings.Assertions;
using DataWings.Common;
using DataWings.Tests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;
using AssertionException=DataWings.Assertions.AssertionException;

namespace DataWings.Tests
{
    [TestFixture]
    [Connection(SqlVendor.Provisioned, "Not important")]
    public class DbAssertTests
    {
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            ConnectionExecutorFinder.Reset();
        }

        [Test]
        public void Exists_ForExistingValue_NothingHappens()
        {
            string columnName = "Surname";
            string columnValue = "Obama";
            SetUpExistsTest(1);

            DbAssert.ForTable("Person").WithColumnValuePair(columnName, columnValue).Exists();
        }

        [Test]
        [ExpectedException(typeof(AssertionException), ExpectedMessage = "No row with column values [Surname : 'Obama'] in table Person exists.")]
        public void Exists_ForNonExistingValue_ThrowsAssertionException()
        {
            string columnName = "Surname";
            string columnValue = "Obama";
            SetUpExistsTest(0);

            DbAssert.ForTable("Person").WithColumnValuePair(columnName, columnValue).Exists();
        }

        [Test]
        public void NotExists_ForNonExistingValue_NothingHappens()
        {
            string columnName = "Surname";
            string columnValue = "Obama";
            SetUpExistsTest(0);

            DbAssert.ForTable("Person").WithColumnValuePair(columnName, columnValue).NotExists();
        }

        [Test]
        [ExpectedException(typeof(AssertionException), ExpectedMessage = "Row with column value [Surname : 'Obama'] in table Person expected not to exist, but exists.")]
        public void NotExists_ForExistingValue_ThrowsAssertionException()
        {
            string columnName = "Surname";
            string columnValue = "Obama";
            SetUpExistsTest(1);

            DbAssert.ForTable("Person").WithColumnValuePair(columnName, columnValue).NotExists();
        }

        [Test]
        public void AreEqual_ForEqualValues_NothingHappens()
        {
            string firstNameColumn = "firstName";
            string firstName = "Barack";

            SetUpAreEqualTest(firstNameColumn, firstName);

            DbAssert.ForTable("Person").WithColumnValuePair("x", "y").AreEqual(firstNameColumn, firstName);
        }

        [Test]
        [ExpectedException(typeof(AssertionException), ExpectedMessage = "Expected Barack but got SomeOtherFirstName.")]
        public void AreEqual_ForNonEqualValues_ThrowsAssertionException()
        {
            string firstNameColumn = "firstName";
            string firstName = "Barack";

            SetUpAreEqualTest(firstNameColumn, "SomeOtherFirstName");

            DbAssert.ForTable("Person").WithColumnValuePair("x", "y").AreEqual(firstNameColumn, firstName);
        }

        [Test]
        public void Evaluate_IsTrue_NothingHappens()
        {
            SetUpEvaluateTest();

            DbAssert.ForTable("Person").WithColumnValuePair("x", "y").Evaluate(x => true);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void Evaluate_IsFalse_ThrowsAssertionException()
        {
            SetUpEvaluateTest();

            DbAssert.ForTable("Person").WithColumnValuePair("x", "y").Evaluate(x => false);
        }

        [Test]
        public void ConventionalExists_ExistsInDb_SendsCountSqlToSqlProvider()
        {
            Guid id = Guid.NewGuid();

            SetUpConventionalExistsTest(id, 1);
            
            var entity = new Entity {IdEntity = id};
            DbAssert.Exists(entity);

            mocks.VerifyAll();
        }

        [Test]
        public void AssertExistsInDatabase_ExistsInDb_SendsCountSqlToSqlProvider()
        {
            Guid id = Guid.NewGuid();

            SetUpConventionalExistsTest(id, 1);

            var entity = new Entity { IdEntity = id };
            entity.AssertExistsInDatabase();

            mocks.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void ConventionalExists_DoesNotExistInDb_ThrowsAssertionException()
        {
            Guid id = Guid.NewGuid();

            SetUpConventionalExistsTest(id, 0);

            var entity = new Entity { IdEntity = id };
            DbAssert.Exists(entity);

            mocks.VerifyAll();
        }

        [Test]
        public void ConventionalNotExists_DoesNotExistInDb_SendsCountSqlToSqlProvider()
        {
            Guid id = Guid.NewGuid();

            SetUpConventionalExistsTest(id, 0);

            var entity = new Entity { IdEntity = id };
            DbAssert.NotExists(entity);

            mocks.VerifyAll();
        }

        [Test]
        public void AssertNotExistsInDatabase_DoesNotExistInDb_SendsCountSqlToSqlProvider()
        {
            Guid id = Guid.NewGuid();

            SetUpConventionalExistsTest(id, 0);

            var entity = new Entity { IdEntity = id };
            entity.AssertNotExistsInDatabase();

            mocks.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void ConventionalNotExists_ExistsInDb_ThrowsAssertionException()
        {
            Guid id = Guid.NewGuid();

            SetUpConventionalExistsTest(id, 1);

            var entity = new Entity { IdEntity = id };
            DbAssert.NotExists(entity);

            mocks.VerifyAll();
        }

        #region Test Support

        private void SetUpConventionalExistsTest(Guid id, decimal countSqlReturn)
        {
            var factory = mocks.Stub<ISqlProviderFactory>();
            ProvisionedProvider.SetFactory(factory);
            var provider = mocks.DynamicMock<ISqlProvider>();
            string expectedSql = String.Format("SELECT count(*) FROM Entity WHERE IdEntity = '{0}'", id);
            using (mocks.Record())
            {
                factory.CreateProvider("some string");
                LastCall.Return(provider).IgnoreArguments();

                var res = new StubSqlResult() { ReturnObject = countSqlReturn };
                var resList = new List<ISqlResult> { res };

                provider.ExecuteQuery(expectedSql, SelectOptions.Single);
                LastCall.Return(resList);
            }
        }

        private void SetUpEvaluateTest()
        {
            var factory = mocks.Stub<ISqlProviderFactory>();
            ProvisionedProvider.SetFactory(factory);
            var provider = mocks.Stub<ISqlProvider>();
            var sqlResult = mocks.Stub<ISqlResult>();
            var resultList = new List<ISqlResult> { sqlResult };
            using (mocks.Record())
            {
                factory.CreateProvider("some string");
                LastCall.Return(provider).IgnoreArguments();

                provider.ExecuteQuery("som sql", SelectOptions.Single);
                LastCall.Return(resultList).IgnoreArguments();
            }
        }

        private void SetUpAreEqualTest(string checkColumn, string checkColumnValue)
        {
            var factory = mocks.Stub<ISqlProviderFactory>();
            ProvisionedProvider.SetFactory(factory);
            var provider = mocks.Stub<ISqlProvider>();
            var sqlResult = mocks.Stub<ISqlResult>();
            var resultList = new List<ISqlResult> { sqlResult };

            using (mocks.Record())
            {
                factory.CreateProvider("some string");
                LastCall.Return(provider).IgnoreArguments();

                provider.ExecuteQuery("som sql", SelectOptions.Single);
                LastCall.Return(resultList).IgnoreArguments();

                sqlResult.GetResult(checkColumn);
                LastCall.Return(checkColumnValue);
            }
        }

        private void SetUpExistsTest(decimal countResult)
        {
            var factory = mocks.Stub<ISqlProviderFactory>();
            ProvisionedProvider.SetFactory(factory);
            var provider = mocks.Stub<ISqlProvider>();
            var sqlResult = mocks.Stub<ISqlResult>();
            var resultList = new List<ISqlResult> { sqlResult };

            using (mocks.Record())
            {
                factory.CreateProvider("some string");
                LastCall.Return(provider).IgnoreArguments();

                provider.ExecuteQuery("som sql", SelectOptions.Single);
                LastCall.Return(resultList).IgnoreArguments();

                //sqlResult.GetSingleResult<decimal>();
                sqlResult.GetSingleResult();
                LastCall.Return(countResult);
            }
        }

        #endregion
    }
}
