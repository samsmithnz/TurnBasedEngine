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
            Character fred = CharacterPool.CreateFred();
            fred.InOverwatch = true;
            //Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(6, 0, 0);
            List<int> diceRolls = new() { 65, 100, 100 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfViewCalculator.GetFieldOfView(map, fred.Location, fred.Range);
            KeyValuePair<Character, List<Vector3>> fredFOV = new(fred, fov);

            Path jeffPath = new(jeff.Location, destination, map);
            PathResult pathResult = jeffPath.FindPath();
            jeff = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV });

            //Assert
            Assert.IsTrue(jeffPath != null);
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(0, jeff.HP);
            Assert.AreEqual(new(8, 0, 7), jeff.Location);
            Assert.AreEqual(100, fred.Experience);
        }
        [TestMethod]
        public void FredInOverwatchMissesWhileJeffMoves()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.InOverwatch = true;
            //Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(6, 0, 0);
            List<int> diceRolls = new() { 0, 0, 0}; //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfViewCalculator.GetFieldOfView(map, fred.Location, fred.Range);
            KeyValuePair<Character, List<Vector3>> fredFOV = new(fred, fov);

            Path jeffPath = new(jeff.Location, destination, map);
            PathResult pathResult = jeffPath.FindPath();
            jeff = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV });

            //Assert
            Assert.IsTrue(jeffPath != null);
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(12, jeff.HP);
            Assert.AreEqual(destination, jeff.Location);
            Assert.AreEqual(0, fred.Experience);
        }
    }
}
