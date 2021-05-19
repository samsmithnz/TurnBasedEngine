﻿using Battle.Logic.Characters;
using Battle.Logic.Encounters;
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

            PathResult pathResult = Path.FindPath(jeff.Location, destination, map);
            List<EncounterResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV });

            //Assert
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(0, jeff.Hitpoints);
            Assert.AreEqual(new(8, 0, 7), jeff.Location);
            Assert.AreEqual(100, fred.Experience); 
            Assert.AreEqual(1, movementResults.Count); 
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 56, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, movementResults[0].LogString);
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

            PathResult pathResult = Path.FindPath(jeff.Location, destination, map);
            List<EncounterResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV });

            //Assert
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(12, jeff.Hitpoints);
            Assert.AreEqual(destination, jeff.Location);
            Assert.AreEqual(0, fred.Experience);
            Assert.AreEqual(1, movementResults.Count);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 56, (dice roll: 0)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, movementResults[0].LogString);
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

            PathResult pathResult = Path.FindPath(jeff.Location, destination, map);
            List<EncounterResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV, harryFOV });

            //Assert
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(0, jeff.Hitpoints);
            Assert.AreEqual(new(8, 0, 7), jeff.Location);
            Assert.AreEqual(100, fred.Experience);
            Assert.AreEqual(10, harry.Experience);
            Assert.AreEqual(1, movementResults.Count);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 56, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, movementResults[0].LogString);
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

            PathResult pathResult = Path.FindPath(jeff.Location, destination, map);
            List<EncounterResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathResult.Path, diceRolls, new() { fredFOV });

            //Assert
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(12, jeff.Hitpoints);
            Assert.AreEqual(destination, jeff.Location);
            Assert.AreEqual(0, fred.Experience);
            Assert.AreEqual(0, harry.Experience);
            Assert.AreEqual(1, movementResults.Count);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 56, (dice roll: 0)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, movementResults[0].LogString);
        }
    }
}