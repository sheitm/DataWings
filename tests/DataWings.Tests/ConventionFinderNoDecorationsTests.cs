using System;
using DataWings.Assertions.Conventions;
using DataWings.Tests.Stubs;
using NUnit.Framework;

namespace DataWings.Tests
{
    [TestFixture]
    public class ConventionFinderNoDecorationsTests
    {
        [Test]
        public void GetTableName_NoDecorations_TableNameEqualsClassName()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual(finder.GetTableName<DateTime>(), "DateTime");
        }

        [Test]
        public void GetId_NoDecorations_GetsIdAsExpected()
        {
            Guid id = Guid.NewGuid();
            var entity = new Entity {IdEntity = id};
            var finder = new ConventionFinder();
            Assert.AreEqual(id, finder.GetId(entity));
        }
    }
}
