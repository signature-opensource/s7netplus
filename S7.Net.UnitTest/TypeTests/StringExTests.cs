﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using S7.Net.Types;
using System;
using System.Collections;
using System.Linq;

namespace S7.Net.UnitTest.TypeTests
{
    [TestClass]
    public class StringExTests
    {
        [TestMethod]
        public void ReadEmptyStringWithZeroByteLength()
        {
            AssertFromByteArrayEquals("", 0, 0);
        }

        [TestMethod]
        public void ReadEmptyStringWithOneByteLength()
        {
            AssertFromByteArrayEquals("", 1, 0, 0);
        }

        [TestMethod]
        public void ReadEmptyStringWithOneByteGarbage()
        {
            AssertFromByteArrayEquals("", 1, 0, (byte) 'a');
        }

        [TestMethod]
        public void ReadMalformedStringTooShort()
        {
            Assert.ThrowsException<PlcException>(() => AssertFromByteArrayEquals("", 1));
        }

        [TestMethod]
        public void ReadMalformedStringSizeLargerThanCapacity()
        {
            Assert.ThrowsException<PlcException>(() => StringEx.FromByteArray(new byte[] { 3, 5, 0, 1, 2 }));
        }

        [TestMethod]
        public void ReadMalformedStringCapacityTooLarge()
        {
            Assert.ThrowsException<ArgumentException>(() => AssertToByteArrayAndBackEquals("", 300, 0));
        }

        [TestMethod]
        public void ReadA()
        {
            AssertFromByteArrayEquals("A", 1, 1, (byte) 'A');
        }

        [TestMethod]
        public void ReadAbc()
        {
            AssertFromByteArrayEquals("Abc", 3, 3, (byte) 'A', (byte) 'b', (byte) 'c');
        }

        [TestMethod]
        public void WriteNullWithReservedLengthZero()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertToByteArrayAndBackEquals(null, 0, 0, 0));
        }

        [TestMethod]
        public void WriteEmptyStringWithReservedLengthZero()
        {
            AssertToByteArrayAndBackEquals("", 0, 0, 0);
        }

        [TestMethod]
        public void WriteAWithReservedLengthZero()
        {
            AssertToByteArrayAndBackEquals("", 0, 0, 0);
        }

        [TestMethod]
        public void WriteNullWithReservedLengthOne()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AssertToByteArrayAndBackEquals(null, 1, 1, 0));
        }

        [TestMethod]
        public void WriteEmptyStringWithReservedLengthOne()
        {
            AssertToByteArrayAndBackEquals("", 1, 1, 0, 0);
        }

        [TestMethod]
        public void WriteAWithReservedLengthOne()
        {
            AssertToByteArrayAndBackEquals("A", 1, 1, 1, (byte) 'A');
        }

        [TestMethod]
        public void WriteAWithReservedLengthTwo()
        {
            AssertToByteArrayAndBackEquals("A", 2, 2, 1, (byte) 'A', 0);
        }

        [TestMethod]
        public void WriteAbcWithStringLargetThanReservedLength()
        {
            Assert.ThrowsException<ArgumentException>(() => StringEx.ToByteArray("Abc", 2));
        }

        [TestMethod]
        public void WriteAbcWithReservedLengthThree()
        {
            AssertToByteArrayAndBackEquals("Abc", 3, 3, 3, (byte) 'A', (byte) 'b', (byte) 'c');
        }

        [TestMethod]
        public void WriteAbcWithReservedLengthFour()
        {
            AssertToByteArrayAndBackEquals("Abc", 4, 4, 3, (byte) 'A', (byte) 'b', (byte) 'c', 0);
        }

        private static void AssertFromByteArrayEquals(string expected, params byte[] bytes)
        {
            var convertedString = StringEx.FromByteArray(bytes);
            Assert.AreEqual(expected, convertedString);
        }


        private static void AssertToByteArrayAndBackEquals(string value, int reservedLength, params byte[] expected)
        {
            var convertedData = StringEx.ToByteArray(value, reservedLength);
            CollectionAssert.AreEqual(expected, convertedData);
            var convertedBack = StringEx.FromByteArray(convertedData);
            Assert.AreEqual(value, convertedBack);
        }
    }
}
