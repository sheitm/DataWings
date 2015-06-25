using System;
using DataWings.Assertions.Conventions;
using NUnit.Framework;

namespace DataWings.Tests
{
    [TestFixture]
    public class DbTableNameConventionAttributeTests
    {
        [Test]
        public void Convention_SetToNull_IsSetToNull()
        {
            var attrib = new DbTableNameConventionAttribute(DbTableNameConventionType.Custom);
            attrib.Convention = null;
            Assert.IsNull(attrib.Convention);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Convention for DbTableNameConvention must contain '{0}'")]
        public void Convention_ValueNotOnCorrectType_ThrowArgumentException()
        {
            var attrib = new DbTableNameConventionAttribute(DbTableNameConventionType.Custom);
            attrib.Convention = "SomeConvention";
        }

        [Test]
        public void GetTableName_ConventionTypeIsClassNameEqualsTableNameConventionNotSet_GetsEntityTypeName()
        {
            var attrib = new DbTableNameConventionAttribute(DbTableNameConventionType.ClassNameEqualsTableName);
            var entity = DateTime.Now;
            Assert.AreEqual(typeof(DateTime).Name, attrib.GetTableName(entity.GetType()));
        }

        [Test]
        public void GetTableName_ConventionTypeIsClassNameEqualsTableNameConventionIsSet_GetsEntityTypeName()
        {
            var attrib = new DbTableNameConventionAttribute(DbTableNameConventionType.ClassNameEqualsTableName);
            attrib.Convention = "TBL_{0}";
            var entity = DateTime.Now;
            Assert.AreEqual(typeof(DateTime).Name, attrib.GetTableName(entity.GetType()));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Convention type is Custom, but convention is not set.")]
        public void GetTableName_ConventionTypeIsCustomAndConventionNotSet_ThrowsInvalidOperationException()
        {
            var attrib = new DbTableNameConventionAttribute(DbTableNameConventionType.Custom);
            attrib.GetTableName(typeof(DateTime));
        }

        [Test]
        public void GetTableName_ByCustomConvention_ReturnsExpectedTableName()
        {
            var attrib = new DbTableNameConventionAttribute(DbTableNameConventionType.Custom);
            attrib.Convention = "TBL_{0}";
            Assert.AreEqual("TBL_DateTime", attrib.GetTableName(typeof(DateTime)));
        }
    }
}
