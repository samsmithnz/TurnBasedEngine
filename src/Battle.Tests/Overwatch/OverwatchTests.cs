using Battle.Logic.Characters;
using Battle.Logic.Weapons;
using Battle.PathFinding;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Battle.Tests.Overwatch
{
    [TestClass]
    [TestCategory("L0")]
    public class OverwatchTests
    {
        [TestMethod]
        public void RandomMapTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            //Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(7, 0, 7);

            //Act
            Path path = new(jeff.Location, destination, map);

            //Assert
            Assert.IsTrue(path != null);
        }
    }
}
