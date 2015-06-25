using System;
using DataWings.Common;
using DataWings.DataMaintenance;
using DataWings.Tests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace DataWings.Tests
{
    [TestFixture]
    [Connection(SqlVendor.Provisioned, "Not important")]
    public class DataBoyTests
    {
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            ConnectionExecutorFinder.Reset();
        }

        [Test]
        public void ExecuteNonQuery_HappyDays_QuerySentThroughToSqlProvider()
        {
            string sqlQuery = "NonQuery";
            var factory = GetProviderFactory();
            var provider = mocks.DynamicMock<ISqlProvider>();
            using (mocks.Record())
            {
                factory.CreateProvider("some string");
                LastCall.Return(provider).IgnoreArguments();

                provider.ExecuteNonQuery(sqlQuery);
            }

            DataBoy.ExecuteNonQuery(sqlQuery);

            mocks.VerifyAll();
        }


        [Test]
        public void Values_WithMultipleRows_SendSqlAsExpected()
        {
            var factory = GetProviderFactory();
            var provider = new StubSqlProvider();
            using (mocks.Record())
            {
                factory.CreateProvider("somestring");
                LastCall.Return(provider).IgnoreArguments();
            }

            DataBoy.ForTable("Person")
                .Values(
                   Column.Named("FirstName").Eq("Steel"),
                   Column.Named("LastName").Eq("Hotman"));

            Assert.AreEqual("INSERT INTO Person (FIRSTNAME, LASTNAME) VALUES('Steel', 'Hotman')", provider.GetExecutedQuery(0));
        }

        [Test]
        public void Commit_MultipleRows_SendsSqlsInCorrectOrder()
        {
            var factory = GetProviderFactory();
            var provider = new StubSqlProvider();
            using (mocks.Record())
            {
                factory.CreateProvider("somestring");
                LastCall.Return(provider).IgnoreArguments();
            }

            DataBoy
                .ForTable("Person")
                    .Row("IdPerson", 1).Data("Surname", "Obama").DeleteFirst()
                    .Row("IdPerson", 2).Data("Surname", "Bush").DeleteFirst()
                .ForTable("Address")
                    .Row("IdAddress", 100).Data("Street", "Main street").DeleteFirst()
                .Commit();

            Assert.AreEqual("DELETE FROM Address WHERE IdAddress = 100", provider.GetExecutedQuery(0));
            Assert.AreEqual("DELETE FROM Person WHERE IdPerson = 2", provider.GetExecutedQuery(1));
            Assert.AreEqual("DELETE FROM Person WHERE IdPerson = 1", provider.GetExecutedQuery(2));
            Assert.AreEqual("INSERT INTO Person (IDPERSON, SURNAME) VALUES(1, 'Obama')", provider.GetExecutedQuery(3));
            Assert.AreEqual("INSERT INTO Person (IDPERSON, SURNAME) VALUES(2, 'Bush')", provider.GetExecutedQuery(4));
            Assert.AreEqual("INSERT INTO Address (IDADDRESS, STREET) VALUES(100, 'Main street')", provider.GetExecutedQuery(5));
        }

        [Test]
        public void Commit_ForUpdate_SendsUpdateInsteadOfInsert()
        {
            var factory = GetProviderFactory();
            var provider = new StubSqlProvider();
            using (mocks.Record())
            {
                factory.CreateProvider("somestring");
                LastCall.Return(provider).IgnoreArguments();
            }

            DataBoy.ForTable("Person").Row("IdPerson", 1).Data("Surname", "Obama").ForUpdate().Commit();

            Assert.IsTrue(provider.GetExecutedQuery(0).StartsWith("UPDATE"));
        }

        [Test]
        public void Commit_ForUpdate_UniqueKeyColumnNotIncludedInUpdateStatement()
        {
            var factory = GetProviderFactory();
            var provider = new StubSqlProvider();
            using (mocks.Record())
            {
                factory.CreateProvider("somestring");
                LastCall.Return(provider).IgnoreArguments();
            }

            string idColumenName = "IdPerson";
            DataBoy.ForTable("Person").Row(idColumenName, 1).Data("Surname", "Obama").ForUpdate().Commit();

            Assert.AreEqual("UPDATE Person SET SURNAME = 'Obama' WHERE IdPerson = 1", provider.GetExecutedQuery(0));
        }

        [Test]
        public void Commit_ForDelete_SendsDeleteInsteadOfInsert()
        {
            var factory = GetProviderFactory();
            var provider = new StubSqlProvider();
            using (mocks.Record())
            {
                factory.CreateProvider("somestring");
                LastCall.Return(provider).IgnoreArguments();
            }

            DataBoy.ForTable("Person").Row("IdPerson", 1).ForDelete().Commit();

            Assert.IsTrue(provider.GetExecutedQuery(0).StartsWith("DELETE"));
        }

        [Test]
        public void Commit_WithReturnValue_SelectExecutedAsExpected()
        {
            //INSERT INTO Address (ID, IdPerson) VALUES('7e7304db-2b77-4040-ae64-9eb6a686a036', 1)
            var factory = GetProviderFactory();
            var provider = new StubSqlProvider();
            provider.SetObjectToReturn(1);
            using (mocks.Record())
            {
                factory.CreateProvider("somestring");
                LastCall.Return(provider).IgnoreArguments();
            }

            Guid personId = Guid.NewGuid();
            DataBoy
                .ForTable("Person")
                    .Row("Id", personId)
                    .ReturnValue("IdPerson").AtKey("IdOfPerson")
                //.ForTable("Address")
                //    .Row("Id", Guid.NewGuid())
                //    .BindColumn("IdPerson").To("IdOfPerson")
                .Commit();

            //HasAnySqlMatching
            string expectedSql = string.Format("SELECT IdPerson FROM Person WHERE Id = '{0}'", personId);
            Assert.IsTrue(provider.HasAnySqlMatching(expectedSql));
        }

        [Test]
        public void Commit_BindColumnToForInsert_InsertWorksCorrectly()
        {
            var factory = GetProviderFactory();
            var provider = new StubSqlProvider();
            provider.SetObjectToReturn(1);
            using (mocks.Record())
            {
                factory.CreateProvider("somestring");
                LastCall.Return(provider).IgnoreArguments();
            }

            Guid personId = Guid.NewGuid();
            Guid addressId = Guid.NewGuid();
            DataBoy
                .ForTable("Person")
                    .Row("Id", personId)
                    .ReturnValue("IdPerson").AtKey("IdOfPerson")
                .ForTable("Address")
                    .Row("Id", addressId)
                    .BindColumn("IdPerson").To("IdOfPerson")
                .Commit();

            string expectedSql = string.Format("INSERT INTO Address (ID, IdPerson) VALUES('{0}', 1)", addressId);
            Assert.IsTrue(provider.HasAnySqlMatching(expectedSql));
        }

        private ISqlProviderFactory GetProviderFactory()
        {
            var factory = mocks.Stub<ISqlProviderFactory>();
            ProvisionedProvider.SetFactory(factory);
            return factory;
        }
    }
}
