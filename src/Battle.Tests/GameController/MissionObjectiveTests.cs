using Battle.Logic.Characters;
using Battle.Logic.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Battle.Tests.GameController
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class MissionObjectiveTests
    {
        [TestMethod]
        public void MissionObjectiveEliminateAllCompleteTest()
        {
            //Arrange
            Mission mission = new Mission();
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team1 = new(1)
            {
                Characters = new() { fred, harry }
            };
            mission.Teams.Add(team1);
            mission.Teams.Add(new Team(0));
            mission.StartMission();

            //Act           
            bool missionComplete = mission.CheckIfMissionIsCompleted();

            //Assert
            Assert.IsTrue(missionComplete);
        }

        [TestMethod]
        public void MissionObjectiveEliminateAllNotCompleteTest()
        {
            //Arrange
            Mission mission = new Mission();
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team1 = new(1)
            {
                Characters = new() { fred, harry }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(null, Vector3.One);
            Team team2 = new Team(0)
            {
                Characters = new() { jethro }
            };
            mission.Teams.Add(team2);
            mission.StartMission();

            //Act           
            bool missionComplete = mission.CheckIfMissionIsCompleted();

            //Assert
            Assert.IsFalse(missionComplete);
        }


    }
}