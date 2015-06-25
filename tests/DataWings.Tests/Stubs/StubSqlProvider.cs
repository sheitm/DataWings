using System;
using System.Collections.Generic;
using DataWings.Common;

namespace DataWings.Tests.Stubs
{
    public class StubSqlProvider : ISqlProvider
    {
        #region Static API

        private static StubSqlProvider current;

        public static StubSqlProvider Current
        {
            get
            {
                if (current == null) current = new StubSqlProvider();
                return current;
            }
        }

        public static void Clear()
        {
            current = null;
        }

        #endregion

        private string sql;
        private object returnObject;
        private Dictionary<string, object> valueMap;
        private readonly IList<string> executedQueries = new List<string>();

        public string Sql
        {
            get { return sql; }
        }

        public ISqlResult ExecuteSelect(string _sql)
        {
            sql = _sql;
            return null;
        }

        public IDictionary<string, object> ExecuteMultiReturnSelect(string sql)
        {
            executedQueries.Add(sql);
            throw new System.NotImplementedException();
        }

        public void SetValueMap(Dictionary<string, object> map)
        {
            valueMap = map;
        }

        public void SetObjectToReturn(object obj)
        {
            returnObject = obj;
        }

        public bool HasAnySqlMatching(string sqlToMatch)
        {
            foreach (var query in executedQueries)
            {
                if (query == sqlToMatch)
                    return true;
            }
            return false;
        }

        public IList<ISqlResult> ExecuteQuery(string _sql, SelectOptions selectOptions)
        {
            sql = _sql;
            executedQueries.Add(sql);
            return new List<ISqlResult>
                       {
                           GetSqlResult()
                       };
        }

        private ISqlResult GetSqlResult()
        {
            return new StubSqlResult
                       {
                           ReturnObject = returnObject,
                           ValueMap = valueMap
                       };
        }

        public void ExecuteNonQuery(string sql)
        {
            executedQueries.Add(sql);
        }

        public string ConnectionString
        {
            get { throw new NotImplementedException(); }
        }

       public string GetExecutedQuery(int index)
       {
           return executedQueries[index];
       }
    }

    public class StubSqlResult : ISqlResult
    {
        public StubSqlResult()
        {
        }

        public Dictionary<string, object> ValueMap { get; set; }

        public object ReturnObject { get; set; }

        public object GetSingleResult()
        {
            return ReturnObject;
        }

        public T GetSingleResult<T>()
        {
            return (T) GetSingleResult();
        }

        public object GetResult(string key)
        {
            return ValueMap[key];
        }

        public T GetResult<T>(string key)
        {
            return (T) GetResult(key);
        }
    }
}