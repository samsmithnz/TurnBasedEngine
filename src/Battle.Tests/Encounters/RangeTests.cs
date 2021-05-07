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
        public void StandardWeapon1SquareAwayRangeTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();

            //Act
            int rangeModifier1 = Range.GetRangeModifier(rifle, 1);
            int rangeModifier2 = Range.GetRangeModifier(rifle, 2);
            int rangeModifier3 = Range.GetRangeModifier(rifle, 3);
            int rangeModifier4 = Range.GetRangeModifier(rifle, 4);
            int rangeModifier5 = Range.GetRangeModifier(rifle, 5);
            int rangeModifier6 = Range.GetRangeModifier(rifle, 6);
            int rangeModifier7 = Range.GetRangeModifier(rifle, 7);
            int rangeModifier8 = Range.GetRangeModifier(rifle, 8);
            int rangeModifier9 = Range.GetRangeModifier(rifle, 9);
            int rangeModifier10 = Range.GetRangeModifier(rifle, 10);
            int rangeModifier11 = Range.GetRangeModifier(rifle, 11);
            int rangeModifier12 = Range.GetRangeModifier(rifle, 12);
            int rangeModifier13 = Range.GetRangeModifier(rifle, 13);
            int rangeModifier14 = Range.GetRangeModifier(rifle, 14);
            int rangeModifier15 = Range.GetRangeModifier(rifle, 15);
            int rangeModifier16 = Range.GetRangeModifier(rifle, 16);
            int rangeModifier17 = Range.GetRangeModifier(rifle, 17);
            int rangeModifier18 = Range.GetRangeModifier(rifle, 18);
            int rangeModifier19 = Range.GetRangeModifier(rifle, 19);
            int rangeModifier20 = Range.GetRangeModifier(rifle, 20);

            //Assert
            Assert.AreEqual(37, rangeModifier1);
            Assert.AreEqual(33, rangeModifier2);
            Assert.AreEqual(28, rangeModifier3);
            Assert.AreEqual(24, rangeModifier4);
            Assert.AreEqual(19, rangeModifier5);
            Assert.AreEqual(15, rangeModifier6);
            Assert.AreEqual(10, rangeModifier7);
            Assert.AreEqual(6, rangeModifier8);
            Assert.AreEqual(1, rangeModifier9);
            Assert.AreEqual(0, rangeModifier10);
            Assert.AreEqual(0, rangeModifier11);
            Assert.AreEqual(0, rangeModifier12);
            Assert.AreEqual(0, rangeModifier13);
            Assert.AreEqual(0, rangeModifier14);
            Assert.AreEqual(0, rangeModifier15);
            Assert.AreEqual(0, rangeModifier16);
            Assert.AreEqual(0, rangeModifier17);
            Assert.AreEqual(0, rangeModifier18);
            Assert.AreEqual(0, rangeModifier19);
            Assert.AreEqual(0, rangeModifier20);
        }

        [TestMethod]
        public void ShotgunRangeTest()
        {
            //Arrange
            Weapon shotgun = WeaponPool.CreateShotgun();

            //Act
            int rangeModifier1 = Range.GetRangeModifier(shotgun, 1);
            int rangeModifier2 = Range.GetRangeModifier(shotgun, 2);
            int rangeModifier3 = Range.GetRangeModifier(shotgun, 3);
            int rangeModifier4 = Range.GetRangeModifier(shotgun, 4);
            int rangeModifier5 = Range.GetRangeModifier(shotgun, 5);
            int rangeModifier6 = Range.GetRangeModifier(shotgun, 6);
            int rangeModifier7 = Range.GetRangeModifier(shotgun,7);
            int rangeModifier8 = Range.GetRangeModifier(shotgun,8);
            int rangeModifier9 = Range.GetRangeModifier(shotgun, 9);
            int rangeModifier10 = Range.GetRangeModifier(shotgun, 10);
            int rangeModifier11 = Range.GetRangeModifier(shotgun, 11);
            int rangeModifier12 = Range.GetRangeModifier(shotgun, 12);
            int rangeModifier13 = Range.GetRangeModifier(shotgun, 13);
            int rangeModifier14 = Range.GetRangeModifier(shotgun, 14);
            int rangeModifier15 = Range.GetRangeModifier(shotgun, 15);
            int rangeModifier16 = Range.GetRangeModifier(shotgun, 16);
            int rangeModifier17 = Range.GetRangeModifier(shotgun, 17);
            int rangeModifier18 = Range.GetRangeModifier(shotgun, 18);
            int rangeModifier19 = Range.GetRangeModifier(shotgun, 19);
            int rangeModifier20 = Range.GetRangeModifier(shotgun, 20);


            //Assert
            Assert.AreEqual(52, rangeModifier1);
            Assert.AreEqual(44, rangeModifier2);
            Assert.AreEqual(40, rangeModifier3);
            Assert.AreEqual(36, rangeModifier4);
            Assert.AreEqual(28, rangeModifier5);
            Assert.AreEqual(24, rangeModifier6);
            Assert.AreEqual(16, rangeModifier7);
            Assert.AreEqual(12, rangeModifier8);
            Assert.AreEqual(4, rangeModifier9);
            Assert.AreEqual(0, rangeModifier10);
            Assert.AreEqual(-8, rangeModifier11);
            Assert.AreEqual(-12, rangeModifier12);
            Assert.AreEqual(-20, rangeModifier13);
            Assert.AreEqual(-24, rangeModifier14);
            Assert.AreEqual(-32, rangeModifier15);
            Assert.AreEqual(-40, rangeModifier16);
            Assert.AreEqual(-40, rangeModifier17);
            Assert.AreEqual(0, rangeModifier18);
            Assert.AreEqual(0, rangeModifier19);
            Assert.AreEqual(0, rangeModifier20);
        }

        [TestMethod]
        public void SniperRifle1SquareAwayRangeTest()
        {
            //Arrange
            Weapon sniperRifle = WeaponPool.CreateSniperRifle();

            //Act
            int rangeModifier1 = Range.GetRangeModifier(sniperRifle, 1);
            int rangeModifier2 = Range.GetRangeModifier(sniperRifle, 2);
            int rangeModifier3 = Range.GetRangeModifier(sniperRifle, 3);
            int rangeModifier4 = Range.GetRangeModifier(sniperRifle, 4);
            int rangeModifier5 = Range.GetRangeModifier(sniperRifle, 5);
            int rangeModifier6 = Range.GetRangeModifier(sniperRifle, 6);
            int rangeModifier7 = Range.GetRangeModifier(sniperRifle, 7);
            int rangeModifier8 = Range.GetRangeModifier(sniperRifle, 8);
            int rangeModifier9 = Range.GetRangeModifier(sniperRifle, 9);
            int rangeModifier10 = Range.GetRangeModifier(sniperRifle, 10);
            int rangeModifier11 = Range.GetRangeModifier(sniperRifle, 11);
            int rangeModifier12 = Range.GetRangeModifier(sniperRifle, 12);
            int rangeModifier13 = Range.GetRangeModifier(sniperRifle, 13);
            int rangeModifier14 = Range.GetRangeModifier(sniperRifle, 14);
            int rangeModifier15 = Range.GetRangeModifier(sniperRifle, 15);
            int rangeModifier16 = Range.GetRangeModifier(sniperRifle, 16);
            int rangeModifier17 = Range.GetRangeModifier(sniperRifle, 17);
            int rangeModifier18 = Range.GetRangeModifier(sniperRifle, 18);
            int rangeModifier19 = Range.GetRangeModifier(sniperRifle, 19);
            int rangeModifier20 = Range.GetRangeModifier(sniperRifle, 20);

            //Assert
            Assert.AreEqual(-24, rangeModifier1);
            Assert.AreEqual(-21, rangeModifier2);
            Assert.AreEqual(-19, rangeModifier3);
            Assert.AreEqual(-16, rangeModifier4);
            Assert.AreEqual(-13, rangeModifier5);
            Assert.AreEqual(-10, rangeModifier6);
            Assert.AreEqual(-7, rangeModifier7);
            Assert.AreEqual(-4, rangeModifier8);
            Assert.AreEqual(-1, rangeModifier9);
            Assert.AreEqual(0, rangeModifier10);
            Assert.AreEqual(0, rangeModifier11);
            Assert.AreEqual(0, rangeModifier12);
            Assert.AreEqual(0, rangeModifier13);
            Assert.AreEqual(0, rangeModifier14);
            Assert.AreEqual(0, rangeModifier15);
            Assert.AreEqual(0, rangeModifier16);
            Assert.AreEqual(0, rangeModifier17);
            Assert.AreEqual(0, rangeModifier18);
            Assert.AreEqual(0, rangeModifier19);
            Assert.AreEqual(0, rangeModifier20);

        }

    }
}