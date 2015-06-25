using System;

namespace DataWings.Assertions
{
    /// <summary>
    /// Exception raised when an assertion has failed
    /// </summary>
    public class AssertionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionException"/> class.
        /// </summary>
        public AssertionException() : base()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssertionException(string message) : base(message)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public AssertionException(string message, Exception inner) :base(message, inner)
        {}
    }
}
