using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterActionTests
    {
        [TestMethod]
        public void StartOfTurnTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();

            //Act
            List<CharacterAction> actions = fred.GetCurrentActions();

            //Assert
            Assert.IsTrue(actions != null);
            Assert.AreEqual(5, actions.Count);
            Assert.AreEqual("1", actions[0].KeyBinding);
            Assert.AreEqual("_shoot", actions[0].Name);;
            Assert.AreEqual("Shoot", actions[0].Caption);
            Assert.AreEqual("2", actions[1].KeyBinding);
            Assert.AreEqual("_overwatch", actions[1].Name);
            Assert.AreEqual("3", actions[2].KeyBinding);
            Assert.AreEqual("_throw_grenade", actions[2].Name);
            Assert.AreEqual("4", actions[3].KeyBinding);
            Assert.AreEqual("_use_medkit", actions[3].Name);
            Assert.AreEqual("5", actions[4].KeyBinding);
            Assert.AreEqual("_hunker_down", actions[4].Name);
        }

        [TestMethod]
        public void NeedToReloadTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.WeaponEquipped.AmmoCurrent = 0;

            //Act
            List<CharacterAction> actions = fred.GetCurrentActions();

            //Assert
            Assert.IsTrue(actions != null);
            Assert.AreEqual(4, actions.Count);
            Assert.AreEqual("1", actions[0].KeyBinding);
            Assert.AreEqual("_reload", actions[0].Name); ;
            Assert.AreEqual("Reload", actions[0].Caption);
            Assert.AreEqual("3", actions[1].KeyBinding);
            Assert.AreEqual("_throw_grenade", actions[1].Name);
            Assert.AreEqual("4", actions[2].KeyBinding);
            Assert.AreEqual("_use_medkit", actions[2].Name);
            Assert.AreEqual("5", actions[3].KeyBinding);
            Assert.AreEqual("_hunker_down", actions[3].Name);
        }

        [TestMethod]
        public void NoUtilityItemTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.UtilityWeaponEquipped = null;
            fred.UtilityItemEquipped = null;

            //Act
            List<CharacterAction> actions = fred.GetCurrentActions();

            //Assert
            Assert.IsTrue(actions != null);
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("1", actions[0].KeyBinding);
            Assert.AreEqual("_shoot", actions[0].Name); ;
            Assert.AreEqual("Shoot", actions[0].Caption);
            Assert.AreEqual("2", actions[1].KeyBinding);
            Assert.AreEqual("_overwatch", actions[1].Name);
            Assert.AreEqual("5", actions[2].KeyBinding);
            Assert.AreEqual("_hunker_down", actions[2].Name);
        }

        [TestMethod]
        public void MedKitConsumedTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.UtilityItemEquipped.ClipRemaining = 0;

            //Act
            List<CharacterAction> actions = fred.GetCurrentActions();

            //Assert
            Assert.IsTrue(actions != null);
            Assert.AreEqual(4, actions.Count);
            Assert.AreEqual("1", actions[0].KeyBinding);
            Assert.AreEqual("_shoot", actions[0].Name); ;
            Assert.AreEqual("Shoot", actions[0].Caption);
            Assert.AreEqual("2", actions[1].KeyBinding);
            Assert.AreEqual("_overwatch", actions[1].Name);
            Assert.AreEqual("3", actions[2].KeyBinding);
            Assert.AreEqual("_throw_grenade", actions[2].Name);
            Assert.AreEqual("5", actions[3].KeyBinding);
            Assert.AreEqual("_hunker_down", actions[3].Name);
        }

        [TestMethod]
        public void NoActionsRemainingTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.ActionPointsCurrent = 0;

            //Act
            List<CharacterAction> actions = fred.GetCurrentActions();

            //Assert
            Assert.IsTrue(actions != null);
            Assert.AreEqual(0, actions.Count);
        }

    }
}
