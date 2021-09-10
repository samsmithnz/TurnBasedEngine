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
    public class OverwatchScenarioTest
    {
        [TestMethod]
        public void FredInOverwatchShootsAtJeffTest()
        {
            //Arrange
            int xMax = 50;
            int zMax = 50;
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            List<int> MapRandomNumbers = RandomNumber.GenerateRandomNumberList(0, xMax - 1, 0, xMax * zMax * 5);
            Queue<int> MapNumberQueue = new Queue<int>(MapRandomNumbers);
            //Add 100 full cover items randomly
            for (int i = 0; i < 100; i++)
            {
                int x = MapNumberQueue.Dequeue();
                int z = MapNumberQueue.Dequeue();
                mission.Map[x, 0, z] = CoverType.FullCover;
            }
            //Add 100 half cover items randomly
            for (int i = 0; i < 100; i++)
            {
                int x = MapNumberQueue.Dequeue();
                int z = MapNumberQueue.Dequeue();
                mission.Map[x, 0, z] = CoverType.HalfCover;
            }
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(1, 0, 1));
            fred.MobilityRange = 16;
            Team team1 = new Team()
            {
                Name = "Good guys",
                Characters = new List<Character>() { fred }
            };
            mission.Teams.Add(team1);
            Character jeff = CharacterPool.CreateJeffBaddie(mission.Map, new Vector3(19, 0, 19));
            Team team2 = new Team()
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jeff }
            };
            mission.Teams.Add(team2);

            //Assert - Setup
            Assert.AreEqual(Mission.MissionType.EliminateAllOpponents, mission.Objective);
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(2, mission.Teams.Count);
            Assert.AreEqual(50 * 50, mission.Map.Length);
            Assert.AreEqual("Good guys", mission.Teams[0].Name);
            Assert.AreEqual(1, mission.Teams[0].Characters.Count);
            Assert.AreEqual("Bad guys", mission.Teams[1].Name);
            Assert.AreEqual(1, mission.Teams[1].Characters.Count);
            Assert.AreEqual(8, mission.RandomNumbers.Queue[0]);

            //Act

            //Turn 1 - Team 1 starts
            //Fred runs to cover
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(mission.Map, fred.Location, fred.MobilityRange);
            string mapMovementString = MapCore.GetMapStringWithItems(mission.Map, movementPossibileTiles);
            string mapMovementResult = @"
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . □ . . ■ . . □ . . . . . . . . □ . . . . . ■ . . . . . . . . . ■ . . . ■ . . . 
. . . . . . . . . . . . . . . . . . . . ■ . . . ■ . . . . . . . . . . . . . . . . . . ■ . . . . □ . 
. . . . . . . . . . . . . . . . ■ . . . . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . ■ . . . . . . . . ■ . . . . . . . . . □ ■ . . . . . . . . . . . . . . . . . . . 
. . . ■ . . ■ . . . □ . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . □ . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . □ . . . . . . . . ■ . . . . . . 
. . . . . □ . . . . . . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . □ . . . . . . □ . 
. . . . ■ . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . □ . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . ■ . . □ . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . □ . . . . □ . . □ . . . . . . . . . ■ . . . . . . . . . 
. . . . . . . . . . . □ . . . . □ . . . . . . . . . . . . . . . . . . . . □ . . . . . . ■ . . . . . 
. . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . ■ . . . . . . . . . . . 
. . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . ■ . . . ■ . . . . . . ■ . . . ■ . . . . . . . . . . . . . . 
. . . . . . ■ . . . . . . ■ . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . 
. □ . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . . . . ■ . . ■ . . . . 
. . . . . . . . . . . . . . . . . . . . □ . . . . ■ . □ . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . ■ . . . . . . . . . . . □ . . . □ . . . . . □ . . . . . . . . ■ . □ . . . . . . . . 
. . . . □ . □ . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . 
. . . . . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . □ . 
. . . . . . . . . . . . . . . . . . . . . . . . □ □ . . . . □ . . . . . . . . . . . . . . . . ■ . . 
□ . . . . . . . . . ■ . . . . . . . . . . . . . . . . . ■ . . □ . ■ . . . ■ . . . ■ . . . . . . ■ . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . □ . . □ . . . . . . 
. . . . . . . . . . . . . . . . . . ■ . . . . . . . . . . . . ■ . . ■ . . . □ . □ . . . . . . . . . 
. . . . . . . . . . . . . ■ . . . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . □ . ■ . . □ . . . . . . . . . ■ . . . . . . . ■ ■ . . . . . □ . . . . . . . . . . ■ . 
. . . . . . . . . ■ . . . . ■ . ■ . . . . ■ . . . . . □ . . . . ■ . . . . . . □ . . . □ . . . . . . 
□ . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
■ . . . . . . . . . . . . . . . . . . P □ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. ■ . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . . . 
■ o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . □ . . □ . . . . . 
o o o o . . . . . . . . . . . . . . . . . . . . □ . . . . . . . . □ . . . . . . . . . . . □ . . . . 
o o o o o o . . . . . . □ . . . . . . . . . . . . . ■ . . . . . . ■ . . . ■ . . . . □ . . . . . . . 
o o o o o o o . o . . . . . . . ■ . . . . . . . . . . □ . . . . . . . . . . . . . . . . □ ■ . . . . 
o o o o o □ o o o o o . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . . . . . . □ ■ . . 
o o o ■ o o o o o □ o o □ . . □ . . . . . . . . . . . . . ■ . □ . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . □ . . . . . . . □ . . . . . □ . . . . . . . 
o o o o o o o ■ o o o o o o . . . . . . . . . . . . . . . . ■ . □ . . . . . . . ■ . □ . . . . . . . 
o o o o □ □ o o ■ o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . □ . . . 
o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . □ . . . . . . 
o o o □ o □ o o o □ o ■ o o o . . . . . . . . . ■ □ . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o □ o o o □ o o . . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . 
o o o □ o □ o ■ o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . □ . 
o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . ■ . □ . . . ■ . . . . . . . . . . 
o o o o o o o o o ■ ■ o o o ■ o o . . . . . . . . . ■ . . . . . □ . . . . . . . . ■ . . . . . ■ . . 
o o o o o o o o o o o o o o o o o . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . 
o P o o o o o o o o o o o o o o □ . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . □ . 
o o o o o o o o o o o o o o o o o . . . . . . . . . . . . □ . □ . . . . . . . . . . . . . . . . ■ . 
";
            Assert.AreEqual(mapMovementResult, mapMovementString);

            //Fred aims at Jeff, who is behind high cover. 
            List<Character> characters = fred.GetCharactersInView(mission.Map, new List<Team>() { team2 });
            Assert.AreEqual(characters[0], jeff);

            int chanceToHit = EncounterCore.GetChanceToHit(fred, fred.WeaponEquipped, jeff);
            int chanceToCrit = EncounterCore.GetChanceToCrit(fred, fred.WeaponEquipped, jeff, mission.Map, false);
            DamageOptions damageOptions = EncounterCore.GetDamageRange(fred, fred.WeaponEquipped);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);

            //Fred shoots at Jeff, and misses 
            EncounterResult encounter1 = Encounter.AttackCharacter(fred,
                    fred.WeaponEquipped,
                    jeff,
                    mission.Map,
                    mission.RandomNumbers);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 80, (dice roll: 8)
Low cover downgraded to no cover at <5, 0, 5>
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log1, encounter1.LogString);
            Assert.AreEqual(new Vector3(5, 0, 5), encounter1.MissedLocation);

            //Fred shoots at Jeff, and hits him. 
            EncounterResult encounter2 = Encounter.AttackCharacter(fred,
                    fred.WeaponEquipped,
                    jeff,
                    mission.Map,
                    mission.RandomNumbers);
            string log2 = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 81)
Damage range: 3-5, (dice roll: 76)
Critical chance: 70, (dice roll: 55)
Critical damage range: 8-12, (dice roll: 76)
11 damage dealt to character Jeff, HP is now -7
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log2, encounter2.LogString);

        }
    }
}