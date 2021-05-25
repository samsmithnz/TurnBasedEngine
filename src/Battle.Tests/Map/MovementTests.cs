using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MovementRange);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTiles)
            {
                if (item == destination)
                {
                    destinationCheck = item;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult PathFindingResult = PathFinding.FindPath(fred.Location, destination, map);
            List<EncounterResult> movementResults = CharacterMovement.MoveCharacter(fred, map, PathFindingResult.Path, diceRolls);

            //Assert
            Assert.IsTrue(PathFindingResult != null);
            Assert.AreEqual(destination, fred.Location);
            Assert.AreEqual(8, PathFindingResult.Path.Count);
            Assert.AreEqual(0, movementResults.Count);
        }

        [TestMethod]
        public void MovementInRangeTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(8, 0, 0);
            
            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MovementRange);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTiles)
            {
                if (item == destination)
                {
                    destinationCheck = item;
                }
            }

            //Assert
            Assert.AreEqual(destination, destinationCheck);
        }

        [TestMethod]
        public void MovementOutOfRangeTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(8, 0, 1);

            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MovementRange);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTiles)
            {
                if (item == destination)
                {
                    destinationCheck = item;
                }
            }

            //Assert
            Assert.AreEqual(Vector3.Zero, destinationCheck);
        }

        [TestMethod]
        public void MovementJustInRangeTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(7, 0, 1);

            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MovementRange);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTiles)
            {
                if (item == destination)
                {
                    destinationCheck = item;
                }
            }

            //Assert
            Assert.AreEqual(destination, destinationCheck);
        }
    }
}
