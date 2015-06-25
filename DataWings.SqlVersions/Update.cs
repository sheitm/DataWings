using System.IO;

namespace DataWings.SqlVersions
{
    using DataWings.IO;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class Update
    {
        [CompilerGenerated]
        private string _description;
        [CompilerGenerated]
        private int _sequenceId;
        [CompilerGenerated]
        private string _updateScript;
        private const string FILE_DESCRIPTION = "desc.txt";
        private const string FILE_UPDATE = "update.sql";
        private const string INSERT_VERSION_MARKER = "-- INSERT VERSION";

        public Update()
        {
        }

        public Update(string directory)
        {
            this.UpdateScript = directory.PathCombineWith("update.sql");
            string path = directory.PathCombineWith("desc.txt");
            if (path.FileExists())
            {
                this.Description = path.ReadAllText();
            }
            string[] strArray = directory.Split(new char[] { '\\' });
            try
            {
                this.SequenceId = int.Parse(strArray[strArray.Length - 1]);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Path must end with valid integer.");
            }
        }

        public string GetScriptContents()
        {
            return File.ReadAllText(UpdateScript);
        }

        private string GetDescriptionForSqlInsert()
        {
            if (this.Description == null)
            {
                return "NULL";
            }
            return string.Format("'{0}'", this.Description.Replace("'", "''"));
        }

        public string Description
        {
            [CompilerGenerated]
            get
            {
                return this._description;
            }
            [CompilerGenerated]
            private set
            {
                this._description = value;
            }
        }

        public int SequenceId
        {
            [CompilerGenerated]
            get
            {
                return this._sequenceId;
            }
            [CompilerGenerated]
            private set
            {
                this._sequenceId = value;
            }
        }

        public string UpdateScript
        {
            [CompilerGenerated]
            get
            {
                return this._updateScript;
            }
            [CompilerGenerated]
            private set
            {
                this._updateScript = value;
            }
        }
    }
}

