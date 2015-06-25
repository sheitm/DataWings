namespace DataWings.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        private bool isSetup = false;
        private string executedSql;

        private int _id;

        public virtual void SetUp()
        {
            if (!isSetup)
            {
                isSetup = true;
                Database.SetUp();
                Database.SqlExecuted += (o, e) => executedSql = e.Sql;
            }
        }

        protected int NextId()
        {
            return ++_id;
        }

        protected string ExecutedSql
        {
            get { return executedSql; }
        }
    }
}
