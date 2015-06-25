using DataWings.Common;
using DataWings.DataAdversary;
using DataWings.Tests.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace DataWings.Tests
{
    [TestFixture]
    [Connection(SqlVendor.Provisioned, "Not important")]
    public class AdversaryTests
    {
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            ConnectionExecutorFinder.Reset();
        }

        [Test]
        public void IncRowVersion_HappyDays_SendsCorrectSqlToProvider()
        {
            var factory = GetProviderFactory();
            var provider = new StubSqlProvider();
            using (mocks.Record())
            {
                factory.CreateProvider("somestring");
                LastCall.Return(provider).IgnoreArguments();
            }

            Adversary.ForTable("Person").IdentifiedBy("IdPerson", 1).IncRowVersion("Version");

            Assert.AreEqual("UPDATE Person SET Version = Version + 1 WHERE IdPerson = 1", provider.GetExecutedQuery(0));
        }

        private ISqlProviderFactory GetProviderFactory()
        {
            var factory = mocks.Stub<ISqlProviderFactory>();
            ProvisionedProvider.SetFactory(factory);
            return factory;
        }
    }
}
