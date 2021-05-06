using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class MapGenerationTests
    {
        [TestMethod]
        public void RandomMapTest()
        {
            //Arrange
            int xMax = 10;
            int zMax = 10;
            int probability = 50;
            string[,] map = InitializeMap(xMax, zMax);

            //Act
            map = MapGeneration.GenerateRandomMap(map, xMax, zMax, probability);
            MapGeneration.DebugPrintOutMap(map, xMax, zMax);

            //Assert
            Assert.AreEqual(100, map.Length);
        }

        private static string[,] InitializeMap(int xMax, int zMax)
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
