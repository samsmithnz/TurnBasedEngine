using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.MainGame;
using Battle.Logic.Weapons;
using Battle.Tests.Characters;
using Battle.Tests.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Scenarios
{
    [TestClass]
    [TestCategory("L0")]
    public class ScenarioTests
    {
        //Turn 1 - Team 1 starts
        //Fred runs to cover
        //Fred shoots at Jeff, who is behind high cover. He hits him. 

        //Turn 1 - Team 2 starts
        //Jeff shoots back and misses

        //Turn 2 - Team 1 starts
        //Fred shoots again, and kills Jeff.

        //End of of battle

        [TestMethod]
        public void SimpleScenarioMultipleTurnsTest()
        {
            //Arrange
            Game game = new();
            game.TurnNumber = 1;
            game.Map = GenerateMap(50, 50);

            Team team1 = new()
            {
                Name = "Player",
                Characters = new() { CharacterPool.CreateJeffBaddie() }
            };
            game.Teams.Add(team1);

            Team team2 = new()
            {
                Name = "Enemy",
                Characters = new() { CharacterPool.CreateFredHero() }
            };
            game.Teams.Add(team2);

            //Act

            //Assert
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

