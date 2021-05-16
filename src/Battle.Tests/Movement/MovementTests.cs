using Battle.Logic.Characters;
using Battle.Logic.Movement;
using Battle.Logic.PathFinding;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Movement
{
    [TestClass]
    [TestCategory("L0")]
    public class MovementTests
    {
        [TestMethod]
        public void MovementTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(8, 0, 0);
            Queue<int> diceRolls = new(new List<int>  { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            Path path = new(fred.Location, destination, map);
            PathResult pathResult = path.FindPath();
            fred = CharacterMovement.MoveCharacter(fred, map, pathResult.Path, diceRolls);

            //Assert
            Assert.IsTrue(path != null);
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(destination, fred.Location);
            Assert.AreEqual(8, pathResult.Path.Count);
        }
    }
}
