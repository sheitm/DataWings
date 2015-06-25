namespace DataWings.DataMaintenance
{
    public class ValueQueryWhere : IValueQueryWhere
    {
        private WhereContraintAdder _adder;
        private string _columnName;

        public ValueQueryWhere(WhereContraintAdder adder, string columnName)
        {
            _adder = adder;
            _columnName = columnName;
        }

        public IInitializedValueQuey Eq(object value)
        {
            return _adder.Invoke(_columnName, value);    
        }
    }
}
