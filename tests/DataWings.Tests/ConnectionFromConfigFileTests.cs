using System;
using NUnit.Framework;
using DataWings.Common;

namespace DataWings.Tests
{
    [TestFixture]
    public class ConnectionFromConfigFileTests
    {
        [Test]
        public void GetExecutor_KeyRegisteredInConfigFile_ReturnsCorrectConnectionString()
        {
            var conn = new ConnectionFromConfigFile(SqlVendor.Oracle)
            {
                Key = "K1",
            };
            var executor = conn.GetExecutor();
            Assert.AreEqual("K1", executor.ConnectionString);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "ConnectionFromConfigFile can not be used when Key is not set")]
        public void GetExecutor_NoKeyProvided_ThrowsInvalidOperationException()
        {
            var conn = new ConnectionFromConfigFile(SqlVendor.Oracle);
            conn.GetExecutor();
        }

        [Test]
        public void GetExecutor_NonExitingKeyButAlternativeGiven_UsesAlternativeConnectionString()
        {
            string alternative = "TheAlternative";
            var conn = new ConnectionFromConfigFile(SqlVendor.Oracle)
                           {
                               Key = "NonExistingKey",
                               AlternativeConnection = alternative
                           };
            var executor = conn.GetExecutor();
            Assert.AreEqual(alternative, executor.ConnectionString);
        }
    }
}
