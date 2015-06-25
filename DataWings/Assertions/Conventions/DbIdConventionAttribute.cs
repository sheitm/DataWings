using System;
using System.Reflection;

namespace DataWings.Assertions.Conventions
{
    public class DbIdConventionAttribute : Attribute
    {
        private string convention;

        public DbIdConventionAttribute(string convention)
        {
            this.convention = convention;
        }

        public Type EntityType { get; set; }

        /// <summary>
        /// Gets the unique id of the entity through
        /// the convention given in this types constructor.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public object GetId(object entity)
        {
            if (convention == null)
                throw new InvalidOperationException("Id convention not set.");
            return GetIdCore(entity, GetPropertyName(entity));
        }

        public string GetPropertyName(object entity)
        {
            if (IsFixed) return convention;
            return string.Format(convention, entity.GetType().Name);
        }

        private bool IsFixed
        {
            get { return !convention.Contains("{0}"); }
        }

        private object GetIdCore(object entity, string property)
        {
            var method = entity.GetType()
                .GetProperty(property, BindingFlags.Instance | BindingFlags.Public)
                .GetGetMethod();
            return method.Invoke(entity, new object[0]);
        }
    }
}
