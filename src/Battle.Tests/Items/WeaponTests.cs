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

        private static void TestRifle(Weapon rifle)
        {
            Assert.IsNotNull(rifle);
            Assert.AreEqual("Rifle", rifle.Name);
            Assert.AreEqual(10, rifle.ChanceToHitAdjustment);
            Assert.AreEqual(18, rifle.Range);
            Assert.AreEqual(10, rifle.DamageRange);
            Assert.AreEqual(20, rifle.CriticalChance);
            Assert.AreEqual(WeaponEnum.Standard, rifle.Type);
        }
      
    }
}