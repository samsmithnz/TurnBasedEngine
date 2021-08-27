using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

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
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
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
            bool leveledUpSuccessfully = fred.LevelUpCharacter();

            //Assert - after the level up
            Assert.IsTrue(leveledUpSuccessfully);
            Assert.IsTrue(fred.LevelUpIsReady == false);
            Assert.AreEqual(5, fred.HitpointsMax);
            Assert.AreEqual(4, fred.HitpointsCurrent);
        }

        [TestMethod]
        public void FredLevelUpToLevel1NotReadyTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Level = 1;
            fred.Experience = 0;
            fred.LevelUpIsReady = false;
            fred.HitpointsCurrent = 3;

            //Assert - before the level up

            //Act
            fred.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);
            Assert.IsTrue(fred.LevelUpIsReady == false);
            Assert.AreEqual(4, fred.HitpointsMax);
            Assert.AreEqual(3, fred.HitpointsCurrent);
            bool leveledUpSuccessfully = fred.LevelUpCharacter();

            //Assert - after the level up
            Assert.IsTrue(leveledUpSuccessfully == false);
            Assert.IsTrue(fred.LevelUpIsReady == false);
            Assert.AreEqual(4, fred.HitpointsMax);
            Assert.AreEqual(3, fred.HitpointsCurrent);
        }

    }
}
