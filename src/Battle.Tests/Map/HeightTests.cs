using Battle.Logic.Characters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class HeightTests
    {
        [TestMethod]
        public void HeightTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(10, 2, 10, ".")
            };
            //Move the top 4 rows of the map to be on Y=1, instead of Y=0
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        if (z <= 5 && y == 0)
                        {
                            mission.Map[x, y, z] = "";
                        }
                        else if (z > 5 && y == 1)
                        {
                            mission.Map[x, y, z] = "";
                        }
                    }
                }
            }
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(1, 0, 1));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(8, 0, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            mission.Teams.Add(team1);
            mission.Teams.Add(team2);
            //mission.StartMission();

            //Assert
            string currentMap = MapCore.GetMapString(mission.Map);
            string expectedMap = @"
. . . . . . . . . . 
. . . . . . . . P . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. P . . . . . . . . 
. . . . . . . . . . 
";
            Assert.AreEqual(expectedMap, currentMap);
        }

    }
}
