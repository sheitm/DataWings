using System;
using NUnit.Framework;
using Rhino.Mocks;
using DataWings.Common;
using DataWings.Common.Sql;

namespace DataWings.Tests
{
    [TestFixture]
    public class ProvisionedProviderTests
    {
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            ProvisionedProvider.Reset();
            mocks = new MockRepository();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Factory not set.")]
        public void GetProvider_FactoryNotSet_ThrowsInvalidOperationException()
        {
            ProvisionedProvider.GetProvider("");
        }

        [Test]
        public void GetProvider_HappyDays_ReturnsProvider()
        {
            var factory = mocks.Stub<ISqlProviderFactory>();
            var provider = mocks.Stub<ISqlProvider>();
            string connectionString = "FakeConnectionString";
            using (mocks.Record())
            {
                factory.CreateProvider(connectionString);
                LastCall.Return(provider);
                ProvisionedProvider.SetFactory(factory);
            }
            var p = ProvisionedProvider.GetProvider(connectionString);
            Assert.AreSame(provider, p);
        }
    }
}
