using DataWings.Common;

namespace DataWings.Tests.Stubs
{
    public class TestConnectionAttribute : AbstractConnectionAttribute
    {
        public TestConnectionAttribute(SqlVendor vendor) : base(vendor)
        {
        }

        public override ISqlProvider GetExecutor()
        {
            return StubSqlProvider.Current;
        }

        protected override string GetConnectionString()
        {
            return "ConnectionString";
        }
    }
}
