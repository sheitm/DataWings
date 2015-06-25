using System;
using DataWings.Assertions.Conventions;
using DataWings.Tests.Stubs;
using NUnit.Framework;

namespace DataWings.Tests
{
    [TestFixture]
    public class DbIdConventionAttributeTests
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Id convention not set.")]
        public void GetId_ConventionIsNull_ThrowsInvalidOperationException()
        {
            var attrib = new DbIdConventionAttribute(null);
            attrib.GetId(DateTime.Now);
        }

        [Test]
        public void GetId_FixedConvention_GetsIdAsExpected()
        {
            var attrib = new DbIdConventionAttribute("Id");
            Guid id = Guid.NewGuid();
            var entity = new Entity {Id = id};
            Assert.AreEqual(id, attrib.GetId(entity));
        }

        [Test]
        public void GetId_DynamicConvention__GetsIdAsExpected()
        {
            var attrib = new DbIdConventionAttribute("Id{0}");
            Guid id = Guid.NewGuid();
            var entity = new Entity { IdEntity = id };
            Assert.AreEqual(id, attrib.GetId(entity));
        }


    }
}
