using Battle.Logic.Characters;
using Battle.Logic.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace Battle.Tests.Items
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class TeamTests
    {
        [TestMethod]
        public void TeamCheckForNextFirstCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team = new(1)
            {
                Characters = new() { fred, harry }
            };

            //Act           
            Character character = team.GetCharacter(team.GetNextCharacterIndex());

            //Assert
            Assert.AreEqual(harry.Name, character.Name);
        }
        [TestMethod]
        public void TeamCheckForPreviousFirstCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team = new(1)
            {
                Characters = new() { fred, harry }
            };

            //Act           
            Character character = team.GetCharacter(team.GetPreviousCharacterIndex());

            //Assert
            Assert.AreEqual(harry.Name, character.Name);
        }

        [TestMethod]
        public void TeamCheckForNextSecondCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            fred.ActionPointsCurrent = 0;
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team = new(1)
            {
                Characters = new() { fred, harry }
            };

            //Act
            Character character = team.GetCharacter(team.GetNextCharacterIndex());

            //Assert
            Assert.AreEqual(harry.Name, character.Name);
        }

        [TestMethod]
        public void TeamCheckForPreviousSecondCharacterForActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            fred.ActionPointsCurrent = 0;
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team = new(1)
            {
                Characters = new() { fred, harry }
            };

            //Act
            Character character = team.GetCharacter(team.GetPreviousCharacterIndex());

            //Assert
            Assert.AreEqual(harry.Name, character.Name);
        }

        [TestMethod]
        public void TeamCheckForNextSecondCharacterForHPsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            harry.HitpointsCurrent = 0;
            Team team = new(1)
            {
                Characters = new() { fred, harry }
            };

            //Act
            Character character = team.GetCharacter(team.GetNextCharacterIndex());

            //Assert
            Assert.AreEqual(fred.Name, character.Name);
        }

        [TestMethod]
        public void TeamCheckForPreviousSecondCharacterForHPsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            harry.HitpointsCurrent = 0;
            Team team = new(1)
            {
                Characters = new() { fred, harry }
            };

            //Act
            Character character = team.GetCharacter(team.GetPreviousCharacterIndex());

            //Assert
            Assert.AreEqual(fred.Name, character.Name);
        }

        [TestMethod]
        public void TeamCheckForNextActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            fred.ActionPointsCurrent = 0;
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            harry.ActionPointsCurrent = 0;
            Team team = new(1)
            {
                Characters = new() { fred, harry }
            };

            //Act
            Character character = team.GetCharacter(team.GetNextCharacterIndex());

            //Assert
            Assert.AreEqual(null, character);
        }

        [TestMethod]
        public void TeamCheckForPreviousActionPointsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            fred.ActionPointsCurrent = 0;
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            harry.ActionPointsCurrent = 0;
            Team team = new(1)
            {
                Characters = new() { fred, harry }
            };

            //Act
            Character character = team.GetCharacter(team.GetPreviousCharacterIndex());

            //Assert
            Assert.AreEqual(null, character);
        }

        [TestMethod]
        public void AITargetingWithNoTeamsTest()
        {
            //Arrange
            Mission mission = new();
            try
            {
                mission.StartMission();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Unexpected number of teams: 0", ex.Message);
            }
        }

        [TestMethod]
        public void AITargetingWithOneTeamsTest()
        {
            //Arrange
            Mission mission = new();
            mission.Teams.Add(new(1));
            try
            {
                mission.StartMission();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Unexpected number of teams: 1", ex.Message);
            }
        }

        [TestMethod]
        public void AITargetingWithTwoTeamsNoCharacterTest()
        {
            //Arrange
            Mission mission = new();
            mission.Teams.Add(new(1));
            mission.Teams.Add(new(0));
            mission.StartMission();
            Character first = mission.Teams[0].GetFirstCharacter();
            Assert.AreEqual(null, first);
        }


    }
}