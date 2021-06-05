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
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new(8, 0, 0);
            Queue<int> diceRolls = new(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTiles)
            {
                if (item == destination)
                {
                    destinationCheck = item;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult pathFindingResult = PathFinding.FindPath(fred.Location, destination, map);
            List<EncounterResult> movementResults = CharacterMovement.MoveCharacter(fred, map, pathFindingResult, diceRolls);

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(destination, fred.Location);
            Assert.AreEqual(8, pathFindingResult.Path.Count);
            Assert.AreEqual(0, movementResults.Count);
        }

        [TestMethod]
        public void MovementInRangeTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new(8, 0, 0);

            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange);
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
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new(8, 0, 1);

            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange);
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
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new(7, 0, 1);

            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange);
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
        public void MovementRange8Test()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(20, 0, 20);
            string[,,] map = MapUtility.InitializeMap(40, 1, 40);
            Vector3 destination = new(12, 0, 20);

            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTiles)
            {
                if (item == destination)
                {
                    destinationCheck = item;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult pathFindingResult = PathFinding.FindPath(fred.Location, destination, map);
            CharacterMovement.MoveCharacter(fred, map, pathFindingResult, null);

            string mapResult = MapCore.GetMapStringWithItems(map, movementPossibileTiles);
            string mapExpected = @"
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . o o o o o o o o o o o . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . o o o o o o o o o o o o o . . . . . . . . . . . . . 
. . . . . . . . . . . . . . o o o o o o o o o o o o o . . . . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o o o o o o o o o . . . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o o o o o o o o o . . . . . . . . . . . . 
. . . . . . . . . . . . o o o o o o o o . o o o o o o o o . . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o o o o o o o o o . . . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o o o o o o o o o . . . . . . . . . . . . 
. . . . . . . . . . . . . . o o o o o o o o o o o o o . . . . . . . . . . . . . 
. . . . . . . . . . . . . . o o o o o o o o o o o o o . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . o o o o o o o o o o o . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
";

            //Assert
            Assert.AreEqual(mapExpected, mapResult);
            Assert.AreEqual(1, fred.ActionPointsCurrent);
        }

        [TestMethod]
        public void MovementRange16Test()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(20, 0, 20);
            string[,,] map = MapUtility.InitializeMap(40, 1, 40);
            Vector3 destination = new(6, 0, 20);

            //Act
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, 16);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTiles)
            {
                if (item == destination)
                {
                    destinationCheck = item;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult pathFindingResult = PathFinding.FindPath(fred.Location, destination, map);
            CharacterMovement.MoveCharacter(fred, map, pathFindingResult, null);

            string mapResult = MapCore.GetMapStringWithItems(map, movementPossibileTiles);
            string mapExpected = @"
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o o o o o o o o o . . . . . . . . . . . . 
. . . . . . . . . . . o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
. . . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . 
. . . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . 
. . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . 
. . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . 
. . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . 
. . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . 
. . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . 
. . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . 
. . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . 
. . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . 
. . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . 
. . . . o o o o o o o o o o o o o o o o . o o o o o o o o o o o o o o o o . . . 
. . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . 
. . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . 
. . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . 
. . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . 
. . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . 
. . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . 
. . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . 
. . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . 
. . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . 
. . . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . 
. . . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . 
. . . . . . . . . . . o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o o o o o o o o o . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
";

            //Assert
            Assert.AreEqual(mapExpected, mapResult);
            Assert.AreEqual(0, fred.ActionPointsCurrent);
        }

        [TestMethod]
        public void MovementRange8AndRange16LayedTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(20, 0, 20);
            string[,,] map = MapUtility.InitializeMap(40, 1, 40);
            Vector3 destination = new(6, 0, 20);

            //Act
            List<Vector3> movementPossibileTilesRange8 = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, 8);
            List<Vector3> movementPossibileTilesRange16 = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, 16);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (Vector3 item in movementPossibileTilesRange16)
            {
                if (item == destination)
                {
                    destinationCheck = item;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult pathFindingResult = PathFinding.FindPath(fred.Location, destination, map);
            CharacterMovement.MoveCharacter(fred, map, pathFindingResult, null);

            string mapResult = MapCore.GetMapStringWithItemLayers(map, movementPossibileTilesRange16, movementPossibileTilesRange8);
            string mapExpected = @"
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o o o o o o o o o . . . . . . . . . . . . 
. . . . . . . . . . . o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
. . . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . 
. . . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . 
. . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . 
. . . . . . . . o o o o o o o o o o o o O o o o o o o o o o o o o . . . . . . . 
. . . . . . . o o o o o o o o o o o O O O O O o o o o o o o o o o o . . . . . . 
. . . . . . . o o o o o o o o o O O O O O O O O O o o o o o o o o o . . . . . . 
. . . . . . . o o o o o o o o O O O O O O O O O O O o o o o o o o o . . . . . . 
. . . . . . o o o o o o o o O O O O O O O O O O O O O o o o o o o o o . . . . . 
. . . . . . o o o o o o o o O O O O O O O O O O O O O o o o o o o o o . . . . . 
. . . . . o o o o o o o o O O O O O O O O O O O O O O O o o o o o o o o . . . . 
. . . . . o o o o o o o o O O O O O O O O O O O O O O O o o o o o o o o . . . . 
. . . . o o o o o o o o O O O O O O O O . O O O O O O O O o o o o o o o o . . . 
. . . . . o o o o o o o o O O O O O O O O O O O O O O O o o o o o o o o . . . . 
. . . . . o o o o o o o o O O O O O O O O O O O O O O O o o o o o o o o . . . . 
. . . . . . o o o o o o o o O O O O O O O O O O O O O o o o o o o o o . . . . . 
. . . . . . o o o o o o o o O O O O O O O O O O O O O o o o o o o o o . . . . . 
. . . . . . . o o o o o o o o O O O O O O O O O O O o o o o o o o o . . . . . . 
. . . . . . . o o o o o o o o o O O O O O O O O O o o o o o o o o o . . . . . . 
. . . . . . . o o o o o o o o o o o O O O O O o o o o o o o o o o o . . . . . . 
. . . . . . . . o o o o o o o o o o o o O o o o o o o o o o o o o . . . . . . . 
. . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . 
. . . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . 
. . . . . . . . . o o o o o o o o o o o o o o o o o o o o o o o . . . . . . . . 
. . . . . . . . . . . o o o o o o o o o o o o o o o o o o o . . . . . . . . . . 
. . . . . . . . . . . . . o o o o o o o o o o o o o o o . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . o o o o o o o o o . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . o o o o o . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . o . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
";

            //Assert
            Assert.AreEqual(mapExpected, mapResult);
            Assert.AreEqual(0, fred.ActionPointsCurrent);
        }
    }
}
