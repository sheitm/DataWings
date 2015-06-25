using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataWings.IO;
using NUnit.Framework;

namespace DataWings.SqlVersions.Tests
{
    [TestFixture]
    public class FileBasedUpdateTests
    {
        [SetUp]
        public void SetUp()
        {
            IoExtensionMethods.Reset();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: path")]
        public void Constructor_PathIsNull_ThrowsArgumentNullException()
        {
            new FileBasedUpdate(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: path")]
        public void Constructor_PathIsEmpty_ThrowsArgumentNullException()
        {
            new FileBasedUpdate(string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "File 00023_DoesNotExist.sql does not exist.")]
        public void Constructor_FileDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var nonExistingFile = "00023_DoesNotExist.sql";
            IoExtensionMethods.FunctionFileExists = f => false;

            // Act
            new FileBasedUpdate(nonExistingFile);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Update script 000001 Missing underscore.sql is missing underscore.")]
        public void Constructor_PathMissingUnderscore_ThrowsArgumentException()
        {
            // Arrange
            var path = @"F:\SomeFolder\000001 Missing underscore.sql";
            IoExtensionMethods.FunctionFileExists = f => true;

            // Act
            new FileBasedUpdate(path);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Unparsable sequence in script NaN.")]
        public void Constructor_UnparsableSequence_ThrowsArgumentException()
        {
            // Arrange
            var path = @"Y:\TestData\Faulty\00002\NaN_UnparsableNumber.sql";
            IoExtensionMethods.FunctionFileExists = f => true;

            // Act
            new FileBasedUpdate(path);
        }

        [Test]
        public void Constructor_ValidPath_ConstructsInstance()
        {
            // Arrange
            var path = @"C:\000001\000001_Inside 1.sql";
            IoExtensionMethods.FunctionFileExists = f => true;

            // Act
            new FileBasedUpdate(path);
        }

        [Test]
        public void Constructor_ValidPath_SetsSequenceIdCorrectly()
        {
            // Arrange
            var path = @"X:\TestData\000001\000004_B.sql";
            IoExtensionMethods.FunctionFileExists = f => true;

            // Act
            var update = new FileBasedUpdate(path);

            // Assert
            Assert.AreEqual(4, update.SequenceId);
        }

        [Test]
        public void Constructor_ValidPath_SetsFolderSequenceIdCorrectly()
        {
            // Arrange
            var path = @"X:\TestData\000007\000004_B.sql";
            IoExtensionMethods.FunctionFileExists = f => true;

            // Act
            var update = new FileBasedUpdate(path);

            // Assert
            Assert.AreEqual(7, update.FolderSequenceId);
        }

        [Test]
        public void Constructor_ValidPathFolderWithUnderscore_SetsFolderSequenceIdCorrectly()
        {
            // Arrange
            var path = @"X:\TestData\000009_MyMagicChange\000004_B.sql";
            IoExtensionMethods.FunctionFileExists = f => true;

            // Act
            var update = new FileBasedUpdate(path);

            // Assert
            Assert.AreEqual(9, update.FolderSequenceId);
        }
    }
}
