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
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(37, rangeModifier);
        }

        [TestMethod]
        public void StandardGun2SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 2;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance,isDiagonalDirection);

            //Assert
            Assert.AreEqual(33, rangeModifier);
        }

        [TestMethod]
        public void StandardGun3SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 3;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(28, rangeModifier);
        }

        [TestMethod]
        public void StandardGun4SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 4;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(24, rangeModifier);
        }

        [TestMethod]
        public void StandardGun5SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 5;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(19, rangeModifier);
        }

        [TestMethod]
        public void StandardGun6SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 6;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(15, rangeModifier);
        }

        [TestMethod]
        public void StandardGun7SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 7;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(10, rangeModifier);
        }

        [TestMethod]
        public void StandardGun8SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 8;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(6, rangeModifier);
        }

        [TestMethod]
        public void StandardGun9SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 9;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(1, rangeModifier);
        }

        [TestMethod]
        public void StandardGun10SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 10;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(0, rangeModifier);
        }

        //Should always return 0, no matter the range
        [TestMethod]
        public void UnknownGun1SquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            rifle.Type = WeaponEnum.Unknown;
            int distance = 1;
            bool isDiagonalDirection = false;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(0, rangeModifier);
        }


        [TestMethod]
        public void StandardGun1DiagonalSquareAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 1;
            bool isDiagonalDirection = true;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(35, rangeModifier);
        }

        [TestMethod]
        public void StandardGun2DiagonalSquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 2;
            bool isDiagonalDirection = true;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(29, rangeModifier);
        }

        [TestMethod]
        public void StandardGun3DiagonalSquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 3;
            bool isDiagonalDirection = true;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(23, rangeModifier);
        }

        [TestMethod]
        public void StandardGun4DiagonalSquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 4;
            bool isDiagonalDirection = true;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(16, rangeModifier);
        }

        [TestMethod]
        public void StandardGun5DiagonalSquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 5;
            bool isDiagonalDirection = true;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(10, rangeModifier);
        }

        [TestMethod]
        public void StandardGun6DiagonalSquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 6;
            bool isDiagonalDirection = true;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(4, rangeModifier);
        }

        [TestMethod]
        public void StandardGun7DiagonalSquaresAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();
            int distance = 7;
            bool isDiagonalDirection = true;

            //Act
            int rangeModifier = Range.GetRangeModifier(rifle, distance, isDiagonalDirection);

            //Assert
            Assert.AreEqual(0, rangeModifier);
        }

    }
}