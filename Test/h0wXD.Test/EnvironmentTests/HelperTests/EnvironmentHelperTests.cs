using h0wXD.Environment.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.EnvironmentTests.HelperTests
{
    [TestClass]
    public class EnvironmentHelperTests
    {
        [TestMethod]
        public void GetEnvironmentVariable_ExistingVariable_SomeValueRetrieved()
        {
            // Arrange
            System.Environment.SetEnvironmentVariable("Test", "someValue");

            // Act
            var actual = EnvironmentHelper.GetEnvironmentVariable<string>("Test");

            // Assert
            Assert.AreEqual("someValue", actual);
        }

        [TestMethod]
        public void GetEnvironmentVariable_NonExistingVariable_DefaultValue()
        {
            // Arrange
            System.Environment.SetEnvironmentVariable("Test", null);

            // Act
            var actual = EnvironmentHelper.GetEnvironmentVariable<string>("Test");

            // Assert
            Assert.AreEqual(default(string), actual);
        }

        [TestMethod]
        public void GetEnvironmentVariable_NonExistingVariable_ProvidedDefaultValue()
        {
            // Arrange
            System.Environment.SetEnvironmentVariable("Test", null);

            // Act
            var actual = EnvironmentHelper.GetEnvironmentVariable("Test", "works");

            // Assert
            Assert.AreEqual("works", actual);
        }

        [TestMethod]
        public void GetEnvironmentVariable_NonExistingVariable_IntegerValue()
        {
            // Arrange
            System.Environment.SetEnvironmentVariable("Test", "123");

            // Act
            var actual = EnvironmentHelper.GetEnvironmentVariable<int>("Test");

            // Assert
            Assert.AreEqual(123, actual);
        }
    }
}
