using System;
using DataWings.Assertions;
using DataWings.Common;
using DataWings.DataAdversary;
using DataWings.DataMaintenance;
using NUnit.Framework;

namespace DataWings.IntegrationTests
{
    [TestFixture]
    [Connection(SqlVendor.Provisioned, Database.CONNECTION_STRING)]
    public class AdversaryTests : IntegrationTestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void IncRowVersion_HappyDays_IncreasesVersionInDatabase()
        {
            int id = NextId();
            int rowVersion = 1;

            DataBoy.ForTable("Person")
                .Row("IdPerson", id)
                .D("FirstName", "Petter")
                .D("LastName", "Hansen")
                .D("Version", rowVersion)
                .Commit();

            Adversary.ForTable("Person").IdentifiedBy("IdPerson", id).IncRowVersion("Version");

            DbAssert.ForTable("Person").WithColumnValuePair("IdPerson", id).AreEqual("Version", rowVersion + 1);
        }
    }
}
