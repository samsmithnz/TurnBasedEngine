using TBE.Logic.Characters;
using TBE.Logic.Game;
using TBE.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.GameController
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class MissionObjectiveSimpleTests
    {
        [TestMethod]
        public void MissionObjectiveEliminateAllCompleteTest()
        {
            //Arrange
            Mission mission = new();
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
            Mission mission = new();
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team1 = new(1)
            {
                Characters = new() { fred, harry }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(null, Vector3.One);
            Team team2 = new(0)
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

        [TestMethod]
        public void MissionObjectiveExtractionCompleteTest()
        {
            //Arrange
            Mission mission = new();
            mission.Objectives[0] = new MissionObjective(MissionObjectiveType.ExtractTroops, false);
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team1 = new(1)
            {
                Characters = new() { fred, harry }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(null, Vector3.One);
            Team team2 = new(0)
            {
                Characters = new() { jethro }
            };
            mission.Teams.Add(team2);
            mission.StartMission();

            //Act
            fred.ExtractedFromMission = true;
            harry.HitpointsCurrent = 0;
            bool missionComplete = mission.CheckIfMissionIsCompleted();

            //Assert
            Assert.IsTrue(missionComplete);
        }

        [TestMethod]
        public void MissionObjectiveExtractionNotCompleteTest()
        {
            //Arrange
            Mission mission = new();
            mission.Objectives[0] = new MissionObjective(MissionObjectiveType.ExtractTroops, false);
            Character fred = CharacterPool.CreateFredHero(null, Vector3.One);
            Character harry = CharacterPool.CreateHarryHero(null, Vector3.One);
            Team team1 = new(1)
            {
                Characters = new() { fred, harry }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(null, Vector3.One);
            Team team2 = new(0)
            {
                Characters = new() { jethro }
            };
            mission.Teams.Add(team2);
            mission.StartMission();

            //Act        
            fred.ExtractedFromMission = true;
            bool missionComplete = mission.CheckIfMissionIsCompleted();

            //Assert
            Assert.IsFalse(missionComplete);
        }

        [TestMethod]
        public void MissionObjectiveToggleSwitchCompleteTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Objectives[0] = new MissionObjective(MissionObjectiveType.ToggleSwitch, false, new Vector3(2f, 0f, 1f));
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(2, 0, 2));
            Character harry = CharacterPool.CreateHarryHero(mission.Map, new Vector3(5, 0, 5));
            Team team1 = new(1)
            {
                Characters = new() { fred, harry }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new Vector3(15, 0, 15));
            Team team2 = new(0)
            {
                Characters = new() { jethro }
            };
            mission.Teams.Add(team2);
            mission.StartMission();

            //Act
            mission.Objectives[0].ObjectiveIsComplete = true;
            bool missionComplete = mission.CheckIfMissionIsCompleted();
            List<CharacterAction> fredActions = fred.GetCurrentActions(mission.Map);
            List<CharacterAction> harryActions = harry.GetCurrentActions(mission.Map);

            //Assert
            Assert.IsTrue(missionComplete);
            Assert.AreEqual(5, fredActions.Count);
            Assert.AreEqual("_toggle", fredActions[4].Name);
            Assert.AreEqual("Toggle switch", fredActions[4].Caption);
            Assert.AreEqual("0", fredActions[4].KeyBinding);
            Assert.AreEqual(3, harryActions.Count);
        }

        [TestMethod]
        public void MissionObjectiveToggleSwitchNotCompleteTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Objectives[0] = new MissionObjective(MissionObjectiveType.ToggleSwitch, false, new Vector3(2f, 0f, 1f));
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(2, 0, 2));
            Character harry = CharacterPool.CreateHarryHero(mission.Map, new Vector3(5, 0, 5));
            Team team1 = new(1)
            {
                Characters = new() { fred, harry }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new Vector3(15, 0, 15));
            Team team2 = new(0)
            {
                Characters = new() { jethro }
            };
            mission.Teams.Add(team2);
            mission.StartMission();

            //Act        
            bool missionComplete = mission.CheckIfMissionIsCompleted();
            List<CharacterAction> fredActions = fred.GetCurrentActions(mission.Map);
            List<CharacterAction> harryActions = harry.GetCurrentActions(mission.Map);

            //Assert
            Assert.IsFalse(missionComplete);
            Assert.AreEqual(5, fredActions.Count);
            Assert.AreEqual("_toggle", fredActions[4].Name);
            Assert.AreEqual("Toggle switch", fredActions[4].Caption);
            Assert.AreEqual("0", fredActions[4].KeyBinding);
            Assert.AreEqual(3, harryActions.Count);
        }

    }
}