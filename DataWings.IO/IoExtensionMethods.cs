using System;
using System.IO;

namespace DataWings.IO
{
    /// <summary>
    /// Static class that wraps the functionalty that is found the the System.IO
    /// namespace, primarily the static classes File, Directory and Path. All
    /// operations against the file system are handled by action/function invocations,
    /// and it is possible to set own actions and functions replacing the default
    /// ones, giving you the ability to stub out the file system.
    /// </summary>
    public static class IoExtensionMethods
    {
      
        #region Declarations and Static constructor

        private static Func<string, bool> _funcFileExists;
        private static Func<string, string> _funcReadAllText;
        private static Action<string> _actionDeleteFile;
        private static Action<string, string> _actionWriteAllText;
        private static Func<string, string[]> _funcReadAllLines;
        private static Func<string, string, string> _funcPathCombineWith;
        private static Func<string, string[]> _funcGetDirectories;
        private static Func<string, string, string[]> _funcGetFiles;
        private static Func<string> _funcGetCurrentDirectory;

        static IoExtensionMethods()
        {
            Reset();
        }

        /// <summary>
        /// Resets this static class by setting all actions and functions back to
        /// their original values where they access the functionality in the
        /// System.IO namespace
        /// </summary>
        public static void Reset()
        {
            _funcFileExists = path => File.Exists(path);
            _funcReadAllText = path => File.ReadAllText(path);
            _actionDeleteFile = path => File.Delete(path);
            _actionWriteAllText = (path, contents) => File.WriteAllText(path, contents);
            _funcReadAllLines = path => File.ReadAllLines(path);
            _funcPathCombineWith = (path1, path2) => Path.Combine(path1, path2);
            _funcGetDirectories = directory => Directory.GetDirectories(directory);
            _funcGetFiles = (directory, mask) => Directory.GetFiles(directory, mask);
            _funcGetCurrentDirectory = () => Directory.GetCurrentDirectory();
        }

        #endregion

        #region IO Emulation

        public static bool FileExists(this string path)
        {
            return _funcFileExists.Invoke(path);
        }

        public static string ReadAllText(this string path)
        {
            return _funcReadAllText.Invoke(path);
        }

        public static void DeleteFile(this string path)
        {
            _actionDeleteFile.Invoke(path);
        }

        public static void WriteAllText(this string path, string contents)
        {
            _actionWriteAllText.Invoke(path, contents);
        }

        public static string[] ReadAllLines(this string path)
        {
            return _funcReadAllLines.Invoke(path);
        }

        public static string PathCombineWith(this string path1, string path2)
        {
            return _funcPathCombineWith.Invoke(path1, path2);
        }

        public static string[] GetDirectories(this string directory)
        {
            return _funcGetDirectories.Invoke(directory);
        }

        public static string[] GetFiles(this string directory, string mask)
        {
            return _funcGetFiles.Invoke(directory, mask);
        }

        public static string GetCurrentDirectory()
        {
            return _funcGetCurrentDirectory.Invoke();
        }

        #endregion

        #region Setting functions and actions

        public static Func<string, bool> FunctionFileExists
        {
            set { _funcFileExists = value; }
        }

        public static Func<string, string> FunctionReadAllText
        {
            set { _funcReadAllText = value; }
        }

        public static Action<string> ActionDeleteFile
        {
            set { _actionDeleteFile = value; }
        }

        public static Action<string, string> ActionWriteAllText
        {
            set { _actionWriteAllText = value; }
        }

        public static Func<string, string[]> FunctionReadAllLines
        {
            set { _funcReadAllLines = value; }
        }

        public static Func<string, string, string> FunctionPathCombineWith
        {
            set { _funcPathCombineWith = value; }
        }

        public static Func<string, string[]> FunctionGetDirectories
        {
            set { _funcGetDirectories = value; }
        }

        public static Func<string, string, string[]> FunctionGetFiles
        {
            set { _funcGetFiles = value; }
        }

        public static Func<string> FunctionGetCurrentDirectory
        {
            set { _funcGetCurrentDirectory = value; }
        }

        #endregion
    }
}
