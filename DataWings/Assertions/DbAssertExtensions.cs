namespace DataWings.Assertions
{
    /// <summary>
    /// Static class for extension methods. Generally contains
    /// extension methods for the API defined in DbAssert.  
    /// </summary>
    public static class DbAssertExtensions
    {
        public static void AssertExistsInDatabase(this object entity)
        {
            DbAssert.Exists(entity);
        }

        public static void AssertNotExistsInDatabase(this object entity)
        {
            DbAssert.NotExists(entity);
        }

        public static void AssertColumnEquals(this object entity, string propertyName)
        {
            DbAssert.ColumnEquals(entity, propertyName);
        }
    }
}
