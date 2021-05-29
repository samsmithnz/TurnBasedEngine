using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.GameController;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Scenarios
{
    [TestClass]
    [TestCategory("L0")]
    public class ScenarioTests
    {
        [TestMethod]
        public void JeffMovesToCoverAndExchangesFireOver2TurnsToWinTest()
        {
            //Arrange
            Mission mission = new();
            mission.Objective = Mission.MissionType.EliminateAllOpponents;
            mission.TurnNumber = 1;
            mission.Map = MapUtility.InitializeMap(50, 1, 50);
            mission.Map[6, 0, 5] = CoverType.FullCover;
            mission.Map[20, 0, 11] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(5, 0, 5);
            Team team1 = new()
            {
                Name = "Good guys",
                Characters = new() { fred }
            };
            mission.Teams.Add(team1);
            Character jeff = CharacterPool.CreateJeffBaddie();
            jeff.Location = new(20, 0, 10);
            jeff.HitpointsCurrent = 6;
            Team team2 = new()
            {
                Name = "Bad guys",
                Characters = new() { jeff }
            };
            mission.Teams.Add(team2);
            Queue<int> diceRolls = new(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll


            //Assert - Setup
            Assert.AreEqual(Mission.MissionType.EliminateAllOpponents, mission.Objective);
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(2, mission.Teams.Count);
            Assert.AreEqual(50 * 50, mission.Map.Length);
            Assert.AreEqual("Good guys", mission.Teams[0].Name);
            Assert.AreEqual(1, mission.Teams[0].Characters.Count);
            Assert.AreEqual("Bad guys", mission.Teams[1].Name);
            Assert.AreEqual(1, mission.Teams[1].Characters.Count);

            //Act

            //Turn 1 - Team 1 starts
            //Fred runs to cover
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(mission.Map, fred.Location, fred.MovementRange);
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
o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o . ■ o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapMovementResult, mapMovementString);
            PathFindingResult PathFindingResult = PathFinding.FindPath(fred.Location, destination, mission.Map);
            CharacterMovement.MoveCharacter(fred, mission.Map, PathFindingResult.Path, diceRolls, null);

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
            Assert.AreEqual(1, fred.TotalKills);
            Assert.AreEqual(17, fred.TotalDamage);
            Assert.AreEqual(1, jeff.TotalShots);
            Assert.AreEqual(0, jeff.TotalHits);
            Assert.AreEqual(0, jeff.TotalKills);
            Assert.AreEqual(0, jeff.TotalDamage);

        }

        //Enemy is behind cover and not visible.
        //Throw grenade to destory cover and make him visible.
        [TestMethod]
        public void JeffIsHidingBehindCoverScenarioTest()
        {
            //Arrange
            Mission mission = new();
            mission.TurnNumber = 1;
            mission.Map = MapUtility.InitializeMap(50,1, 50);
            mission.Map[5, 0, 6] = CoverType.FullCover;
            mission.Map[14, 0, 5] = CoverType.FullCover;
            mission.Map[14, 0, 6] = CoverType.FullCover;
            mission.Map[14, 0, 7] = CoverType.FullCover;
            mission.Map[14, 0, 8] = CoverType.FullCover;
            mission.Map[14, 0, 9] = CoverType.FullCover;
            mission.Map[14, 0, 10] = CoverType.FullCover;
            mission.Map[14, 0, 11] = CoverType.FullCover;
            mission.Map[14, 0, 12] = CoverType.FullCover;
            mission.Map[14, 0, 13] = CoverType.FullCover;
            mission.Map[14, 0, 14] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(5, 0, 5);
            Team team1 = new()
            {
                Name = "Good guys",
                Characters = new() { fred }
            };
            mission.Teams.Add(team1);
            Character jeff = CharacterPool.CreateJeffBaddie();
            jeff.Location = new(15, 0, 10);
            jeff.HitpointsCurrent = 5;
            Team team2 = new()
            {
                Name = "Bad guys",
                Characters = new() { jeff }
            };
            mission.Teams.Add(team2);
            Queue<int> diceRolls = new(new List<int> { 100, 100, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll


            //Assert - Setup
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(2, mission.Teams.Count);
            Assert.AreEqual(50 * 50, mission.Map.Length);
            Assert.AreEqual("Good guys", mission.Teams[0].Name);
            Assert.AreEqual(1, mission.Teams[0].Characters.Count);
            Assert.AreEqual("Bad guys", mission.Teams[1].Name);
            Assert.AreEqual(1, mission.Teams[1].Characters.Count);

            //Act

            //Turn 1 - Team 1 starts
            //Fred cannot see Jeff, who is hiding behind cover
            string mapString1 = fred.GetCharactersInViewMapString(mission.Map, new List<Team> { team2 });
            List<Character> characters = fred.GetCharactersInView(mission.Map, new List<Team>() { team2 });
            Assert.AreEqual(0, characters.Count);
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
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o . . . . . . . . . o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o . . . . . . . o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o . . . . . . . o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o . . . . . o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o . . . . . o o o o o o ■ P . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o . . . o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o . . . o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o . o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o ■ o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o P o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapResult1, mapString1);

            //Throw grenade in front of wall
            Vector3 targetThrowingLocation = new(13, 0, 10);
            EncounterResult encounter1 = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, team2.Characters, mission.Map, diceRolls, targetThrowingLocation);
            string log1 = @"
Fred is attacking with area effect Grenade aimed at <13, 0, 10>
Characters in affected area: Jeff
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 3-4, (dice roll: 100)
4 damage dealt to character Jeff, HP is now 1
10 XP added to character Fred, for a total of 10 XP
Cover removed from <14, 0, 8>
Cover removed from <14, 0, 7>
Cover removed from <14, 0, 12>
Cover removed from <14, 0, 13>
Cover removed from <14, 0, 9>
Cover removed from <14, 0, 11>
Cover removed from <14, 0, 10>
";
            Assert.AreEqual(log1, encounter1.LogString);

            string mapString2 = fred.GetCharactersInViewMapString(mission.Map, new List<Team> { team2 });
            List<Character> characters2 = fred.GetCharactersInView(mission.Map, new List<Team>() { team2 });
            Assert.AreEqual(1, characters2.Count);
            string mapResult2 = @"
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
. . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o o . o o . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o . o o o . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . o o o o o o o o o . o o o o o . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . o o o o o o o o . o o o o o o o . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . o o o o o o o o . o o o o o o o o . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . o o o o o o o . o o o o o o o o o o . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o . o o o o o o o o o o o . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . o o o o o o o . o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . o o o o o o . o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . 
. . . . . . . . . . . o o o o o o . o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . 
. . . . . . . . . . . o o o o o . o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . 
. . . . . . . . . . o o o o o . o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . 
o . . . . . . . . . o o o o ■ o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . 
o o . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . 
o o . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o . . . . . o o o o o o o P o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . 
o o o o . . . o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . 
o o o o . . . o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o . o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o ■ o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o P o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapResult2, mapString2);

            //End of of battle
            mission.EndMission();

            //Assert
            Assert.AreEqual(1, fred.MissionsCompleted);
            Assert.AreEqual(1, jeff.MissionsCompleted);
            Assert.AreEqual(0, fred.TotalShots);
            Assert.AreEqual(1, fred.TotalHits);
            Assert.AreEqual(0, fred.TotalKills);
            Assert.AreEqual(4, fred.TotalDamage);
            Assert.AreEqual(0, jeff.TotalShots);
            Assert.AreEqual(0, jeff.TotalHits);
            Assert.AreEqual(0, jeff.TotalKills);
            Assert.AreEqual(0, jeff.TotalDamage);
        }

    }
}

