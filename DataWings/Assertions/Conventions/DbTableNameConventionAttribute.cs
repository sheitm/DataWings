using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataWings.Assertions.Conventions
{
    public enum DbTableNameConventionType
    {
        ClassNameEqualsTableName,

        Custom
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class DbTableNameConventionAttribute : Attribute
    {
        private DbTableNameConventionType conventionType;
        private string convention;

        public DbTableNameConventionAttribute(DbTableNameConventionType conventionType)
        {
            this.conventionType = conventionType;
        }

        /// <summary>
        /// Gets or sets the name of the entity. If this property
        /// returns null, the attribute is applicable for all 
        /// entities that are not otherwise mentioned in some
        /// other attribute decoration
        /// </summary>
        /// <value>The name of the entity.</value>
        public Type EntityType { get; set; }

        /// <summary>
        /// Gets or sets the convention to be used. This
        /// </summary>
        /// <value>The convention.</value>
        public string Convention
        {
            get { return convention; }
            set
            {
                if (value == null)
                {
                    convention = null;
                }
                else
                {
                    if (!value.Contains("{0}"))
                        throw new ArgumentException("Convention for DbTableNameConvention must contain '{0}'");

                    convention = value;
                }
            }
        }

        public string GetTableName(Type entityType)
        {
            if (DbTableNameConventionType.ClassNameEqualsTableName == conventionType)
                return entityType.Name;

            if (Convention == null)
                throw new InvalidOperationException("Convention type is Custom, but convention is not set.");

            return string.Format(Convention, entityType.Name);
        }
    }
}
