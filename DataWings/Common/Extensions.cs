using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataWings.Assertions;

namespace DataWings.Common
{
    /// <summary>
    /// Static class for various extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns a string representing the incoming value. This string
        /// can be embedded in a sql statement.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToSqlValueString(this object value)
        {
            if(value is string && ((string)value).StartsWith("$$"))
            {
                string v = (string) value;
                return v.Substring(2, (v.Length - 2));
            }

            if (value is string || value is Guid)
                return "'" + value + "'";
            return value.ToString();
        }

        /// <summary>
        /// Combines the template and additionalMessage with information contained in the
        /// assertion in order to produce and return a comprehensive message to be displayed 
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="assertion">The assertion.</param>
        /// <param name="additionalMessage">The additional message.</param>
        /// <returns></returns>
        public static string ToFailedAssertionMessage(this string template, AccumulativeAssertion assertion, string additionalMessage)
        {
            string res = String.Format(template, assertion.WhereColumnDisplayString, assertion.TableName);
            if (additionalMessage == null)
                return res;
            return res + " " + additionalMessage;
        }

        public static Dictionary<string, string> ToColumnValuePairs(this string columnValueStringPair)
        {
            if (columnValueStringPair == null)
                throw new ArgumentNullException("columnValueStringPair");

            var result = new Dictionary<string, string>();
            using(var reader = new StringReader(columnValueStringPair))
            {
                int i;
                do
                {
                    i = reader.Peek();
                    if (i >= 0)
                    {
                        string key = GetKey(reader).Trim().ToUpper();
                        string val = GetValue(reader).Trim();
                        result.Add(key, val);
                    }
                } while (i >= 0);
            }
            return result;
        }

        private static string GetKey(TextReader reader)
        {
            var sb = new StringBuilder();
            char c;
            do
            {
                c = (char) reader.Read();
                if (c != '=') sb.Append(c);
            } while (c != '=');
            return sb.ToString();
        }

        private static string GetValue(StringReader reader)
        {
            var sb = new StringBuilder();
            bool inComment = false;
            bool finished = false;
            do
            {
                int i = reader.Read();
                if (i < 0)
                {
                    finished = true;
                }
                else
                {
                    char c = (char) i;
                    switch (c)
                    {
                        case '\'':
                            inComment = !inComment;
                            sb.Append(c);
                            break;
                        case ';':
                            if (!inComment)
                            {
                                finished = true;
                            }
                            else
                            {
                                sb.Append(c);
                            }
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }
            } while (!finished);
            return sb.ToString();
        }
    }
}