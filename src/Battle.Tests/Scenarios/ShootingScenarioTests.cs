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
    public class ShootingScenarioTests
    {
        [TestMethod]
        public void JeffMovesToCoverAndExchangesFireOver2TurnsToWinTest()
        {
            //Arrange
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[6, 0, 5] = CoverType.FullCover;
            mission.Map[20, 0, 11] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(5, 0, 5));
            Team team1 = new Team()
            {
                Name = "Good guys",
                Characters = new List<Character>() { fred },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jeff = CharacterPool.CreateJeffBaddie(mission.Map, new Vector3(20, 0, 10));
            jeff.HitpointsCurrent = 6;
            Team team2 = new Team()
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jeff },
                Color = "Red"
            };
            mission.Teams.Add(team2);
            RandomNumberQueue diceRolls = new RandomNumberQueue(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll


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

            //Act

            //Turn 1 - Team 1 starts
            //Fred runs to cover
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(mission.Map, fred.Location, fred.MobilityRange);
            Vector3 destination = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTiles)
            {
                if (item == new Vector3(9, 0, 10))
                {
                    destination = item;
                }
            }
            Assert.AreEqual(new Vector3(9, 0, 10), destination);
            string mapMovementString = MapCore.GetMapStringWithItems(mission.Map, movementPossibileTiles);
            string mapMovementResult = @"
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. o o o o o o o o o . . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o . . . . . . . . . P . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o P ■ o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapMovementResult, mapMovementString);
            PathFindingResult pathFindingResult = PathFinding.FindPath(fred.Location, destination, mission.Map);
            List<ActionResult> movementResults = CharacterMovement.MoveCharacter(fred, mission.Map, pathFindingResult, diceRolls, null);
            Assert.AreEqual(5, movementResults.Count);
            string log = @"
Fred is moving from <5, 0, 5> to <6, 0, 6>
Fred is moving from <6, 0, 6> to <7, 0, 7>
Fred is moving from <7, 0, 7> to <8, 0, 8>
Fred is moving from <8, 0, 8> to <8, 0, 9>
Fred is moving from <8, 0, 9> to <9, 0, 10>
";
            Assert.AreEqual(log, ActionResultLog.LogString(movementResults));

            //Fred aims at Jeff, who is behind high cover. 
            string mapString1 = fred.GetCharactersInViewMapString(mission.Map, new List<Team> { team2 });
            List<Character> characters = fred.GetCharactersInView(mission.Map, new List<Team>() { team2 });
            Assert.AreEqual(characters[0], jeff);
            string mapResult1 = @"
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o ■ . . . . o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o o o o P o o o o o o o o o o P o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o ■ o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o o . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o o . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
o o o o . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . 
o o o o . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . 
o o o . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . 
";
            Assert.AreEqual(mapResult1, mapString1);

            int chanceToHit = EncounterCore.GetChanceToHit(fred, fred.WeaponEquipped, jeff);
            int chanceToCrit = EncounterCore.GetChanceToCrit(fred, fred.WeaponEquipped, jeff, mission.Map, false);
            DamageOptions damageOptions = EncounterCore.GetDamageRange(fred, fred.WeaponEquipped);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);

            //Fred shoots at Jeff, who is behind high cover. He hits him. 
            EncounterResult encounter1 = Encounter.AttackCharacter(fred,
                    fred.WeaponEquipped,
                    jeff,
                    mission.Map,
                    diceRolls);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 1
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log1, encounter1.LogString);

            //Turn 1 - Team 2 starts
            //Jeff aims back and misses
            List<Character> characters2 = jeff.GetCharactersInView(mission.Map, new List<Team>() { team1 });
            Assert.AreEqual(characters2[0], fred);
            int chanceToHit2 = EncounterCore.GetChanceToHit(jeff, jeff.WeaponEquipped, fred);
            int chanceToCrit2 = EncounterCore.GetChanceToCrit(jeff, jeff.WeaponEquipped, jeff, mission.Map, false);
            DamageOptions damageOptions2 = EncounterCore.GetDamageRange(jeff, jeff.WeaponEquipped);
            Assert.AreEqual(72, chanceToHit2);
            Assert.AreEqual(70, chanceToCrit2);
            Assert.AreEqual(3, damageOptions2.DamageLow);
            Assert.AreEqual(5, damageOptions2.DamageHigh);

            //Jeff shoots back and misses
            EncounterResult encounter2 = Encounter.AttackCharacter(jeff,
                    jeff.WeaponEquipped,
                    fred,
                    mission.Map,
                    diceRolls);
            string log2 = @"
Jeff is attacking with Shotgun, targeted on Fred
Missed: Chance to hit: 72, (dice roll: 0)
0 XP added to character Jeff, for a total of 0 XP
";
            Assert.AreEqual(log2, encounter2.LogString);

            //Turn 2 - Team 1 starts
            //Fred shoots again, and kills Jeff.
            List<Character> characters3 = fred.GetCharactersInView(mission.Map, new List<Team>() { team2 });
            Assert.AreEqual(characters3[0], jeff);
            int chanceToHit3 = EncounterCore.GetChanceToHit(fred, fred.WeaponEquipped, jeff);
            int chanceToCrit3 = EncounterCore.GetChanceToCrit(fred, fred.WeaponEquipped, jeff, mission.Map, false);
            DamageOptions damageOptions3 = EncounterCore.GetDamageRange(fred, fred.WeaponEquipped);
            Assert.AreEqual(80, chanceToHit3);
            Assert.AreEqual(70, chanceToCrit3);
            Assert.AreEqual(3, damageOptions3.DamageLow);
            Assert.AreEqual(5, damageOptions3.DamageHigh);

            EncounterResult encounter3 = Encounter.AttackCharacter(fred,
                    fred.WeaponEquipped,
                    jeff,
                    mission.Map,
                    diceRolls);
            string log3 = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now -11
Jeff is killed
100 XP added to character Fred, for a total of 110 XP
Fred is ready to level up
";
            Assert.AreEqual(log3, encounter3.LogString);

            //End of of battle
            mission.EndMission();

            //Assert
            Assert.AreEqual(-11, jeff.HitpointsCurrent);
            Assert.AreEqual(1, fred.MissionsCompleted);
            Assert.AreEqual(0, jeff.MissionsCompleted);
            Assert.AreEqual(2, fred.TotalShots);
            Assert.AreEqual(2, fred.TotalHits);
            Assert.AreEqual(0, fred.TotalMisses);
            Assert.AreEqual(1, fred.TotalKills);
            Assert.AreEqual(17, fred.TotalDamage);
            Assert.AreEqual(1, jeff.TotalShots);
            Assert.AreEqual(0, jeff.TotalHits);
            Assert.AreEqual(1, jeff.TotalMisses);
            Assert.AreEqual(0, jeff.TotalKills);
            Assert.AreEqual(0, jeff.TotalDamage);

        }

    }
}