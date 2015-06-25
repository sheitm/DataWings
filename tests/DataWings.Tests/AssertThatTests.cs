using System;
using NUnit.Framework;
using DataWings.Assertions;
using AssertionException=DataWings.Assertions.AssertionException;

namespace DataWings.Tests
{
    [TestFixture]
    public class AssertThatTests
    {
        [Test]
        public void IsTrue_True_NothingHappens()
        {
            AssertThat.IsTrue(true);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsTrue_False_ThrowsAssertionExcpetion()
        {
            AssertThat.IsTrue(false);
        }

        [Test]
        public void IsNotNull_NotNull_NothingHappens()
        {
            AssertThat.IsNotNull(new object());
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsNotNull_Null_ThrowsAssertionException()
        {
            AssertThat.IsNotNull(null);
        }

        [Test]
        public void AreEqual_TwoNulls_NothingHappens()
        {
            AssertThat.AreEqual(null, null);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreEqual_DifferentTypes_ThrowsAssertionException()
        {
            AssertThat.AreEqual("string", DateTime.Now);
        }

        [Test]
        public void AreEqual_TwoEqualStrings_NothingHappens()
        {
            AssertThat.AreEqual("s", "s");
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreEqual_TwoNonEqualStrings_ThrowsAssertionException()
        {
            AssertThat.AreEqual("s", "s1");
        }

        [Test]
        public void AreEqual_TwoEqualDateTimes_NothingHappens()
        {
            AssertThat.AreEqual(new DateTime(1999, 12, 12, 12, 12, 12, 12), new DateTime(1999, 12, 12, 12, 12, 12, 12));
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreEqual_TwoNonEqualDateTimes_ThrowsAssertionException()
        {
            AssertThat.AreEqual(new DateTime(1999, 12, 12, 12, 12, 12, 12), new DateTime(1999, 11, 12, 12, 12, 12, 12));
        }

        [Test]
        public void AreEqual_TwoLogicallyEqualDateTimes_NothingHappens()
        {
            var d1 = new DateTime(2009, 5, 19, 9, 18, 50, 99);
            var d2 = new DateTime(2009, 5, 19, 9, 18, 50, 11);
            AssertThat.AreEqual(d1, d2);
        }

        [Test]
        public void AreEqual_EquivalentIntAndLong_NothingHappens()
        {
            int i = 99;
            long l = 99;
            AssertThat.AreEqual(i, l);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void AreEqual_NotEquivalentIntAndLong_ThrowsAssertionException()
        {
            int i = 99;
            long l = 98;
            AssertThat.AreEqual(i, l);
        }
    }
}
