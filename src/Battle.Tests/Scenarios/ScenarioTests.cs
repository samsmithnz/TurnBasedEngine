using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.MainGame;
using Battle.Logic.Movement;
using Battle.Logic.PathFinding;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battle.Tests.Scenarios
{
    [TestClass]
    [TestCategory("L0")]
    public class ScenarioTests
    {
        [TestMethod]
        public void SimpleScenarioMultipleTurnsTest()
        {
            //Arrange
            Game game = new();
            game.TurnNumber = 1;
            game.Map = GenerateMap(50, 50);
            game.Map[10, 10] = "W";
            game.Map[20, 10] = "W";
            Team team1 = new()
            {
                Name = "Player",
                Characters = new() { CharacterPool.CreateFredHero() }
            };
            team1.Characters[0].Location = new(5, 0, 5);
            game.Teams.Add(team1);

            Team team2 = new()
            {
                Name = "Enemy",
                Characters = new() { CharacterPool.CreateJeffBaddie() }
            };
            team2.Characters[0].Location = new(20, 0, 11);
            game.Teams.Add(team2);
            Queue<int> diceRolls = new(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll


            //Assert - Setup
            Assert.AreEqual(1, game.TurnNumber);
            Assert.AreEqual(2, game.Teams.Count);
            Assert.AreEqual(50 * 50, game.Map.Length);
            Assert.AreEqual("Player", game.Teams[0].Name);
            Assert.AreEqual(1, game.Teams[0].Characters.Count);
            Assert.AreEqual("Enemy", game.Teams[1].Name);
            Assert.AreEqual(1, game.Teams[1].Characters.Count);

            //Act

            //Turn 1 - Team 1 starts
            //Fred runs to cover
            PathResult pathResult = Path.FindPath(team1.Characters[0].Location, new(9, 0, 10), game.Map);
            CharacterMovement.MoveCharacter(team1.Characters[0], game.Map, pathResult.Path, diceRolls, null);

            //Fred shoots at Jeff, who is behind high cover. He hits him. 
            List<Character> characters = game.Teams[0].Characters[0].GetCharactersInView(game.Map, new List<Team>() { team2 });
            EncounterResult encounter1 = Encounter.AttackCharacter(game.Teams[0].Characters[0],
                    game.Teams[0].Characters[0].WeaponEquipped,
                    game.Teams[1].Characters[0],
                    game.Map,
                    diceRolls);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 20, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log1, encounter1.LogString);

            //Turn 1 - Team 2 starts
            //Jeff shoots back and misses
            List<Character> characters2 = game.Teams[1].Characters[0].GetCharactersInView(game.Map, new List<Team>() { team1 });
            EncounterResult encounter2 = Encounter.AttackCharacter(game.Teams[1].Characters[0],
                    game.Teams[1].Characters[0].WeaponEquipped,
                    game.Teams[0].Characters[0],
                    game.Map,
                    diceRolls);
            string log2 = @"
Jeff is attacking with Shotgun, targeted on Fred
Missed: Chance to hit: 28, (dice roll: 0)
0 XP added to character Jeff, for a total of 0 XP
";
            Assert.AreEqual(log2, encounter2.LogString);

            //Turn 2 - Team 1 starts
            //Fred shoots again, and kills Jeff.
            List<Character> characters3 = game.Teams[0].Characters[0].GetCharactersInView(game.Map, new List<Team>() { team2 });
            EncounterResult encounter3 = Encounter.AttackCharacter(game.Teams[0].Characters[0],
                    game.Teams[0].Characters[0].WeaponEquipped,
                    game.Teams[1].Characters[0],
                    game.Map,
                    diceRolls);
            string log3 = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 20, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now -5
Jeff is killed
100 XP added to character Fred, for a total of 110 XP
";
            Assert.AreEqual(log3, encounter3.LogString);

            //End of of battle

            //Assert
            Assert.AreEqual(-5,game.Teams[1].Characters[0].Hitpoints);
        }

        private static string[,] GenerateMap(int xMax, int zMax)
        {
            string[,] map = new string[xMax, zMax];

            //Initialize the map
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    map[x, z] = "";
                }
            }

            return map;
        }

    }
}

