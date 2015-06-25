using System;
using NUnit.Framework;
using DataWings.Common;
using DataWings.DataMaintenance;

namespace DataWings.IntegrationTests
{
    [TestFixture]
    [Connection(SqlVendor.Provisioned, Database.CONNECTION_STRING)]
    public class DataBoyTests : IntegrationTestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void InsertData_ForInsert_SendsExpectedSqlToDatabase()
        {
            int id = NextId();
            string expectedSql = String.Format("INSERT INTO Person (IDPERSON, FIRSTNAME, LASTNAME) VALUES({0}, 'Petter', 'Hansen')", id);
            
            DataBoy.ForTable("Person")
                .Row("IdPerson", id)
                .D("FirstName", "Petter")
                .D("LastName", "Hansen")
                .Commit();

            Assert.AreEqual(expectedSql, ExecutedSql);
        }

        [Test]
        public void InsertData_ForInsert_DataActuallyInsertedInDatabase()
        {
            int id = NextId();
            string firstName = "Jens";
            string lastName = "Stoltenberg";
            DataBoy.ForTable("Person")
               .Row("IdPerson", id)
               .D("FirstName", firstName)
               .D("LastName", lastName)
               .Commit();

            var results = Database.Select(String.Format("SELECT * FROM Person WHERE IdPerson = {0}", id),SelectOptions.Single);
            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void QueryForTable_HappyDays_GetsDataAsExpected()
        {
            int id = NextId();
            string firstName = "Reodor";
            string lastName = "Felgen";
            DataBoy.ForTable("Person")
               .Row("IdPerson", id)
               .D("FirstName", firstName)
               .D("LastName", lastName)
               .Commit();

            string selectedLastName = DataBoy.QueryForTable("Person")
                                        .Where("IdPerson").Eq(id)
                                        .GetValueForColumn<string>("LastName");
            Assert.AreEqual(lastName, selectedLastName);
        }

        [Test]
        public void UpdateData_HappyDays_DataIsUpdatedInDatabase()
        {
            int id = NextId();
            string firstName = "Jens";
            string lastName = "Stoltenberg";
            DataBoy.ForTable("Person")
               .Row("IdPerson", id)
               .D("FirstName", firstName)
               .D("LastName", lastName)
               .Commit();

            string newFirstName = "Thorvald";
            DataBoy.ForTable("Person")
                .Row("IdPerson", id).ForUpdate()
                .D("FirstName", newFirstName)
                .Commit();
            var resultingRow =
                (Database.Select(String.Format("SELECT * FROM Person WHERE IdPerson = {0}", id), SelectOptions.Single))[0];
            Assert.AreEqual(newFirstName, resultingRow.GetResult("FirstName"));
        }

        [Test]
        public void DeleteData_HappyDays_DataIsDeletedFromDatabase()
        {
            int id = NextId();
            string firstName = "Jens";
            string lastName = "Stoltenberg";
            DataBoy.ForTable("Person")
               .Row("IdPerson", id)
               .D("FirstName", firstName)
               .D("LastName", lastName)
               .Commit();

            DataBoy.ForTable("Person")
                .Row("IdPerson", id).ForDelete()
                .Commit();
            var results = Database.Select(String.Format("SELECT * FROM Person WHERE IdPerson = {0}", id), SelectOptions.Single);
            Assert.AreEqual(0, results.Count);
        }

        
    }
}
