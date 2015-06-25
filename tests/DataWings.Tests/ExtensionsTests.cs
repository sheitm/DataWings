using System;
using System.Collections.Generic;
using NUnit.Framework;
using DataWings;

namespace DbAssserions.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToColumnValuePairs_InputIsNull_ThrowsArgumentNullException()
        {
            Extensions.ToColumnValuePairs(null);
        }

        [Test]
        public void ToColumnValuePairs_HappyDays_WorksAsExpected()
        {
            string columnValuePairs = "FirstName='Hans';LastName ='Haslum';TullBall='c;c';Age=34";
            var keys = new List<string> {"FIRSTNAME", "LASTNAME", "TULLBALL", "AGE"};

            var dict = columnValuePairs.ToColumnValuePairs();

            Assert.AreEqual(keys.Count, dict.Count);
            foreach (var key in keys)
            {
                Assert.IsTrue(dict.ContainsKey(key), "Does not contain key: " + key);
            }

            Assert.AreEqual("'Hans'", dict["FIRSTNAME"]);
            Assert.AreEqual("'Haslum'", dict["LASTNAME"]);
            Assert.AreEqual("34", dict["AGE"]);
            Assert.AreEqual("'c;c'", dict["TULLBALL"]);
        }

        [Test]
        public void ToColumnValuePairs_TrailingSemiColon_WorksAsExpected()
        {
            string columnValuePairs = "FirstName='Hans';LastName ='Haslum';TullBall='c;c';Age=34;";
            var keys = new List<string> { "FIRSTNAME", "LASTNAME", "TULLBALL", "AGE" };

            var dict = columnValuePairs.ToColumnValuePairs();

            Assert.AreEqual(keys.Count, dict.Count);
            foreach (var key in keys)
            {
                Assert.IsTrue(dict.ContainsKey(key), "Does not contain key: " + key);
            }

            Assert.AreEqual("'Hans'", dict["FIRSTNAME"]);
            Assert.AreEqual("'Haslum'", dict["LASTNAME"]);
            Assert.AreEqual("34", dict["AGE"]);
            Assert.AreEqual("'c;c'", dict["TULLBALL"]);
        }

        [Test]
        public void ToColumnValuePairs_FunnyFormating_WorksAsExpected()
        {
            string columnValuePairs = "   FirstName='Hans'  ;   LastName =      'Haslum';TullBall=   'c;c';      Age=34";
            var keys = new List<string> { "FIRSTNAME", "LASTNAME", "TULLBALL", "AGE" };

            var dict = columnValuePairs.ToColumnValuePairs();

            Assert.AreEqual(keys.Count, dict.Count);
            foreach (var key in keys)
            {
                Assert.IsTrue(dict.ContainsKey(key), "Does not contain key: " + key);
            }

            Assert.AreEqual("'Hans'", dict["FIRSTNAME"]);
            Assert.AreEqual("'Haslum'", dict["LASTNAME"]);
            Assert.AreEqual("34", dict["AGE"]);
            Assert.AreEqual("'c;c'", dict["TULLBALL"]);
        }
    }
}
