using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Common.Test
{
    [TestClass]
    public class ExtensionMethodsTest
    {
        [TestMethod]
        public void ToIPEndPoint_ValidEndPoint_Success()
        {
            var sInputString = "127.0.0.1:1234";

            var expected = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            var actual = sInputString.ToIPEndPoint();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToIPEndPoint_InvalidEndPoint_ExceptionThrown()
        {
            var sInputString = "127.0.0.1";

            sInputString.ToIPEndPoint();
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToIPEndPoint_ValidEndPointInvalidPort_ExceptionThrown()
        {
            var sInputString = "127.0.0.1:123456";

            sInputString.ToIPEndPoint();
        }
    }
}
