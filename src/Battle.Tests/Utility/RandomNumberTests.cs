using Battle.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battle.Tests.Utility
{
    [TestClass]
    [TestCategory("L0")]
    public class RandomNumberTests
    {
        [TestMethod]
        public void RandomNumberWithSeedZeroTest()
        {
            //Arrange
            int minValue = 1;
            int maxValue = 10;
            int seed = 0;

            //Act
            int result = RandomNumber.GenerateRandomNumber(minValue, maxValue, seed);

            //Assert
            Assert.IsTrue(ValueIsInRange(result, minValue, maxValue));
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void RandomNumberWithSeedOneTest()
        {
            //Arrange
            int minValue = 1;
            int maxValue = 10;
            int seed = 1;

            //Act
            int result = RandomNumber.GenerateRandomNumber(minValue, maxValue, seed);

            //Assert

            Assert.IsTrue(ValueIsInRange(result, minValue, maxValue));
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void RandomNumberListWithSeedZeroTest()
        {
            //Arrange
            int minValue = 1;
            int maxValue = 10;
            int listLength = 10;
            int seed = 0;

            //Act
            List<int> result = RandomNumber.GenerateRandomNumberList(minValue, maxValue, seed, listLength);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(10, result.Count);
            Assert.IsTrue(ValueIsInRange(result[0], minValue, maxValue));

            Assert.IsTrue(ValueIsInRange(result[9], minValue, maxValue)); Assert.AreEqual(7, result[0]);
            Assert.IsTrue(ValueIsInRange(result[9], minValue, maxValue));
            Assert.AreEqual(3, result[9]);
        }

        [TestMethod]
        public void RandomNumberListWithOneItemSeedZeroTest()
        {
            //Arrange
            int minValue = 0;
            int maxValue = 100;
            int listLength = 1;
            int seed = 0;

            //Act
            List<int> result = RandomNumber.GenerateRandomNumberList(minValue, maxValue, seed, listLength);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(ValueIsInRange(result[0], minValue, maxValue));
            Assert.AreEqual(72, result[0]);
        }

        private static bool ValueIsInRange(int value, int minValue, int maxValue)
        {
            return (value >= minValue & value <= maxValue);
        }
    }
}