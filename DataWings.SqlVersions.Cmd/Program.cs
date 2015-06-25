using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataWings.SqlVersions.Cmd
{
    class Program
    {
        private static readonly Dictionary<string, Type> Commands = new Dictionary<string, Type>
                                                                        {
                                                                            {"update", typeof (UpdateDatabaseCommand)}
                                                                        };

        /// <summary>
        /// Example commands:
        /// 
        /// Update:
        /// DataWings.SqlVersions.Cmd.exe Update BaseDirectory:c:\temp DatabaseName:Tiril DatabaseServer:(local)
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            var a = new string[] { "Update", @"BaseDirectory:C:\depot_hg\TirilWeb\Database", "DatabaseName:Tiril", "DatabaseServer:(local)" };
            ProcessCommand(a);
        }

        private static void ProcessCommand(string[] args)
        {
            var command = Activator.CreateInstance(Commands[args[0].ToLower()]);
            ((ICommand)command).Invoke(GetParameters(args));
        }

        private static IDictionary<string, string> GetParameters(string[] args)
        {
            var map = new Dictionary<string, string>();
            for (int i = 1  ; i < args.Length; i++)
            {
                var element = args[i];
                var index = element.IndexOf(':');
                var key = element.Substring(0, index);
                var value = element.Substring(index + 1, (element.Length - index) - 1);
                map.Add(key, value);
            }

            return map;
        }
    }
}
