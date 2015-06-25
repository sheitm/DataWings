namespace DataWings.SqlVersions
{
    using DataWings.IO;
    using System;
    using System.Diagnostics;

    public class Creator
    {
        private string _databaseName;
        private string _directory;

        public Creator(string databaseName)
            : this(databaseName, IoExtensionMethods.GetCurrentDirectory())
        {
        }

        public Creator(string databaseName, string directory)
        {
            this._databaseName = databaseName;
            this._directory = directory;
        }

        public string CreateDatabaseScript()
        {
            string contents = Scripts.CreateDatabase.Replace(Scripts.DbUseName, this._databaseName);
            string str2 = this._directory.PathCombineWith(Guid.NewGuid() + ".sql");
            Trace.WriteLine(string.Format("Writing database create script: {0}", str2));
            str2.WriteAllText(contents);
            return str2;
        }
    }
}

