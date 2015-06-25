using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DataWings.SqlVersions.Cmd
{
    public class UpdateDatabaseCommand : ICommand
    {
        private IDictionary<string, string> _parameters;

        private string BaseDirectory
        {
            get { return _parameters["BaseDirectory".ToLower()]; }
        }

        private string DatabaseName
        {
            get { return _parameters["DatabaseName".ToLower()]; }
        }

        private string DatabaseServer
        {
            get { return _parameters["DatabaseServer".ToLower()]; }
        }

        private string ConnectionString
        {
            get
            {
                return string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", DatabaseServer, DatabaseName);
            }
        }

        /// <summary>
        /// Invokes the command with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void Invoke(IDictionary<string, string> parameters)
        {
            _parameters = parameters.Aggregate(
                new Dictionary<string, string>(),
                (map, pair) =>
                    {
                        map.Add(pair.Key.ToLower(), pair.Value);
                        return map;
                    });

            var updates = new Updater(BaseDirectory, DatabaseServer, DatabaseName).GetUpdates();
            if (updates == null || updates.Count == 0)
            {
                return;
            }

            foreach (var update in updates)
            {
                UpdateDatabase(update);
            }
        }

        private void UpdateDatabase(Update update)
        {
            var script = update.GetScriptContents();
        }
    }
}
