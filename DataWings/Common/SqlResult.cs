using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataWings.Common
{
    /// <summary>
    /// Represents a single row from the database
    /// </summary>
    public class SqlResult : ISqlResult
    {
        #region Constructor and Declaration

        private readonly Dictionary<string, object> valueMap = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlResult"/> class.
        /// </summary>
        public SqlResult()
        {}

        #endregion

        #region ISqlResult Members

        /// <summary>
        /// Gets the single result, i.e. the first column/value pair
        /// encountered. This method is probably most useful in situations
        /// when one knows that only one row is returned
        /// </summary>
        /// <returns>The first column/value pair encountered</returns>
        public object GetSingleResult()
        {
            foreach (var pair in valueMap)
            {
                return pair.Value;
            }
            throw new DataException("No rows returned from database");
        }

        /// <summary>
        /// Gets the single result, i.e. the first column/value pair
        /// encountered. This method is probably most useful in situations
        /// when one knows that only one row is returned
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <returns>The first column/value pair encountered</returns>
        public T GetSingleResult<T>()
        {
            object o = GetSingleResult();
            if (o == null) return default(T);

            // Hacky!
            if (typeof(decimal) == typeof(T) && typeof(int) == o.GetType())
            {
                object o2 = Convert.ToDecimal(o);
                return (T) o2;
            }

            return (T) GetSingleResult();
        }

        /// <summary>
        /// Gets the result keyed at the given key. This key should be
        /// a column name. The key is case insensitive (in keeping with
        /// sql standards).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value</returns>
        public object GetResult(string key)
        {
            object v = valueMap[key.ToUpper()];
            if (v is DBNull) return null;
            return v;
        }

        /// <summary>
        /// Gets the result keyed at the given key. This key should be
        /// a column name. The key is case insensitive (in keeping with
        /// sql standards).
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The value</returns>
        public T GetResult<T>(string key)
        {
            return (T) GetResult(key);
        }

        #endregion

        #region Public API

        /// <summary>
        /// Sets the data based on the incoming sql reader. The reader is assumed to already
        /// have been advanced (reader.Read()) to the next row by the caller of this method.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void SetData(DbDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                AddValue(reader.GetName(i), reader.GetValue(i));
            }
        }

        #endregion

        #region Private

        private void AddValue(string key, object value)
        {
            string k = key.ToUpper();
            if (valueMap.ContainsKey(k))
                throw new InvalidOperationException("Value at key " + k + " already registered");
            valueMap.Add(k, value);
        }

        #endregion
    }
}