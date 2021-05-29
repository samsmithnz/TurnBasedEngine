using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterLevelUpTests
    {
        [TestMethod]
        public void FredLevelUpToLevel1Test()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Level = 1;
            fred.Experience = 100;
            fred.LevelUpIsReady = true;
            fred.HitpointsCurrent = 3;

            //Assert - before the level up

            //Act
            fred.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);
            Assert.IsTrue(fred.LevelUpIsReady);
            Assert.AreEqual(4, fred.HitpointsMax);
            Assert.AreEqual(3, fred.HitpointsCurrent);
            fred.LevelUpCharacter();

            //Assert - after the level up
            Assert.IsTrue(fred.LevelUpIsReady == false);
            Assert.AreEqual(5, fred.HitpointsMax );
            Assert.AreEqual(4, fred.HitpointsCurrent);
        }

    }
}
