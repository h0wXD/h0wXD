﻿using System;
using System.Net;
using h0wXD.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        [TestMethod]
        public void ToIPEndPoint_ValidEndPoint_Success()
        {
            // Arrange
            var expected = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            var inputString = "127.0.0.1:1234";

            // Act
            var actual = inputString.ToIPEndPoint();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToIPEndPoint_InvalidEndPoint_ExceptionThrown()
        {
            // Arrange
            var inputString = "127.0.0.1";

            // Act
            inputString.ToIPEndPoint();
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToIPEndPoint_ValidEndPointInvalidPort_ExceptionThrown()
        {
            // Arrange
            var inputString = "127.0.0.1:123456";

            // Act
            inputString.ToIPEndPoint();
        }
    }
}
