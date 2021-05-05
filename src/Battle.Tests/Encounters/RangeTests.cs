using Battle.Logic.Encounters;
using Battle.Logic.Weapons;
using Battle.Tests.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Encounters
{
    [TestClass]
    [TestCategory("L0")]
    public class RangeTests
    {
        [TestMethod]
        public void StandardGun1SquareAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 1;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(37, rangeModifier);
        }

        [TestMethod]
        public void StandardGun2SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 2;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(33, rangeModifier);
        }

        [TestMethod]
        public void StandardGun3SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 3;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(28, rangeModifier);
        }

        [TestMethod]
        public void StandardGun4SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 4;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(24, rangeModifier);
        }

        [TestMethod]
        public void StandardGun5SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 5;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(19, rangeModifier);
        }

        [TestMethod]
        public void StandardGun6SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 6;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(15, rangeModifier);
        }

        [TestMethod]
        public void StandardGun7SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 7;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(10, rangeModifier);
        }

        [TestMethod]
        public void StandardGun8SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 8;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(6, rangeModifier);
        }

        [TestMethod]
        public void StandardGun9SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 9;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(1, rangeModifier);
        }

        [TestMethod]
        public void StandardGun10SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 10;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance);

            //Assert
            Assert.AreEqual(0, rangeModifier);
        }

    }
}