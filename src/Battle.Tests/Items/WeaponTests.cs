using Battle.Logic.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Weapons
{
    [TestClass]
    [TestCategory("L0")]
    public class WeaponTests
    {
        [TestMethod]
        public void WeaponRifleTest()
        {
            //Arrange
            Weapon rifle = WeaponPool.CreateRifle();

            //Act            

            //Assert
            TestRifle(rifle);
        }

        [TestMethod]
        public void WeaponShotgunTest()
        {
            //Arrange
            Weapon shotgun = WeaponPool.CreateShotgun();

            //Act            

            //Assert
            TestShotgun(shotgun);
        }

        [TestMethod]
        public void WeaponSniperRifleTest()
        {
            //Arrange
            Weapon sniperRifle = WeaponPool.CreateSniperRifle();

            //Act            

            //Assert
            TestSniperRifle(sniperRifle);
        }

        [TestMethod]
        public void WeaponGrenadeTest()
        {
            //Arrange
            Weapon grenade = WeaponPool.CreateGrenade();

            //Act            

            //Assert
            TestGrenade(grenade);
        }

        private static void TestRifle(Weapon weapon)
        {
            Assert.IsNotNull(weapon);
            Assert.AreEqual("Rifle", weapon.Name);
            Assert.AreEqual(10, weapon.ChanceToHitAdjustment);
            Assert.AreEqual(18, weapon.Range);
            Assert.AreEqual(0, weapon.AreaEffectRadius);
            Assert.AreEqual(3, weapon.DamageRangeLow);
            Assert.AreEqual(5, weapon.DamageRangeHigh);
            Assert.AreEqual(20, weapon.CriticalChance);
            Assert.AreEqual(5, weapon.CriticalDamageLow);
            Assert.AreEqual(7, weapon.CriticalDamageHigh);
            Assert.AreEqual(4, weapon.ClipSize);
            Assert.AreEqual(1, weapon.ActionPointsRequired);
            Assert.AreEqual(WeaponEnum.Standard, weapon.Type);
        }

        private static void TestShotgun(Weapon weapon)
        {
            Assert.IsNotNull(weapon);
            Assert.AreEqual("Shotgun", weapon.Name);
            Assert.AreEqual(10, weapon.ChanceToHitAdjustment);
            Assert.AreEqual(17, weapon.Range);
            Assert.AreEqual(0, weapon.AreaEffectRadius);
            Assert.AreEqual(3, weapon.DamageRangeLow);
            Assert.AreEqual(5, weapon.DamageRangeHigh);
            Assert.AreEqual(20, weapon.CriticalChance);
            Assert.AreEqual(6, weapon.CriticalDamageLow);
            Assert.AreEqual(8, weapon.CriticalDamageHigh);
            Assert.AreEqual(4, weapon.ClipSize);
            Assert.AreEqual(1, weapon.ActionPointsRequired);
            Assert.AreEqual(WeaponEnum.Shotgun, weapon.Type);
        }
        
        private static void TestSniperRifle(Weapon weapon)
        {
            Assert.IsNotNull(weapon);
            Assert.AreEqual("Sniper Rifle", weapon.Name);
            Assert.AreEqual(10, weapon.ChanceToHitAdjustment);
            Assert.AreEqual(50, weapon.Range);
            Assert.AreEqual(0, weapon.AreaEffectRadius);
            Assert.AreEqual(3, weapon.DamageRangeLow);
            Assert.AreEqual(5, weapon.DamageRangeHigh);
            Assert.AreEqual(25, weapon.CriticalChance);
            Assert.AreEqual(6, weapon.CriticalDamageLow);
            Assert.AreEqual(8, weapon.CriticalDamageHigh);
            Assert.AreEqual(4, weapon.ClipSize);
            Assert.AreEqual(2, weapon.ActionPointsRequired);
            Assert.AreEqual(WeaponEnum.SniperRifle, weapon.Type);
        }

        private static void TestGrenade(Weapon weapon)
        {
            Assert.IsNotNull(weapon);
            Assert.AreEqual("Grenade", weapon.Name);
            Assert.AreEqual(0, weapon.ChanceToHitAdjustment);
            Assert.AreEqual(10, weapon.Range);
            Assert.AreEqual(3, weapon.AreaEffectRadius);
            Assert.AreEqual(3, weapon.DamageRangeLow);
            Assert.AreEqual(4, weapon.DamageRangeHigh);
            Assert.AreEqual(0, weapon.CriticalChance);
            Assert.AreEqual(0, weapon.CriticalDamageLow);
            Assert.AreEqual(0, weapon.CriticalDamageHigh);
            Assert.AreEqual(1, weapon.ClipSize);
            Assert.AreEqual(1, weapon.ActionPointsRequired);
            Assert.AreEqual(WeaponEnum.Grenade, weapon.Type);
        }
      
    }
}