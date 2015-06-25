using System.Text;
using DataWings.IO;

namespace DataWings.SqlVersions
{
    public static class SqlScriptFixer
    {
        public static void FixSqlScript(this string path, string databaseName)
        {
            FixUse(path, databaseName);
        }

        private static void FixUse(string path, string databaseName)
        {
            StringBuilder builder = new StringBuilder();
            string str = "USE " + databaseName;
            bool flag = false;
            foreach (string str2 in path.ReadAllLines())
            {
                if (NeedsFixing(str2, databaseName))
                {
                    flag = true;
                    builder.AppendLine(str);
                }
                else
                {
                    builder.AppendLine(str2);
                }
            }
            if (flag)
            {
                path.DeleteFile();
                path.WriteAllText(builder.ToString());
            }
        }

        private static bool NeedsFixing(string line, string databaseName)
        {
            if (!line.ToUpper().StartsWith("USE "))
            {
                return false;
            }
            return !line.Contains(databaseName);
        }
    }
}

