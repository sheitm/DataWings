using DataWings.Assertions;
using DataWings.Common;
using DataWings.DataMaintenance;
using NUnit.Framework;
using AssertionException=DataWings.Assertions.AssertionException;

namespace DataWings.IntegrationTests
{
    [TestFixture]
    [Connection(SqlVendor.Provisioned, Database.CONNECTION_STRING)]
    public class DbAssertTests : IntegrationTestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        [ExpectedException(typeof(AssertionException), ExpectedMessage = "No row with column values [IdPerson : 12] in table Person exists.")]
        public void Exists_DoesNotExistInDatabase_ThrowsAssertionException()
        {
            DbAssert.ForTable("Person")
                .WithColumnValuePair("IdPerson", 12)
                .Exists();
        }

        [Test]
        public void Exists_ExistsInDatabase_NoExceptionThrown()
        {
            string lastName = "Stoltenberg";
            string firstName = "Jens";
            int id = InsertPerson(lastName, firstName);

            DbAssert.ForTable("Person")
                .WithColumnValuePair("IdPerson", id)
                .Exists();
        }

        [Test]
        public void NotExists_DoesNotExistInDatabase_NoExceptionThrown()
        {
            DbAssert.ForTable("Person")
               .WithColumnValuePair("IdPerson", NextId())
               .NotExists();
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void NotExists_ExistsInDatabase_ThrowsAssertionException()
        {
            string lastName = "Bulgakov";
            string firstName = "Mikhail";
            int id = InsertPerson(lastName, firstName);

            DbAssert.ForTable("Person")
              .WithColumnValuePair("IdPerson", id)
              .NotExists();
        }

        [Test]
        public void AreEqual_ShouldBeAble_NoExceptionThrown()
        {
            string lastName = "Bulgakov";
            string firstName = "Mikhail";
            int id = InsertPerson(lastName, firstName);

            DbAssert.ForTable("Person")
                .WithColumnValuePair("IdPerson", id)
                .AreEqual("LastName", lastName);
        }

        #region Private helper methods

        private int InsertPerson(string lastName, string firstName)
        {
            int id = NextId();

            DataBoy.ForTable("Person")
                .Row("IdPerson", id)
                .D("FirstName", firstName)
                .D("LastName", lastName)
                .Commit();

            return id;
        }

        #endregion

    }
}
