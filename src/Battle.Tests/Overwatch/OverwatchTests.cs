using Battle.Logic.Characters;
using Battle.Logic.FieldOfView;
using Battle.Logic.Movement;
using Battle.Logic.PathFinding;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Overwatch
{
    [TestClass]
    [TestCategory("L0")]
    public class OverwatchTests
    {
        [TestMethod]
        public void FredInOverwatchKillsWhileJeffMoves()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.InOverwatch = true;
            //Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(6, 0, 0);
            Queue<int> diceRolls = new(new List<int>  { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfViewCalculator.GetFieldOfView(map, fred.Location, fred.Range);
            KeyValuePair<Character, List<Vector3>> fredFOV = new(fred, fov);

            Path jeffPath = new(jeff.Location, destination, map);
            PathResult pathResult = jeffPath.FindPath();
            jeff = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV });

            //Assert
            Assert.IsTrue(jeffPath != null);
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(0, jeff.Hitpoints);
            Assert.AreEqual(new(8, 0, 7), jeff.Location);
            Assert.AreEqual(100, fred.Experience);
        }

        [TestMethod]
        public void FredInOverwatchMissesWhileJeffMoves()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.InOverwatch = true;
            //Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(6, 0, 0);
            Queue<int> diceRolls = new(new List<int>  { 0, 1, 2, 3, 4, 5, 0, 1 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfViewCalculator.GetFieldOfView(map, fred.Location, fred.Range);
            KeyValuePair<Character, List<Vector3>> fredFOV = new(fred, fov);

            Path jeffPath = new(jeff.Location, destination, map);
            PathResult pathResult = jeffPath.FindPath();
            jeff = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV });

            //Assert
            Assert.IsTrue(jeffPath != null);
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(12, jeff.Hitpoints);
            Assert.AreEqual(destination, jeff.Location);
            Assert.AreEqual(0, fred.Experience);
        }

        [TestMethod]
        public void FredAndHarryInOverwatchKillsWhileJeffMoves()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.InOverwatch = true;
            Character harry = CharacterPool.CreateHarryHeroSidekick();
            harry.InOverwatch = true;
            Character jeff = CharacterPool.CreateJeffBaddie();
            jeff.Hitpoints = 25;
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(6, 0, 0);
            Queue<int> diceRolls = new(new List<int>  { 100, 100, 100, 100, 100, 100, 0, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fovFred = FieldOfViewCalculator.GetFieldOfView(map, fred.Location, fred.Range);
            KeyValuePair<Character, List<Vector3>> fredFOV = new(fred, fovFred);
            List<Vector3> fovHarry = FieldOfViewCalculator.GetFieldOfView(map, harry.Location, harry.Range);
            KeyValuePair<Character, List<Vector3>> harryFOV = new(harry, fovHarry);

            Path jeffPath = new(jeff.Location, destination, map);
            PathResult pathResult = jeffPath.FindPath();
            jeff = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV, harryFOV });

            //Assert
            Assert.IsTrue(jeffPath != null);
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(0, jeff.Hitpoints);
            Assert.AreEqual(new(8, 0, 7), jeff.Location);
            Assert.AreEqual(100, fred.Experience);
            Assert.AreEqual(10, harry.Experience);
        }

        [TestMethod]
        public void FredAndHarryInOverwatchMissesWhileJeffMoves()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.InOverwatch = true;
            Character harry = CharacterPool.CreateHarryHeroSidekick();
            harry.InOverwatch = true;
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(6, 0, 0);
            Queue<int> diceRolls = new(new List<int>  { 0, 1, 2, 3, 4, 5, 0, 1 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfViewCalculator.GetFieldOfView(map, fred.Location, fred.Range);
            KeyValuePair<Character, List<Vector3>> fredFOV = new(fred, fov);

            Path jeffPath = new(jeff.Location, destination, map);
            PathResult pathResult = jeffPath.FindPath();
            jeff = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV });

            //Assert
            Assert.IsTrue(jeffPath != null);
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(12, jeff.Hitpoints);
            Assert.AreEqual(destination, jeff.Location);
            Assert.AreEqual(0, fred.Experience);
            Assert.AreEqual(0, harry.Experience);
        }
    }
}
