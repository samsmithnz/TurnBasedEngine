using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterExperienceTests
    {
        [TestMethod]
        public void CharacterFredLevel1To2XPTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Experience = 100;
            fred.Level = 1;

            //Act
            bool readyToLevelUp = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);

            //Assert
            Assert.AreEqual(true, readyToLevelUp);
        }

        [TestMethod]
        public void CharacterFredLevel2To3XPTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Experience = 500;
            fred.Level = 2;

            //Act
            bool readyToLevelUp = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);

            //Assert
            Assert.AreEqual(true, readyToLevelUp);
        }

        [TestMethod]
        public void CharacterFredLevel3To4XPTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Experience = 1000;
            fred.Level = 3;

            //Act
            bool readyToLevelUp = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);

            //Assert
            Assert.AreEqual(true, readyToLevelUp);
        }

        [TestMethod]
        public void CharacterFredLevel4To5XPTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Experience = 1500;
            fred.Level = 4;

            //Act
            bool readyToLevelUp = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);

            //Assert
            Assert.AreEqual(true, readyToLevelUp);
        }

        [TestMethod]
        public void CharacterFredLevel5To6XPTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Experience = 2000;
            fred.Level = 5;

            //Act
            bool readyToLevelUp = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);

            //Assert
            Assert.AreEqual(true, readyToLevelUp);
        }

        [TestMethod]
        public void CharacterFredLevel6To7XPTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Experience = 2500;
            fred.Level = 6;

            //Act
            bool readyToLevelUp = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);

            //Assert
            Assert.AreEqual(true, readyToLevelUp);
        }

        [TestMethod]
        public void CharacterFredLevel7To8XPFailTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Experience = 3000 - 1;
            fred.Level = 7;

            //Act
            bool readyToLevelUp = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);

            //Assert
            Assert.AreEqual(false, readyToLevelUp);
        }

        [TestMethod]
        public void CharacterFredLevel7To8XPTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new Vector3(0, 0, 0));
            fred.Experience = 3000;
            fred.Level = 7;

            //Act
            bool readyToLevelUp = Experience.CheckIfReadyToLevelUp(fred.Level, fred.Experience);

            //Assert
            Assert.AreEqual(true, readyToLevelUp);
        }

    }
}
