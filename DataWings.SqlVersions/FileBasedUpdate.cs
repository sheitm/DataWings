using System;
using System.IO;
using DataWings.IO;

namespace DataWings.SqlVersions
{
    public class FileBasedUpdate
    {
        /// <summary>
        /// Updates the specified path.
        /// </summary>
        /// <param name="path">The update (sql) script</param>
        public FileBasedUpdate(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            if (!path.FileExists())
            {
                throw new ArgumentException(string.Format("File {0} does not exist.", path));
            }

            SetData(path);
        }

        public int FolderSequenceId { get; private set; }

        public int SequenceId { get; private set; }

        public string UpdateScript { get; private set; }

        public string Description { get; private set; }

        private void SetData(string path)
        {
            UpdateScript = path;
            SequenceId = ParseAndgetSequenceId(Path.GetFileName(path), true);

            var pathSplit = path.Split('\\');
            FolderSequenceId = ParseAndgetSequenceId(pathSplit[pathSplit.Length - 2], false);
        }

        private int ParseAndgetSequenceId(string source, bool throwIfNoUnderscore)
        {
            var split = Path.GetFileName(source).Split(new char[] { '_' });
            if (throwIfNoUnderscore && split.Length < 2)
            {
                throw new ArgumentException(string.Format("Update script {0} is missing underscore.", Path.GetFileName(source)));
            }

            return GetSequenceId(split[0]);
        }

        private int GetSequenceId(string source)
        {
            int seqId;
            if (!int.TryParse(source, out seqId))
            {
                throw new ArgumentException(string.Format("Unparsable sequence in script {0}.", source));
            }

            return seqId;
        }
    }
}
