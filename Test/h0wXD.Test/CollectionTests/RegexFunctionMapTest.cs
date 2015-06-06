using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using h0wXD.Collections;
using h0wXD.Collections.Elements;
using h0wXD.Collections.Elements.Interfaces;
using h0wXD.Collections.Interfaces;
using h0wXD.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.CollectionTests
{
    [TestClass]
    public class RegexFunctionMapTest
    {
        private enum Identifiers
        {
            Numbers,
            Text
        }

        private IRegexFunctionMap<Identifiers> _regexFunctionMap;

        [TestMethod]
        public void Initializer_TwoRegularExpressions_RegularExpressionsAdded()
        {
            // Arrange

            // Act
            _regexFunctionMap = new RegexFunctionMap<Identifiers>()
            {
                {Identifiers.Numbers, new RegexFunctionElement(@"([0-9])\w+")},
                {Identifiers.Text, @"([a-z])\w+", RegexOptions.IgnoreCase},
            };

            // Assert
            Assert.AreEqual(2, _regexFunctionMap.Count());
        }

        [TestMethod]
        public void Clear_TwoRegularExpressions_Empty()
        {
            // Arrange
            _regexFunctionMap = new RegexFunctionMap<Identifiers>()
            {
                {Identifiers.Numbers, new RegexFunctionElement(@"([0-9])\w+")},
                {Identifiers.Text, @"([a-z])\w+", RegexOptions.IgnoreCase},
            };

            // Act
            _regexFunctionMap.Clear();

            // Assert
            Assert.AreEqual(0, _regexFunctionMap.Count());
        }

        [TestMethod]
        public void Match_TwoRegularExpressions_EqualToInput()
        {
            // Arrange
            var text = "some text 123456";
            _regexFunctionMap = new RegexFunctionMap<Identifiers>()
            {
                {Identifiers.Numbers, new RegexFunctionElement(@"([0-9]*)$")},
                {Identifiers.Text, @"([a-z\s]*)", RegexOptions.IgnoreCase},
            };

            // Act
            _regexFunctionMap.Match(text);
            var actual1 = _regexFunctionMap[Identifiers.Numbers];
            var actual2 = _regexFunctionMap[Identifiers.Text].Trim();

            // Assert
            Assert.AreEqual("123456", actual1);
            Assert.AreEqual("some text", actual2);
        }

        [TestMethod]
        public void At_Identifier_RegexFunctionElementReturned()
        {
            // Arrange
            _regexFunctionMap = new RegexFunctionMap<Identifiers>()
            {
                {Identifiers.Numbers, new RegexFunctionElement(@"([0-9])\w+")},
                {Identifiers.Text, @"([a-z\s]*)", RegexOptions.IgnoreCase},
            };

            // Act
            var actual = _regexFunctionMap.At(Identifiers.Numbers);

            // Assert
            Assert.AreEqual(@"([0-9])\w+", actual.Regex.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void BracketOperator_Empty_ExceptionThrown()
        {
            // Arrange
            _regexFunctionMap = new RegexFunctionMap<Identifiers>();

            // Act
            var result = _regexFunctionMap[Identifiers.Numbers];
        }

        [TestMethod]
        public void GetEnumerator_TwoElements_EnumeratesTwice()
        {
            // Arrange
            var count = 0;
            _regexFunctionMap = new RegexFunctionMap<Identifiers>()
            {
                {Identifiers.Numbers, new RegexFunctionElement(@"([0-9])\w+")},
                {Identifiers.Text, @"([a-z\s]*)", RegexOptions.IgnoreCase},
            };
            var weakEnumerable = _regexFunctionMap.AsWeakEnumerable();

            // Act
            count = weakEnumerable.Cast<KeyValuePair<Identifiers, IRegexFunctionElement>>().Count();

            // Assert
            Assert.AreEqual(2, count);
        }
    }
}