using System;
using System.IO;
using NUnit.Framework;
using DataWings.Common;

namespace DataWings.Tests
{
    [TestFixture]
    public class ConnectionFromFileTests
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "ConnectionFromFile can not be used when FilePath is not set")]
        public void GetExecutor_FileNameNotGivenAndNoALternative_ThrowsInvalidOperationException()
        {
            var conn = new ConnectionFromFile(SqlVendor.SqlServer);
            conn.GetExecutor();
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException), ExpectedMessage = @"C:\DoesNotExist.txt not found")]
        public void GetExecutor_FileDoesNotExistAndNoAlternative_ThrowsFileNotFoundException()
        {
            var conn = new ConnectionFromFile(SqlVendor.SqlServer)
                           {
                               FilePath = @"C:\DoesNotExist.txt"
                           };
            conn.GetExecutor();
        }

        [Test]
        public void GetExecutor_FileDoesNotExistButAlternativeIsGiven_UsesAlternativeConnection()
        {
            string alternative = "TheAlternative";
            var conn = new ConnectionFromFile(SqlVendor.SqlServer)
            {
                FilePath = @"C:\DoesNotExist.txt",
                AlternativeConnection = alternative
            };
            var executor = conn.GetExecutor();
            
            Assert.AreEqual(alternative, executor.ConnectionString);
        }

        [Test]
        public void GetExecutor_FileExists_UsesConnectionFromFile()
        {
            string path = @"..\..\MyConnection.txt";
            var conn = new ConnectionFromFile(SqlVendor.SqlServer)
            {
                FilePath = path
            };

            var executor = conn.GetExecutor();

            Assert.AreEqual("MyConnection", executor.ConnectionString);
        }
    }
}
