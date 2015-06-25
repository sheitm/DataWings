using System;
using System.Collections.Generic;
using System.Data;
using DataWings.Common;

namespace DataWings.DataMaintenance
{
    /// <summary>
    /// An accumulator session is a construct that can be built up
    /// (accumulated) with any number of IDataAccumulatorBatch objects
    /// each containing data that should be written to the database
    /// </summary>
    public class DataSession : IDataAccumulatorSession
    {
        #region Constructor and Declarations

        private readonly List<DataBatch> batches = new List<DataBatch>();
        private DataBatch currentBatch;
        private string connectionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSession"/> class.
        /// </summary>
        public DataSession()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSession"/> class.
        /// </summary>
        /// <param name="connectionKey">The connection key.</param>
        public DataSession(string connectionKey)
        {
            this.connectionKey = connectionKey;
        }

        #endregion

        #region IDataAccumulatorSession Members


        /// <summary>
        /// Creates a new IDataAccumulatorBatch and adds it to the
        /// inner data structure. This instance of IDataAccumulatorBatch
        /// represents a batch of operations that are to be invoked
        /// against the table with the given name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public IDataAccumulatorBatch ForTable(string tableName)
        {
            currentBatch = new DataBatch(tableName, this);
            batches.Add(currentBatch);
            return currentBatch;
        }

        /// <summary>
        /// Commits the accumulative session, i.e. performs the actual
        /// changes in the database
        /// </summary>
        public void Commit()
        {
            var sqlExecutor = ConnectionExecutorFinder.GetSqlExecutor(connectionKey);

            // First, delete the rows from back to front
            for (int i = batches.Count-1; i >= 0; i--)
            {
                batches[i].DoDelete(sqlExecutor);
            }

            // Then, insert the rows from front to back
            foreach (var batch in batches)
            {
                batch.DoWrite(sqlExecutor);
            }
        }

        #endregion

        #region In support of return values

        private static ReturnValueCommand _lastRegisteredCommand;
        private static readonly Dictionary<string, ReturnValueCommand> CommandMap = new Dictionary<string, ReturnValueCommand>();

        internal static void AddReturnValueCommand(ReturnValueCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            _lastRegisteredCommand = command;
            if (command.Key != null)
            {
                if (CommandMap.ContainsKey(command.Key))
                    throw new DuplicateNameException(string.Format("Return command with name {0} already registered.", command.Key));

                CommandMap.Add(command.Key, command);
            }
        }

        public static object GetLastReturnValue()
        {
            if (_lastRegisteredCommand == null)
                throw new InvalidOperationException("No return value is registered.");

            return _lastRegisteredCommand.Value;
        }

        public static object GetReturnValueAt(string key)
        {
            if (!CommandMap.ContainsKey(key))
                throw new KeyNotFoundException(string.Format("No return value registered at key {0}.", key));

            return CommandMap[key].Value;
        }

        #endregion

    }
}