using System;

namespace DataWings.Assertions
{
    /// <summary>
    /// Similar to the Assert class of System.Diagnostics or NUnit
    /// </summary>
    public static class AssertThat
    {
        /// <summary>
        /// Raises an AssertionException if check == false, else
        /// nothing happens
        /// </summary>
        /// <param name="check">if set to <c>true</c> [check].</param>
        public static void IsTrue(bool check)
        {
            IsTrue(check, "Assertion was false");
        }

        /// <summary>
        /// Raises an AssertionException with the given message if 
        /// check == false,else nothing happens
        /// </summary>
        /// <param name="check">if set to <c>true</c> [check].</param>
        /// <param name="failedAssertionMessage">The failed assertion message.</param>
        public static void IsTrue(bool check, string failedAssertionMessage)
        {
            if (!check)
                throw new AssertionException(failedAssertionMessage);
        }

        /// <summary>
        /// Raises an AssertionException if obj == null, else
        /// nothing happens
        /// </summary>
        /// <param name="obj">The obj.</param>
        public static void IsNotNull(object obj)
        {
            IsNotNull(obj, "Expected not null, but was null");
        }

        /// <summary>
        /// Raises an AssertionException with the given message if obj == null, else
        /// nothing happens
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="failedAssertionMessage">The failed assertion message.</param>
        public static void IsNotNull(object obj, string failedAssertionMessage)
        {
            if (obj == null)
                throw new AssertionException(failedAssertionMessage);
        }

        /// <summary>
        /// Raises an AssertionException if expected is not equal to actual, else
        /// nothing happens
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        public static void AreEqual(object expected, object actual)
        {
            AreEqual(expected, actual, "Are not equal");
        }

        /// <summary>
        /// Raises an AssertionException with the given message if expected is 
        /// not equal to actual, else nothing happens
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="failedAssertionMessage">The failed assertion message.</param>
        public static void AreEqual(object expected, object actual, string failedAssertionMessage)
        {
            string msg = failedAssertionMessage ?? "{0} {1}";
            if (expected == null && actual == null)
                return;
            if (expected == null)
                throw new AssertionException(string.Format(msg, expected, actual));
            if (actual == null)
                throw new AssertionException(string.Format(msg, expected, actual));

            // ints, longs, decimals which are logically
            // equivalent should be equal
            if (NumericCheckForEquality(expected, actual))
                return;

            if (expected.GetType() != actual.GetType())
                throw new AssertionException(string.Format(msg, expected, actual));

            // design decision: we only consider the equality down
            // to the second level, i.e. datetimes with different
            // values for millisecond and below are considered
            // equal
            if (expected is DateTime)
            {
                AreEqualDateTime((DateTime)expected, (DateTime)actual, failedAssertionMessage);
                return;
            }

            if (!expected.Equals(actual))
                throw new AssertionException(string.Format(msg, expected, actual));
        }

        private static bool NumericCheckForEquality(object expected, object actual)
        {
            if (expected is int || expected is long || expected is decimal)
                return expected.ToString() == actual.ToString();
            return false;
        }

        /// <summary>
        /// Iff the two dates are equal all the way down to second, we're happy
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="failedAssertionMessage">The failed assertion message.</param>
        private static void AreEqualDateTime(DateTime expected, DateTime actual, string failedAssertionMessage)
        {
            AreEqual(expected.Year, actual.Year);
            AreEqual(expected.Month, actual.Month);
            AreEqual(expected.Day, actual.Day);
            AreEqual(expected.Hour, actual.Hour);
            AreEqual(expected.Minute, actual.Minute);
            AreEqual(expected.Second, actual.Second);
        }
    }
}
