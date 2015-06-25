using System;

namespace DataWings.Assertions
{
    /// <summary>
    /// Exception thrown whenever an assertion fails
    /// </summary>
    public class DataWingsAssertionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataWingsAssertionException"/> class.
        /// </summary>
        public DataWingsAssertionException()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWingsAssertionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DataWingsAssertionException(string message) : base(message)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWingsAssertionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DataWingsAssertionException(string message, Exception inner) : base(message, inner)
        {}
    }
}
