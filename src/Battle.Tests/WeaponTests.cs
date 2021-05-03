using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battle.Logic;
using System.Collections.Generic;

namespace Battle.Tests
{
    [TestClass]
    [TestCategory("L0")]
    public class WeaponTests
    {
        [TestMethod]
        public void WeaponSwordTest()
        {
            //Arrange
            Weapon sword = CreateSword();

            //Act            

            //Assert
            TestSword(sword);
        }

        private static Weapon CreateSword()
        {
            Weapon sword = new()
            {
                Name = "Sword",
                Range = 1,
                DamageRange = 10,
                CriticalChance = 20
            };
            return sword;
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
