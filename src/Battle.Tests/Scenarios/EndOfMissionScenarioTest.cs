using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class EndOfMissionScenarioTest
    {
        [TestMethod]
        public void EndOfMissionWithNegativeHealthTest()
        {
            //Arrange
            bool teamIsDone;
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(4, 0, 4));
            Team team1 = new Team()
            {
                Name = "Good guys",
                Characters = new List<Character>() { fred },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new Vector3(12, 0, 12));
            Character bart = CharacterPool.CreateBartBaddie(mission.Map, new Vector3(10, 0, 10));
            bart.HitpointsCurrent = -10;
            Team team2 = new Team()
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jethro, bart },
                Color = "Red"
            };
            mission.Teams.Add(team2);
            mission.RandomNumbers = new RandomNumberQueue(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll
            mission.UpdateTargetsForAllTeams();

            //Act
            bool missionIsCompleted = mission.CheckIfMissionIsCompleted();

            //Assert
            Assert.IsTrue(missionIsCompleted == false);
        }
    }
}
