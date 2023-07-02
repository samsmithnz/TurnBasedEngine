using TBE.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));
            fred.Level = 1;
            fred.XP = 100;
            fred.LevelUpIsReady = true;
            fred.HitpointsCurrent = 3;

            //Assert - before the level up

            //Act
            fred.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(fred.Level, fred.XP);
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
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));
            fred.Level = 1;
            fred.XP = 0;
            fred.LevelUpIsReady = false;
            fred.HitpointsCurrent = 3;

            //Assert - before the level up

            //Act
            fred.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(fred.Level, fred.XP);
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
