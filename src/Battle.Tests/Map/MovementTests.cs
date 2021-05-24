using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class MovementTests
    {
        [TestMethod]
        public void MovementWithNoOverwatchTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(8, 0, 0);
            Queue<int> diceRolls = new(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(fred.Location, destination, map);
            List<EncounterResult> movementResults = CharacterMovement.MoveCharacter(fred, map, PathFindingResult.Path, diceRolls);

            //Assert
            Assert.IsTrue(PathFindingResult != null);
            Assert.AreEqual(destination, fred.Location);
            Assert.AreEqual(8, PathFindingResult.Path.Count);
            Assert.AreEqual(0, movementResults.Count);
        }
    }
}
