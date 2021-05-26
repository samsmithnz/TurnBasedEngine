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
            string[,,] map = MapUtility.InitializeMap(xMax, 1, zMax);

            //Act
            map = MapGeneration.GenerateRandomMap(map, probability);
            MapGeneration.DebugPrintOutMap(map);

            //Assert
            Assert.AreEqual(100, map.Length);
        }

    }
}
