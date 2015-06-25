using System.IO;

namespace DataWings.SqlVersions
{
    using DataWings.IO;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class Updater
    {
        private string _baseDirectory;
        private string _database;
        private string _server;

        public Updater(string baseDirectory, string server, string database)
        {
            _baseDirectory = baseDirectory;
            _server = server;
            _database = database;
        }

        private IEnumerable<Update> GetAllLoadableUpdates()
        {
            var loaded = GetAllLoadedSequenceIds();
            return _baseDirectory.GetDirectories()
                .Where(d => IsUpdateDirectory(d) && !DirectoryIsLoaded(d, loaded))
                .Select(d => new Update(d));
        }

        private bool DirectoryIsLoaded(string path, IList<int> loaded)
        {
            var args = path.Split('\\');
            var seq = int.Parse(args[args.Length - 1]);
            return loaded.Contains(seq);
        }

        private bool IsUpdateDirectory(string path)
        {
            var args = path.Split('\\');
            return args[args.Length - 1].All(c => char.IsDigit(c));
        }

        private IList<int> GetAllLoadedSequenceIds()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var loaded = new List<int>();
                connection.Open();
                var command = new SqlCommand("SELECT SequenceId FROM Version.Version", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loaded.Add(reader.GetInt32(0));
                    }
                }

                connection.Close();

                return loaded;
            }
        }

        public IList<Update> GetUpdates()
        {
            SortedList<int, Update> list = new SortedList<int, Update>();
            foreach (Update update in this.GetAllLoadableUpdates())
            {
                list.Add(update.SequenceId, update);
            }
            return list.Values;
        }

        private string ConnectionString
        {
            get
            {
                return string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", _server, _database);
            }
        }
    }
}

