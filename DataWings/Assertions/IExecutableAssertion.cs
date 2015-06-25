using System;
using DataWings.Common;

namespace DataWings.Assertions
{
    /// <summary>
    /// Assertion containing methods that when invoked will
    /// mark the end of the accumulative build up phase, and
    /// the actual db call änd assertive comparison will be
    /// executed.
    /// </summary>
    public interface IExecutableAssertion
    {
        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// 
        /// Asserts that a single row matching the criteria exists.
        /// </summary>
        void Exists();

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// 
        /// Asserts that a single row matching the criteria exists.
        /// </summary>
        /// <param name="failedAssertionMessage">A message to be displayed if the assertion fails</param>
        void Exists(string failedAssertionMessage);

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// 
        /// Asserts that a single row matching the criteria does not
        /// exist.
        /// </summary>
        void NotExists();

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// 
        /// Asserts that a single row matching the criteria does not
        /// exist.
        /// </summary>
        /// <param name="failedAssertionMessage">A message to be displayed if the assertion fails</param>
        void NotExists(string failedAssertionMessage);

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// 
        /// Asserts that the value in the db
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="compareWith"></param>
        IExecutableAssertion AreEqual(string columnName, object compareWith);

        /// <summary>
        /// This is a command marking the end of the accumulation
        /// phase, and the assertion will be executed immediatly here.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="compareWith"></param>
        /// <param name="failedAssertionMessage">A message to be displayed if the assertion fails</param>
        IExecutableAssertion AreEqual(string columnName, object compareWith, string failedAssertionMessage);

        /// <summary>
        /// Evaluates the specified function.
        /// </summary>
        /// <param name="function">The function.</param>
        void Evaluate(Func<ISqlResult, bool> function);

    }
}