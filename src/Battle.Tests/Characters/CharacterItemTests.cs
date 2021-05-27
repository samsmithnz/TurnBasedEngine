using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Tests.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterItemTests
    {
        [TestMethod]
        public void CharacterItemFredTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Hitpoints = 1;
            fred.UtilityItemEquipped = ItemPool.CreateMedKit();

            //Act
            fred.UseItem(fred.UtilityItemEquipped);

            //Assert
            Assert.AreEqual(4, fred.Hitpoints);
            Assert.AreEqual(0, fred.UtilityItemEquipped.ClipRemaining = 0);
        }  
    }
}
