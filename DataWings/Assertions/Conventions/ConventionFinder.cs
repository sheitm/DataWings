using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace DataWings.Assertions.Conventions
{
    /// <summary>
    /// Finds all conventions for the given test. This is
    /// done by traversing the stack looking for method
    /// and class attribute decorations
    /// </summary>
    public class ConventionFinder
    {
        #region Constructor and Declarations

        private const string NULLKEY = "_NULL_";
        private const string DEFAULT_TABLE_CONVENTION = "_DEFAULT_CONVENTION_";

        private readonly Dictionary<string, DbTableNameConventionAttribute> tableNameConventions =
            new Dictionary<string, DbTableNameConventionAttribute>
                {
                    {DEFAULT_TABLE_CONVENTION, new DbTableNameConventionAttribute(DbTableNameConventionType.ClassNameEqualsTableName)}
                };

        private readonly Dictionary<string, DbIdConventionAttribute> idConventions =
            new Dictionary<string, DbIdConventionAttribute>();

        public ConventionFinder()
        {
            WalkTheStack();
        }

        #endregion

        #region Public API

        /// <summary>
        /// Gets the name of the table for the given entity.
        /// This is done by traversing the stack looking for
        /// an DbTableNameConventionAttribute on method or
        /// class specifying the convention to be used for 
        /// the type of the entity. If no such decoration is
        /// found the conventional convention will be used.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public string GetTableName(object entity)
        {
            return GetTableName(entity.GetType());
        }

        /// <summary>
        /// Gets the name of the table for the given type.
        /// This is done by traversing the stack looking for
        /// an DbTableNameConventionAttribute on method or
        /// class specifying the convention to be used for 
        /// the type. If no such decoration is found the 
        /// conventional convention will be used.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        public string GetTableName(Type entityType)
        {
            if (tableNameConventions.ContainsKey(entityType.Name))
                return tableNameConventions[entityType.Name].GetTableName(entityType);
            if (tableNameConventions.ContainsKey(NULLKEY))
                return tableNameConventions[NULLKEY].GetTableName(entityType);

            return tableNameConventions[DEFAULT_TABLE_CONVENTION].GetTableName(entityType);
        }

        /// <summary>
        /// Gets the name of the table for the given type (T).
        /// This is done by traversing the stack looking for
        /// an DbTableNameConventionAttribute on method or
        /// class specifying the convention to be used for 
        /// the type. If no such decoration is found the 
        /// conventional convention will be used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableName<T>()
        {
            return GetTableName(typeof (T));
        }

        /// <summary>
        /// Gets the id property, i.e. the name of the
        /// property
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public string GetIdProperty(object entity)
        {
            var attrib = GetIdAttribute(entity);
            return attrib.GetPropertyName(entity);
        }

        public object GetId(object entity)
        {
            var attrib = GetIdAttribute(entity);
            return attrib.GetId(entity);
        }


        private DbIdConventionAttribute GetIdAttribute(object entity)
        {
            DbIdConventionAttribute attrib;
            if(idConventions.ContainsKey(entity.GetType().Name))
            {
                attrib = idConventions[entity.GetType().Name];
            }
            else if(idConventions.ContainsKey(NULLKEY))
            {
                attrib = idConventions[NULLKEY];
            }
            else
            {
                attrib = new DbIdConventionAttribute("Id{0}");
            }
            return attrib;
        }

        #endregion

        #region Private

        private void WalkTheStack()
        {
            var callStack = new StackTrace();
            int currentIndex = 0;
            while (currentIndex < callStack.FrameCount)
            {
                var frame = callStack.GetFrame(currentIndex);

                // Important that the method is checked
                // before the class!

                // Method
                SetTableNameAttributes(Attribute.GetCustomAttributes(frame.GetMethod(),
                                                                     typeof(DbTableNameConventionAttribute)));
                SetIdAttributes(Attribute.GetCustomAttributes(frame.GetMethod(),
                                                             typeof (DbIdConventionAttribute)));

                // Class
                SetTableNameAttributes(Attribute.GetCustomAttributes(frame.GetMethod().ReflectedType,
                                                                     typeof(DbTableNameConventionAttribute)));
                SetIdAttributes(Attribute.GetCustomAttributes(frame.GetMethod().ReflectedType,
                                                             typeof(DbIdConventionAttribute)));
                currentIndex++;
            }
        }

        private void SetIdAttributes(IEnumerable<Attribute> attribs)
        {
            if (attribs == null)
                return;
            foreach (DbIdConventionAttribute attrib in attribs)
            {
                string key = attrib.EntityType == null ? NULLKEY : attrib.EntityType.Name;
                if (!idConventions.ContainsKey(key))
                {
                    idConventions.Add(key, attrib);
                }
            }
        }

        private void SetTableNameAttributes(IEnumerable<Attribute> attribs)
        {
            if (attribs == null)
                return;
            foreach (DbTableNameConventionAttribute attrib in attribs)
            {
                string key = attrib.EntityType == null ? NULLKEY : attrib.EntityType.Name;
                if (!tableNameConventions.ContainsKey(key))
                {
                    tableNameConventions.Add(key, attrib);
                }
            }
        }

        #endregion


    }
}
