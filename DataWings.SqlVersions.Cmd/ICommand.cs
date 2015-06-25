using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataWings.SqlVersions.Cmd
{
    /// <summary>
    /// Common API for all classes that implement commands that
    /// can be invoked from the command line
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Invokes the command with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        void Invoke(IDictionary<string, string> parameters);
    }
}
