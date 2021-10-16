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
    public class CoverScenarioTest
    {
        //Enemy is behind cover and not visible.
        //Throw grenade to destory cover and make him visible.
        [TestMethod]
        public void JethroIsHidingBehindCoverScenarioTest()
        {
            //Arrange
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[5, 0, 6] = CoverType.FullCover;
            mission.Map[14, 0, 5] = CoverType.FullCover;
            mission.Map[14, 0, 6] = CoverType.FullCover;
            mission.Map[14, 0, 7] = CoverType.HalfCover; //half cover here!
            mission.Map[14, 0, 8] = CoverType.FullCover;
            mission.Map[14, 0, 9] = CoverType.FullCover;
            mission.Map[14, 0, 10] = CoverType.FullCover;
            mission.Map[14, 0, 11] = CoverType.FullCover;
            mission.Map[14, 0, 12] = CoverType.FullCover;
            mission.Map[14, 0, 13] = CoverType.FullCover;
            mission.Map[14, 0, 14] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(5, 0, 5));
            Team team1 = new Team()
            {
                Name = "Good guys",
                Characters = new List<Character>() { fred }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new Vector3(15, 0, 10));
            jethro.HitpointsCurrent = 5;
            Team team2 = new Team()
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jethro }
            };
            mission.Teams.Add(team2);
            RandomNumberQueue diceRolls = new RandomNumberQueue(new List<int> { 100, 100, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll


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
            //Fred cannot see Jethro, who is hiding behind cover
            string mapString1 = fred.GetCharactersInViewMapString(mission.Map, new List<Team> { team2 });
            List<Character> characters = fred.GetCharactersInRangeWithCurrentWeapon(mission.Map, new List<Team>() { team2 });
            Assert.AreEqual(0, characters.Count);
            string mapStringExpected1 = @"
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
o o . . . . . . . o o o o o ■ . . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . 
o o . . . . . . . o o o o o ■ . . . . . . . . . . . . . . . o o o o o o . . . . . . . . . . . . . . 
o o o . . . . . o o o o o o ■ . . . . . . . . . . . o o o o o o o o o o . . . . . . . . . . . . . . 
o o o . . . . . o o o o o o ■ P . . . . . . o o o o o o o o o o o . . . . . . . . . . . . . . . . . 
o o o o . . . o o o o o o o ■ . . . . o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . 
o o o o . . . o o o o o o o ■ o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o . o o o o o o o o □ o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o ■ o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o P o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapStringExpected1, mapString1);

            //Throw grenade in front of wall
            Vector3 targetThrowingLocation = new Vector3(13, 0, 10);
            EncounterResult encounter1 = Encounter.AttackCharacterWithAreaOfEffect(mission.Map, fred, fred.UtilityWeaponEquipped, team2.Characters, diceRolls, targetThrowingLocation);
            string log1 = @"
Fred is attacking with area effect Grenade aimed at <13, 0, 10>
Characters in affected area: Jethro
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 3-4, (dice roll: 100)
4 damage dealt to character Jethro, HP is now 1
10 XP added to character Fred, for a total of 10 XP
High cover downgraded to low cover at <14, 0, 8>
Low cover downgraded to no cover at <14, 0, 7>
High cover downgraded to low cover at <14, 0, 12>
High cover downgraded to low cover at <14, 0, 13>
High cover downgraded to low cover at <14, 0, 9>
High cover downgraded to low cover at <14, 0, 11>
High cover downgraded to low cover at <14, 0, 10>
";
            Assert.AreEqual(log1, encounter1.LogString);
            Assert.AreEqual(7, encounter1.AffectedMap.Count);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new Vector3(14, 0, 8), 1), encounter1.AffectedMap[0]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new Vector3(14, 0, 7), 1), encounter1.AffectedMap[1]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new Vector3(14, 0, 12), 1), encounter1.AffectedMap[2]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new Vector3(14, 0, 13), 1), encounter1.AffectedMap[3]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new Vector3(14, 0, 9), 1), encounter1.AffectedMap[4]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new Vector3(14, 0, 11), 1), encounter1.AffectedMap[5]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new Vector3(14, 0, 10), 1), encounter1.AffectedMap[6]);

            string mapString2 = fred.GetCharactersInViewMapString(mission.Map, new List<Team> { team2 });
            List<Character> characters2 = fred.GetCharactersInRangeWithCurrentWeapon(mission.Map, new List<Team>() { team2 });
            Assert.AreEqual(1, characters2.Count);
            string mapStringExpected2 = @"
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
o o . . . . . . . o o o o o □ o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . 
o o . . . . . . . o o o o o □ o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o . . . . . o o o o o o □ o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o . . . . . o o o o o o □ P o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . 
o o o o . . . o o o o o o o □ o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . 
o o o o . . . o o o o o o o □ o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o . o o o o o o o o . o o o . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o ■ o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o P o o o o o o o o ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapStringExpected2, mapString2);

            //End of of battle
            mission.EndMission();

            //Assert
            Assert.AreEqual(1, fred.MissionsCompleted);
            Assert.AreEqual(1, jethro.MissionsCompleted);
            Assert.AreEqual(0, fred.TotalShots);
            Assert.AreEqual(1, fred.TotalHits);
            Assert.AreEqual(0, fred.TotalKills);
            Assert.AreEqual(4, fred.TotalDamage);
            Assert.AreEqual(0, jethro.TotalShots);
            Assert.AreEqual(0, jethro.TotalHits);
            Assert.AreEqual(0, jethro.TotalKills);
            Assert.AreEqual(0, jethro.TotalDamage);
        }
    }
}
