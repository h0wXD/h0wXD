using System;
using h0wXD.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.HelperTests
{
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void StartsWith_StringStartingWithTestLowerCase_TrueReturned()
        {
            // Arrange
            var someString = "testString";

            // Act
            var result = someString.StartsWith("test", 4, StringComparison.InvariantCulture);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StartsWith_StringStartingPartiallyWithTestLowerCase_TrueReturned()
        {
            // Arrange
            var someString = "testString";

            // Act
            var result = someString.StartsWith("testString", 3, StringComparison.InvariantCulture);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StartsWith_StringStartingPartiallyWithTestUpperCase_FalseReturned()
        {
            // Arrange
            var someString = "testString";

            // Act
            var result = someString.StartsWith("TEStString", 3, StringComparison.InvariantCulture);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void StartsWith_StringStartingPartiallyWithTestUpperCaseIgnoreCase_TrueReturned()
        {
            // Arrange
            var someString = "testString";

            // Act
            var result = someString.StartsWith("TEStString", 3, StringComparison.InvariantCultureIgnoreCase);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StartsWith_StringWithLongerLength_TrueReturned()
        {
            // Arrange
            var someString = "testString";

            // Act
            var result = someString.StartsWith("TEStString", 50, StringComparison.InvariantCultureIgnoreCase);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Prepend_StringNotStartingWith_PrependedString()
        {
            // Arrange
            var someString = "PrependTest";

            // Act
            var actual = someString.Prepend("test");

            // Assert
            Assert.AreEqual("testPrependTest", actual);
        }

        [TestMethod]
        public void Prepend_StringStartingWith_SameString()
        {
            // Arrange
            var someString = "testPrependTest";

            // Act
            var actual = someString.Prepend("test");

            // Assert
            Assert.AreEqual(someString, actual);
        }

        [TestMethod]
        public void Prepend_StringStartingWithDifferentCase_PrependedString()
        {
            // Arrange
            var someString = "testPrependTest";

            // Act
            var actual = someString.Prepend("Test");

            // Assert
            Assert.AreEqual("TesttestPrependTest", actual);
        }

        [TestMethod]
        public void Prepend_StringStartingWithIgnoreCase_SameString()
        {
            // Arrange
            var someString = "testPrependTest";

            // Act
            var actual = someString.Prepend("Test", StringComparison.CurrentCultureIgnoreCase);

            // Assert
            Assert.AreEqual(someString, actual);
        }

        [TestMethod]
        public void Append_StringNotEndingWith_AppendedString()
        {
            // Arrange
            var someString = "Append";

            // Act
            var actual = someString.Append("Test");

            // Assert
            Assert.AreEqual("AppendTest", actual);
        }

        [TestMethod]
        public void Append_StringEndingWith_SameString()
        {
            // Arrange
            var someString = "AppendTest";

            // Act
            var actual = someString.Append("Test");

            // Assert
            Assert.AreEqual(someString, actual);
        }

        [TestMethod]
        public void Append_StringEndingWithDifferentCase_AppendedString()
        {
            // Arrange
            var someString = "testAppendtest";

            // Act
            var actual = someString.Append("Test");

            // Assert
            Assert.AreEqual("testAppendtestTest", actual);
        }

        [TestMethod]
        public void Append_StringEndingWithIgnoreCase_SameString()
        {
            // Arrange
            var someString = "testAppendtest";

            // Act
            var actual = someString.Prepend("Test", StringComparison.CurrentCultureIgnoreCase);

            // Assert
            Assert.AreEqual(someString, actual);
        }

        [TestMethod]
        public void Wrap_StringNotWrappedWith_WrappedWith()
        {
            // Arrange
            var someString = "Wrapped";

            // Act
            var actual = someString.Wrap("Test");

            // Assert
            Assert.AreEqual("TestWrappedTest", actual);
        }

        [TestMethod]
        public void Wrap_StringWrappedWith_SameString()
        {
            // Arrange
            var someString = "TestWrappedTest";

            // Act
            var actual = someString.Wrap("Test");

            // Assert
            Assert.AreEqual(someString, actual);
        }

        [TestMethod]
        public void Wrap_StringNotWrappedWithDifferentStrings_WrappedString()
        {
            // Arrange
            var someString = "Wrapped";

            // Act
            var actual = someString.Wrap("Prepend", "Append");

            // Assert
            Assert.AreEqual("PrependWrappedAppend", actual);
        }

        [TestMethod]
        public void Wrap_StringNotWrappedWithDifferentStrings_SameString()
        {
            // Arrange
            var someString = "PrependWrappedAppend";

            // Act
            var actual = someString.Wrap("Prepend", "Append");

            // Assert
            Assert.AreEqual(someString, actual);
        }

        [TestMethod]
        public void Strip_StringWrappedWith_StrippedString()
        {
            // Arrange
            var someString = "TestWrappedTest";

            // Act
            var actual = someString.Strip("Test");

            // Assert
            Assert.AreEqual("Wrapped", actual);
        }

        [TestMethod]
        public void Strip_StringWrappedWithIgnoreCase_StrippedString()
        {
            // Arrange
            var someString = "testWrappedtest";

            // Act
            var actual = someString.Strip("TEST", StringComparison.CurrentCultureIgnoreCase);

            // Assert
            Assert.AreEqual("Wrapped", actual);
        }

        [TestMethod]
        public void Strip_StringNotWrappedWith_SameString()
        {
            // Arrange
            var someString = "Wrapped";

            // Act
            var actual = someString.Strip("Test");

            // Assert
            Assert.AreEqual(someString, actual);
        }
    }
}
