﻿using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.GameController;
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
    public class MultipleTurnsScenarioTest
    {
        [TestMethod]
        public void MultipleTurnsTest()
        {
            //Arrange
            bool teamIsDone = false;
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(5, 0, 5));
            fred.SetLocation(new Vector3(4, 0, 4), mission.Map);
            Team team1 = new Team()
            {
                Name = "Good guys",
                Characters = new List<Character>() { fred },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jeff = CharacterPool.CreateJeffBaddie(mission.Map, new Vector3(20, 0, 10));
            jeff.SetLocation(new Vector3(12, 0, 12), mission.Map);
            Team team2 = new Team()
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jeff },
                Color = "Red"
            };
            mission.Teams.Add(team2);
            mission.RandomNumbers = new RandomNumberQueue(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Assert - Setup
            Assert.AreEqual(Mission.MissionType.EliminateAllOpponents, mission.Objective);
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(2, mission.Teams.Count);
            Assert.AreEqual(50 * 50, mission.Map.Length);
            Assert.AreEqual("Good guys", mission.Teams[0].Name);
            Assert.AreEqual("Blue", mission.Teams[0].Color);
            Assert.AreEqual(1, mission.Teams[0].Characters.Count);
            Assert.AreEqual("Bad guys", mission.Teams[1].Name);
            Assert.AreEqual("Red", mission.Teams[1].Color);
            Assert.AreEqual(1, mission.Teams[1].Characters.Count);
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(0, mission.CurrentTeamIndex);

            //Act

            //Turn 1 - Team 1 starts
            //Fred moves 
            Vector3 fredDestination = new Vector3(14, 0, 14);
            PathFindingResult pathFindingResult = PathFinding.FindPath(fred.Location, fredDestination, mission.Map);
            CharacterMovement.MoveCharacter(fred, mission.Map, pathFindingResult, mission.RandomNumbers, null);
            Assert.AreEqual(0, fred.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(true, teamIsDone);
            bool missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 1 - Team 2 starts
            //Jeff moves
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(1, mission.CurrentTeamIndex);
            Vector3 jeffDestination = new Vector3(2, 0, 2);
            pathFindingResult = PathFinding.FindPath(jeff.Location, jeffDestination, mission.Map);
            CharacterMovement.MoveCharacter(jeff, mission.Map, pathFindingResult, mission.RandomNumbers, null);
            Assert.AreEqual(0, jeff.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(true, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 2 - Team 1 starts
            Assert.AreEqual(2, mission.TurnNumber);
            Assert.AreEqual(0, mission.CurrentTeamIndex);
            fredDestination = new Vector3(4, 0, 4);
            pathFindingResult = PathFinding.FindPath(fred.Location, fredDestination, mission.Map);
            CharacterMovement.MoveCharacter(fred, mission.Map, pathFindingResult, mission.RandomNumbers, null);
            Assert.AreEqual(0, fred.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(true, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 2 - Team 2 starts
            Assert.AreEqual(2, mission.TurnNumber);
            Assert.AreEqual(1, mission.CurrentTeamIndex);
            jeffDestination = new Vector3(12, 0, 12);
            pathFindingResult = PathFinding.FindPath(jeff.Location, jeffDestination, mission.Map);
            CharacterMovement.MoveCharacter(jeff, mission.Map, pathFindingResult, mission.RandomNumbers, null);
            Assert.AreEqual(0, jeff.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(true, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 3 - Team 1 starts - force an end turn
            Assert.AreEqual(3, mission.TurnNumber);
            Assert.AreEqual(0, mission.CurrentTeamIndex);
            fredDestination = new Vector3(5, 0, 5);
            pathFindingResult = PathFinding.FindPath(fred.Location, fredDestination, mission.Map);
            CharacterMovement.MoveCharacter(fred, mission.Map, pathFindingResult, mission.RandomNumbers, null);
            Assert.AreEqual(1, fred.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(false, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 3 - Team 2 starts - force an end turn
            Assert.AreEqual(3, mission.TurnNumber);
            Assert.AreEqual(1, mission.CurrentTeamIndex);
            jeffDestination = new Vector3(11, 0, 11);
            pathFindingResult = PathFinding.FindPath(jeff.Location, jeffDestination, mission.Map);
            CharacterMovement.MoveCharacter(jeff, mission.Map, pathFindingResult, mission.RandomNumbers, null);
            Assert.AreEqual(1, jeff.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(false, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 4 - Team 1 starts, and aborts mission
            //Fred shoots at Jeff and kills him. 
            EncounterResult encounter1 = Encounter.AttackCharacter(fred,
                    fred.WeaponEquipped,
                    jeff,
                    mission.Map,
                    mission.RandomNumbers);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 86, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now -1
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log1, encounter1.LogString);

            //End of of battle
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(true, missionIsComplete);
            mission.EndMission();

            //Assert
            Assert.AreEqual(-1, jeff.HitpointsCurrent);
            Assert.AreEqual(1, fred.MissionsCompleted);
            Assert.AreEqual(0, jeff.MissionsCompleted);
            Assert.AreEqual(1, fred.TotalShots);
            Assert.AreEqual(1, fred.TotalHits);
            Assert.AreEqual(0, fred.TotalMisses);
            Assert.AreEqual(1, fred.TotalKills);
            Assert.AreEqual(5, fred.TotalDamage);
            Assert.AreEqual(0, jeff.TotalShots);
            Assert.AreEqual(0, jeff.TotalHits);
            Assert.AreEqual(0, jeff.TotalMisses);
            Assert.AreEqual(0, jeff.TotalKills);
            Assert.AreEqual(0, jeff.TotalDamage);

        }
    }
}