using System;
using h0wXD.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.Collections
{
    [TestClass]
    public class TupleListTest
    {
        [TestMethod]
        public void Initializer_TwoTypes_AreEqual()
        {
            var tupleList = new TupleList<int, int>()
            {
                {10, 15},
                {7, 11},
            };

            Assert.AreEqual(new Tuple<int, int>(10, 15), tupleList[0]);
            Assert.AreEqual(new Tuple<int, int>(7, 11), tupleList[1]);
        }

        [TestMethod]
        public void Initializer_ThreeTypes_AreEqual()
        {
            var tupleList = new TupleList<int, string, int>()
            {
                {10, "one", 6},
                {7, "two", 6},
            };

            Assert.AreEqual(new Tuple<int, string, int>(10, "one", 6), tupleList[0]);
            Assert.AreEqual(new Tuple<int, string, int>(7, "two", 6), tupleList[1]);
        }
    }
}
