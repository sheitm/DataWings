using System;
using System.Text;
using DataWings.Assertions.Conventions;
using NUnit.Framework;

namespace DataWings.IntegrationTests
{
    [TestFixture]
    [DbTableNameConvention(DbTableNameConventionType.ClassNameEqualsTableName)]
    [DbTableNameConvention(DbTableNameConventionType.Custom, Convention = "TBL_{0}")]
    public class ConventionFinderTests
    {
        [Test]
        public void GetTableName_DefaultDecorationFromClass_TableNameAccordingToConvention()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("TBL_DateTime", finder.GetTableName<DateTime>());
        }

        [Test]
        [DbTableNameConvention(DbTableNameConventionType.ClassNameEqualsTableName)]
        public void GetTableName_DecorationOnBothMethodAndClass_TableNameAccordingMethodDecoration()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("DateTime", finder.GetTableName<DateTime>());
        }

        [Test]
        [DbTableNameConvention(DbTableNameConventionType.ClassNameEqualsTableName)]
        public void GetTableName_ShouldUseNamedDecoration_TableNameAccordingToNamedDecoration()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("TStringBuilder", finder.GetTableName<StringBuilder>());
        }

        [Test]
        [DbTableNameConvention(DbTableNameConventionType.Custom, Convention = "XX{0}", EntityType = typeof(StringBuilder))]
        public void GetTableName_NamedDecorationOnBothMethodAndClass_UsesDecorationFromMethod()
        {
            var finder = new ConventionFinder();
            Assert.AreEqual("XXStringBuilder", finder.GetTableName<StringBuilder>());
        }
    }
}
