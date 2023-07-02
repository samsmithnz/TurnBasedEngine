using TBE.Logic.Characters;
using TBE.Logic.Game;
using TBE.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class EndOfMissionScenarioTest
    {
        [TestMethod]
        public void EndOfMissionWithNegativeHealthTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(4, 0, 4));
            Team team1 = new(1)
            {
                Name = "Good guys",
                Characters = new() { fred },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(12, 0, 12));
            Character bart = CharacterPool.CreateBartBaddie(mission.Map, new(10, 0, 10));
            bart.HitpointsCurrent = -10;
            Team team2 = new(0)
            {
                Name = "Bad guys",
                Characters = new() { jethro, bart },
                Color = "Red"
            };
            mission.Teams.Add(team2);
            mission.RandomNumbers = new(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll
            mission.StartMission();

            //Act
            bool missionIsCompleted = mission.CheckIfMissionIsCompleted();

            //Assert
            Assert.IsTrue(missionIsCompleted == false);
        }
    }
}
