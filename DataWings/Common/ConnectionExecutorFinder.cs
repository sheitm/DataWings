using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace DataWings.Common
{
    /// <summary>
    /// Defines a static API for accessing the Sql executor that should be used.
    /// These executors are computed once and then cached. If an appropriate executor
    /// is not in the cache, it will be found, and this is done be traversing the stack
    /// backwards until a method or class (in that order) that is decorated with an
    /// appropriate ConnectionAttribute is located
    /// </summary>
    public class ConnectionExecutorFinder
    {
        #region Static API

        private readonly static Dictionary<string, ISqlProvider> executorMap = new Dictionary<string, ISqlProvider>();

        /// <summary>
        /// Gets the SQL executor. If an appropriate executor
        /// is not in the cache, it will be found, and this is done be traversing the stack
        /// backwards until a method or class (in that order) that is decorated with a connection
        /// attribute
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static ISqlProvider GetSqlExecutor(string name)
        {
            if (name == null && executorMap.Count > 0)
            {
                return executorMap.Values.First(x => true);
            }

            var finder = new ConnectionExecutorFinder(name);
            var result = finder.GetExecutorByWalkingTheStack();
            executorMap.Add(finder.Name, result);
            return result;
        }

        /// <summary>
        /// Resets by clearing the internal cache of executors.
        /// </summary>
        public static void Reset()
        {
            executorMap.Clear();
        }

        #endregion

        #region Constructor and Declarations

        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionExecutorFinder"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ConnectionExecutorFinder(string name)
        {
            this.name = name;
        }

        #endregion

        /// <summary>
        /// Gets the executor by walking the stack looking for an appropriate ConnectionAttribute
        /// decoration on either method or class (in that order). If no such decoration is found,
        /// an InvalidOperationException will be thrown.
        /// </summary>
        /// <returns></returns>
        public ISqlProvider GetExecutorByWalkingTheStack()
        {
            AbstractConnectionAttribute attrib = GetConnectionAttribute();
            if (attrib == null)
                throw new InvalidOperationException("Unable to locate connection string attribute");
            name = attrib.Name;
            return attrib.GetExecutor();
        }

        /// <summary>
        /// Gets the name. This is the name from the ConnectionAttribute decoration
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                if (name == null) return Guid.NewGuid().ToString();

                return name;
            }
        }

        private AbstractConnectionAttribute GetConnectionAttribute()
        {
            var callStack = new StackTrace();
            int currentIndex = 0;
            while (currentIndex < callStack.FrameCount)
            {
                var frame = callStack.GetFrame(currentIndex);
                var attrib = GetConnectionForMethod(frame.GetMethod());
                if (attrib != null) return attrib;
                currentIndex++;
            }
            return null;
        }

        private AbstractConnectionAttribute GetConnectionForMethod(MemberInfo method)
        {
            // First, check if the method is decorated
            AbstractConnectionAttribute connectionAttribute =
                GetAttribFromList(Attribute.GetCustomAttributes(method, typeof (AbstractConnectionAttribute)));
            if (connectionAttribute != null) return connectionAttribute;

            // Method not decorated. Check class
            return GetAttribFromList(Attribute.GetCustomAttributes(method.DeclaringType, typeof(AbstractConnectionAttribute)));
        }

        private AbstractConnectionAttribute GetAttribFromList(Attribute[] candidates)
        {
            if (candidates == null || candidates.Length == 0) return null;

            if(String.IsNullOrEmpty(name))
            {
                return (AbstractConnectionAttribute) candidates[0];
            }
            foreach (var attribute in candidates)
            {
                if (((AbstractConnectionAttribute)attribute).Name == name)
                    return (AbstractConnectionAttribute) attribute;
            }
            return null;
        }
    }
}
