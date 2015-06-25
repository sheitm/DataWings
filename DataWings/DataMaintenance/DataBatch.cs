using System;
using System.Collections.Generic;
using DataWings.Common;

namespace DataWings.DataMaintenance
{
    /// <summary>
    /// Can be built up (accumulated) with any number of row definitions,
    /// each specifying data that should be inserted in the table 
    /// </summary>
    public class DataBatch : IDataAccumulatorBatch
    {
        #region Constructor and Declarations

        private string tableName;
        private readonly List<DataRow> rows = new List<DataRow>();
        private DataRow currentRow;
        private DataSession parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBatch"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="parent">The parent.</param>
        public DataBatch(string tableName, DataSession parent)
        {
            this.tableName = tableName;
            this.parent = parent;
        }

        #endregion

        /// <summary>
        /// Gets the name of the table form this batch
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName
        {
            get { return tableName; }
        }

        /// <summary>
        /// Creates a new IDataAccumulatorBatch and adds it to the
        /// inner data structure. This instance of IDataAccumulatorBatch
        /// represents a batch of operations that are to be invoked
        /// against the table with the given name.
        /// </summary>
        /// <param name="_tableName">Name of the table.</param>
        /// <returns></returns>
        public IDataAccumulatorBatch ForTable(string _tableName)
        {
            return parent.ForTable(_tableName);
        }

        /// <summary>
        /// Commits the accumulative session, i.e. performs the actual
        /// changes in the database
        /// </summary>
        public void Commit()
        {
            parent.Commit();
        }

        /// <summary>
        /// Creates a new accumulative row and adds it to the inner
        /// data structure
        /// </summary>
        /// <param name="idColumnName">Name of the id column.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IDataAccumulatorRow Row(string idColumnName, object id)
        {
            currentRow = new DataRow(idColumnName, id, this);
            rows.Add(currentRow);
            return currentRow;
        }

        public void Values(params IColumnValuePair[] columnValuePairs)
        {
            if (columnValuePairs == null)
                throw new ArgumentException("Can not be null: columnValuePairs.");
            var row = new DataRow(this);
            rows.Add(row);
            row.Values(columnValuePairs);
        }

        internal void DoWrite(ISqlProvider provider)
        {
            foreach (var row in rows)
            {
                row.DoWrite(provider);
            }
        }

        internal void DoDelete(ISqlProvider provider)
        {
            for (int i = rows.Count - 1; i >= 0; i--)
            {
                rows[i].DoDelete(provider);
            }
        }
    }
}