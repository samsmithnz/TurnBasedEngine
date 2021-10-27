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
    [TestCategory("L2")]
    public class OverwatchScenarioTest
    {
        [TestMethod]
        public void FredInOverwatchShootsAtJethroTest()
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
            fred.MobilityRange = 8;
            Team team1 = new Team(1)
            {
                Name = "Good guys",
                Characters = new List<Character>() { fred }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new Vector3(19, 0, 19));
            jethro.ActionPointsCurrent = 2;
            Team team2 = new Team(0)
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jethro }
            };
            mission.Teams.Add(team2);
            mission.StartMission();

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
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = mission.GetMovementPossibleTiles(fred);
            string mapMovementString = MapCore.GetMapStringWithItems(mission.Map, MovementPossibileTiles.ExtractVectorListFromKeyValuePair(movementPossibileTiles));
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

            //Fred aims at Jethro, who is behind high cover. 
            Assert.AreEqual(fred.TargetCharacters[0], jethro.Name);

            int chanceToHit = EncounterCore.GetChanceToHit(fred, fred.WeaponEquipped, jethro);
            int chanceToCrit = EncounterCore.GetChanceToCrit(mission.Map, fred, fred.WeaponEquipped, jethro, false);
            DamageRange damageOptions = EncounterCore.GetDamageRange(fred, fred.WeaponEquipped);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);

            //Fred shoots at Jethro, and misses 
            EncounterResult encounter1 = mission.AttackCharacter(fred,
                   fred.WeaponEquipped,
                   jethro,
                   team1,
                   team2);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 80, (dice roll: 8)
Low cover downgraded to no cover at <5, 0, 5>
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log1, encounter1.LogString);
            Assert.AreEqual(new Vector3(5, 0, 5), encounter1.MissedLocation);

            //Fred shoots at Jethro, and hits him. 
            EncounterResult encounter2 = mission.AttackCharacter(fred,
               fred.WeaponEquipped,
               jethro,
               team1,
               team2);
            mission.EndMission();
            string log2 = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 81)
Damage range: 3-5, (dice roll: 76)
Critical chance: 70, (dice roll: 55)
Critical damage range: 8-12, (dice roll: 76)
11 damage dealt to character Jethro, HP is now -7
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log2, encounter2.LogString);

        }
    }
}
