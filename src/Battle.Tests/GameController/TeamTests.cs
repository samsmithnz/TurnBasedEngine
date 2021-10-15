using Battle.Logic.Characters;
using Battle.Logic.Game;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Items
{
    [TestClass]
    [TestCategory("L0")]
    public class TeamTests
    {
        [TestMethod]
        public void TeamCheckForNextFirstCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHeroSidekick(null, Vector3.One);
            Team team = new Team()
            {
                Characters = new List<Character>() { fred, harry }
            };

            //Act           
            Character character = team.GetNextCharacter();

            //Assert
            Assert.AreEqual(harry, character);
        }
        [TestMethod]
        public void TeamCheckForPreviousFirstCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHeroSidekick(null, Vector3.One);
            Team team = new Team()
            {
                Characters = new List<Character>() { fred, harry }
            };

            //Act           
            Character character = team.GetPreviousCharacter();

            //Assert
            Assert.AreEqual(harry, character);
        }

        [TestMethod]
        public void TeamCheckForNextSecondCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            fred.ActionPointsCurrent = 0;
            Character harry = CharacterPool.CreateHarryHeroSidekick(null, Vector3.One);
            Team team = new Team()
            {
                Characters = new List<Character>() { fred, harry }
            };

            //Act
            Character character = team.GetNextCharacter();

            //Assert
            Assert.AreEqual(harry, character);
        }

        [TestMethod]
        public void TeamCheckForPreviousSecondCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            fred.ActionPointsCurrent = 0;
            Character harry = CharacterPool.CreateHarryHeroSidekick(null, Vector3.One);
            Team team = new Team()
            {
                Characters = new List<Character>() { fred, harry }
            };

            //Act
            Character character = team.GetPreviousCharacter();

            //Assert
            Assert.AreEqual(harry, character);
        }

        [TestMethod]
        public void TeamCheckForNextActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            fred.ActionPointsCurrent = 0;
            Character harry = CharacterPool.CreateHarryHeroSidekick(null, Vector3.One);
            harry.ActionPointsCurrent = 0;
            Team team = new Team()
            {
                Characters = new List<Character>() { fred, harry }
            };

            //Act
            Character character = team.GetNextCharacter();

            //Assert
            Assert.AreEqual(null, character);
        }

        [TestMethod]
        public void TeamCheckForPreviousActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            fred.ActionPointsCurrent = 0;
            Character harry = CharacterPool.CreateHarryHeroSidekick(null, Vector3.One);
            harry.ActionPointsCurrent = 0;
            Team team = new Team()
            {
                Characters = new List<Character>() { fred, harry }
            };

            //Act
            Character character = team.GetPreviousCharacter();

            //Assert
            Assert.AreEqual(null, character);
        }


    }
}