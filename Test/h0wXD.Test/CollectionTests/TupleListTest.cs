using System;
using h0wXD.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace h0wXD.Test.CollectionTests
{
    [TestClass]
    public class TupleListTest
    {
        [TestMethod]
        public void Initializer_IntegerArrayOfSize2_TupleInitializedWithArray()
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
        public void Initializer_MixedArrayOfSize3_TupleInitializedWithArray()
        {
            var tupleList = new TupleList<int, string, int>()
            {
                {10, "one", 6},
                {7, "two", 6},
            };

            Assert.AreEqual(new Tuple<int, string, int>(10, "one", 6), tupleList[0]);
            Assert.AreEqual(new Tuple<int, string, int>(7, "two", 6), tupleList[1]);
        }

        [TestMethod]
        public void Initializer_MixedArrayOfSize4_TupleInitializedWithArray()
        {
            var tupleList = new TupleList<int, string, int, string>()
            {
                {10, "one", 6, "two"},
                {7, "three", 6, "four"},
            };

            Assert.AreEqual(new Tuple<int, string, int, string>(10, "one", 6, "two"), tupleList[0]);
            Assert.AreEqual(new Tuple<int, string, int, string>(7, "three", 6, "four"), tupleList[1]);
        }

        [TestMethod]
        public void Initializer_MixedArrayOfSize5_TupleInitializedWithArray()
        {
            var tupleList = new TupleList<int, string, int, string, int>()
            {
                {10, "one", 6, "two", 8},
                {7, "three", 6, "four", 9},
            };

            Assert.AreEqual(new Tuple<int, string, int, string, int>(10, "one", 6, "two", 8), tupleList[0]);
            Assert.AreEqual(new Tuple<int, string, int, string, int>(7, "three", 6, "four", 9), tupleList[1]);
        }
    }
}
