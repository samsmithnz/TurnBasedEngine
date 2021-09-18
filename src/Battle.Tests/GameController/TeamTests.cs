using Battle.Logic.Characters;
using Battle.Logic.GameController;
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
        public void TeamCheckFirstCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHeroSidekick(null, Vector3.One);
            Team team = new Team()
            {
                Characters = new List<Character>() { fred, harry }
            };

            //Act
            Character character = team.GetCharacterWithActionPoints();

            //Assert
            Assert.AreEqual(fred, character);
        }

        [TestMethod]
        public void TeamCheckSecondCharacterForActionPointsTest()
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
            Character character = team.GetCharacterWithActionPoints();

            //Assert
            Assert.AreEqual(harry, character);
        }

        [TestMethod]
        public void TeamCheckForActionPointsTest()
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
            Character character = team.GetCharacterWithActionPoints();

            //Assert
            Assert.AreEqual(null, character);
        }


    }
}