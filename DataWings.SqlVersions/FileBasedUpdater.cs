using System.Collections.Generic;
using System.Linq;
using DataWings.IO;

namespace DataWings.SqlVersions
{
    /// <summary>
    /// Makes a full update (or creation) of the database in a file-based
    /// manner, i.e. when each change is contained in a seperate, named
    /// and enumerated by convention
    /// </summary>
    public class FileBasedUpdater
    {
        private readonly string _baseDirectory;
        private readonly string _database;
        private readonly string _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBasedUpdater"/> class.
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
        /// <param name="server">The server.</param>
        /// <param name="database">The database.</param>
        public FileBasedUpdater(string baseDirectory, string server, string database)
        {
            _baseDirectory = baseDirectory;
            _server = server;
            _database = database;
        }

        /// <summary>
        /// Gets the updates sorted correctly.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FileBasedUpdate> GetUpdates()
        {
            //new SortedList<string, FileBasedUpdate>()
            return _baseDirectory.GetFiles("*.sql").Select(file => new FileBasedUpdate(file));
        }
    }
}
