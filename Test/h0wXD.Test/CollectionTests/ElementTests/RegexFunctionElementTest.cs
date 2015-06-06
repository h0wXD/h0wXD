using System.Collections.Generic;
using System.Text.RegularExpressions;
using h0wXD.Collections.Elements;
using h0wXD.Collections.Elements.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.CollectionTests.ElementTests
{
    [TestClass]
    public class RegexFunctionElementTest
    {
        [TestMethod]
        public void Constructor_RegexWithStringGroups_GroupValue()
        {
            // Arrange
            var someText = "";
            var element = new RegexFunctionElement(@"(?<numbers>([0-9])\w+)", RegexOptions.IgnoreCase, "numbers");

            // Act
            element.Match("test 123 some 456 test");
            var keys = element.Keys;
            var defaultKeyValue = element.Value;

            // Assert
            Assert.AreEqual(3, element.Count);
            Assert.AreEqual(3, keys.Length);
            Assert.AreEqual(@"(?<numbers>([0-9])\w+)", element.Regex.ToString());
            Assert.AreEqual("123", defaultKeyValue);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Constructor_InvalidKey_ExceptionThrown()
        {
            new RegexFunctionElement(@"(?<numbers>([0-9])\w+)", RegexOptions.IgnoreCase, "something");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Constructor_NullKey_ExceptionThrown()
        {
            new RegexFunctionElement(@"(?<numbers>([0-9])\w+)", RegexOptions.IgnoreCase, null);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Constructor_InvalidIndex_ExceptionThrown()
        {
            new RegexFunctionElement(@"(?<numbers>([0-9])\w+)", RegexOptions.IgnoreCase, 10);
        }

        [TestMethod]
        public void BracketOperator_RegexWithStringGroups_GroupValue()
        {
            // Arrange
            var someText = "";
            var element = new RegexFunctionElement(@"(?<numbers>([0-9])\w+)", RegexOptions.IgnoreCase, "numbers");
            var interfaceElement = (IRegexFunctionElement)element;

            // Act
            element.Match("test 123 some 456 test");
            var actual1 = element["numbers"];
            var actual2 = interfaceElement["numbers"];

            // Assert
            Assert.AreEqual("123", actual1);
            Assert.AreEqual("123", actual2);
        }

        [TestMethod]
        public void BracketOperator_RegexWithIntegerGroups_GroupValue()
        {
            // Arrange
            var someText = "";
            var element = new RegexFunctionElement(@"\s([0-9]*)\s");
            var interfaceElement = (IRegexFunctionElement)element;

            // Act
            element.Match("test 123 some 456 test");
            var actual1 = element[1];
            var actual2 = interfaceElement[1];

            // Assert
            Assert.AreEqual("123", actual1);
            Assert.AreEqual("123", actual2);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void BracketOperator_InvalidKey_ExceptionThrown()
        {
            // Arrange
            var element = new RegexFunctionElement(@"(?<numbers>([0-9])\w+)", RegexOptions.IgnoreCase, "numbers");
            var interfaceElement = (IRegexFunctionElement)element;

            // Act
            var actual = interfaceElement["anotherkey"];
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void BracketOperator_InvalidIndex_ExceptionThrown()
        {
            // Arrange
            var element = new RegexFunctionElement(@"(?<numbers>([0-9])\w+)", RegexOptions.IgnoreCase, 1);
            var interfaceElement = (IRegexFunctionElement)element;

            // Act
            var actual = interfaceElement[7];
        }
    }
}