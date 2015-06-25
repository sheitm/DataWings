using System;
using DataWings.Assertions.Conventions;
using DataWings.Tests.Stubs;
using NUnit.Framework;

namespace DataWings.Tests
{
    [TestFixture]
    [DbTableNameConvention(DbTableNameConventionType.Custom, Convention = "tbl_{0}")]
    [DbTableNameConvention(DbTableNameConventionType.Custom, Convention = "{0}XXX", EntityType = typeof(Entity))]
    [DbIdConvention("Id")]
    public class ConventionFinderTests
    {
        [Test]
        public void GetTableName_CustomConventionRegisteredOnClass_GetsExpectedTableName()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("tbl_DateTime", finder.GetTableName<DateTime>());
        }

        [Test]
        public void GetTableName_CustomConventionNamedRegisteredOnClass_GetsExpectedTableName()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("EntityXXX", finder.GetTableName<Entity>());
        }

        [Test]
        [DbTableNameConvention(DbTableNameConventionType.Custom, Convention = "METHOD_{0}")]
        public void GetTableName_CustomConventionNamedRegisteredOnMethod_GetsExpectedTableName()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("METHOD_DateTime", finder.GetTableName<DateTime>());
        }

        [Test]
        [DbTableNameConvention(DbTableNameConventionType.Custom, Convention = "METHOD_{0}")]
        public void GetTableName_CustomConventionNamedRegisteredOnClassAndAttribOnMethod_GetsExpectedTableName()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("EntityXXX", finder.GetTableName<Entity>());
        }

        [Test]
        [DbTableNameConvention(DbTableNameConventionType.Custom, Convention = "{0}YYY", EntityType = typeof(Entity))]
        public void GetTableName_CustomConventionNamedRegisteredOnClassAndNamedAttribOnMethod_GetsExpectedTableName()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("EntityYYY", finder.GetTableName<Entity>());
        }

        [Test]
        public void GetId_FixedIdConventionRegisteredOnClass_GetsCorrectId()
        {
            Guid id = Guid.NewGuid();
            var entity = new Entity {Id = id};
            var finder = new ConventionFinder();
            Assert.AreEqual(id, finder.GetId(entity));
        }
    }
}
