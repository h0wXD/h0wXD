using System.Collections.Generic;
using h0wXD.Collections;
using h0wXD.Collections.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.CollectionTests
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void BracketOperator_Dictionary_ExceptionThrown()
        {
            // Arrange
            var dictionary = new Dictionary<int, int>();

            // Act
            var result = dictionary[10];
        }

        [TestMethod]
        public void BracketOperator_MapInterface_DefaultIntegerValue()
        {
            // Arrange
            IMap<int, int> dictionary = new Map<int, int>();

            // Act
            var result = dictionary[10];

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void BracketOperator_Map_DefaultIntegerValue()
        {
            // Arrange
            var dictionary = new Map<int, int>();

            // Act
            var result = dictionary[10];

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void BracketOperator_Map_DefaultStringValue()
        {
            // Arrange
            var dictionary = new Map<int, string>();

            // Act
            var result = dictionary[10];

            // Assert
            Assert.AreEqual(default(string), result);
        }

        [TestMethod]
        public void BracketOperator_TestString_DefaultIntegerValue()
        {
            // Arrange
            IMap<int, string> dictionary = new Map<int, string>();

            // Act
            dictionary[10] = "test";
            var result = dictionary[10];

            // Assert
            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void BracketOperator_AnInteger_SameValue()
        {
            // Arrange
            var dictionary = new Map<int, int>();

            // Act
            dictionary[10] = 123;
            var result = dictionary[10];

            // Assert
            Assert.AreEqual(123, result);
        }
    }
}