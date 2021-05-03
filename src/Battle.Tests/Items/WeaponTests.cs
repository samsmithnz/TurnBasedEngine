using Battle.Logic.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Weapons
{
    [TestClass]
    [TestCategory("L0")]
    public class WeaponTests
    {
        [TestMethod]
        public void WeaponSwordTest()
        {
            //Arrange
            Weapon sword = WeaponPool.CreateSword();

            //Act            

            //Assert
            TestSword(sword);
        }

        private static void TestSword(Weapon sword)
        {
            Assert.IsNotNull(sword);
            Assert.AreEqual("Sword", sword.Name);
            Assert.AreEqual(1, sword.Range);
            Assert.AreEqual(10, sword.DamageRange);
            Assert.AreEqual(20, sword.CriticalChance);
        }
      
    }
}
