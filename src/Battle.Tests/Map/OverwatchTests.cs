using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
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
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, fred.Location, fred.ShootingRange);
            KeyValuePair<Character, List<Vector3>> fredFOV = new KeyValuePair<Character, List<Vector3>>(fred, fov);

            PathFindingResult pathFindingResult = PathFinding.FindPath(jeff.Location, destination, map);
            List<ActionResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathFindingResult, diceRolls, new List<KeyValuePair<Character, List<Vector3>>>() { fredFOV });

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(-8, jeff.HitpointsCurrent);
            Assert.AreEqual(new Vector3(8, 0, 7), jeff.Location);
            Assert.AreEqual(100, fred.Experience);
            Assert.AreEqual(1, movementResults.Count);
            string log = @"
Jeff is moving from <8, 0, 8> to <8, 0, 7>
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 56, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now -8
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, ActionResultLog.LogString(movementResults));
        }

        [TestMethod]
        public void FredInOverwatchWithOpportunistKillsWhileJeffMoves()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.InOverwatch = true;
            fred.Abilities.Add(AbilityPool.OpportunistAbility());
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, fred.Location, fred.ShootingRange);
            KeyValuePair<Character, List<Vector3>> fredFOV = new KeyValuePair<Character, List<Vector3>>(fred, fov);

            PathFindingResult pathFindingResult = PathFinding.FindPath(jeff.Location, destination, map);
            List<ActionResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathFindingResult, diceRolls, new List<KeyValuePair<Character, List<Vector3>>>() { fredFOV });

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(-8, jeff.HitpointsCurrent);
            Assert.AreEqual(new Vector3(8, 0, 7), jeff.Location);
            Assert.AreEqual(100, fred.Experience);
            Assert.AreEqual(1, movementResults.Count);
            string log = @"
Jeff is moving from <8, 0, 8> to <8, 0, 7>
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now -8
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, ActionResultLog.LogString(movementResults));
        }

        [TestMethod]
        public void FredInOverwatchMissesWhileJeffMoves()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.InOverwatch = true;
            //Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Queue<int> diceRolls = new Queue<int>(new List<int> { 0, 1, 2, 3, 4, 5, 0, 1 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, fred.Location, fred.ShootingRange);
            KeyValuePair<Character, List<Vector3>> fredFOV = new KeyValuePair<Character, List<Vector3>>(fred, fov);

            PathFindingResult pathFindingResult = PathFinding.FindPath(jeff.Location, destination, map);
            List<ActionResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathFindingResult, diceRolls, new List<KeyValuePair<Character, List<Vector3>>>() { fredFOV });

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(4, jeff.HitpointsCurrent);
            Assert.AreEqual(destination, jeff.Location);
            Assert.AreEqual(0, fred.Experience);
            Assert.AreEqual(8, movementResults.Count);
            string log = @"
Jeff is moving from <8, 0, 8> to <8, 0, 7>
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 56, (dice roll: 0)
0 XP added to character Fred, for a total of 0 XP
Jeff is moving from <8, 0, 7> to <8, 0, 6>
Jeff is moving from <8, 0, 6> to <8, 0, 5>
Jeff is moving from <8, 0, 5> to <8, 0, 4>
Jeff is moving from <8, 0, 4> to <7, 0, 3>
Jeff is moving from <7, 0, 3> to <7, 0, 2>
Jeff is moving from <7, 0, 2> to <6, 0, 1>
Jeff is moving from <6, 0, 1> to <6, 0, 0>
";
            Assert.AreEqual(log, ActionResultLog.LogString(movementResults));
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
            jeff.HitpointsCurrent = 25;
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 100, 100, 100, 100, 100, 0, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fovFred = FieldOfView.GetFieldOfView(map, fred.Location, fred.ShootingRange);
            KeyValuePair<Character, List<Vector3>> fredFOV = new KeyValuePair<Character, List<Vector3>>(fred, fovFred);
            List<Vector3> fovHarry = FieldOfView.GetFieldOfView(map, harry.Location, harry.ShootingRange);
            KeyValuePair<Character, List<Vector3>> harryFOV = new KeyValuePair<Character, List<Vector3>>(harry, fovHarry);

            PathFindingResult pathFindingResult = PathFinding.FindPath(jeff.Location, destination, map);
            List<ActionResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathFindingResult, diceRolls, new List<KeyValuePair<Character, List<Vector3>>>() { fredFOV, harryFOV });

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(0, jeff.HitpointsCurrent);
            Assert.AreEqual(new Vector3(8, 0, 7), jeff.Location);
            Assert.AreEqual(100, fred.Experience);
            Assert.AreEqual(10, harry.Experience);
            Assert.AreEqual(1, movementResults.Count);
            Assert.AreEqual(2, movementResults[0].EncounterResults.Count);
            string log = @"
Jeff is moving from <8, 0, 8> to <8, 0, 7>
Harry is attacking with Sniper Rifle, targeted on Jeff
Hit: Chance to hit: 42, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 9-13, (dice roll: 100)
13 damage dealt to character Jeff, HP is now 12
10 XP added to character Harry, for a total of 10 XP
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
            Assert.AreEqual(log, ActionResultLog.LogString(movementResults));

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
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Queue<int> diceRolls = new Queue<int>(new List<int> { 0, 1, 2, 3, 4, 5, 0, 1 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, fred.Location, fred.ShootingRange);
            KeyValuePair<Character, List<Vector3>> fredFOV = new KeyValuePair<Character, List<Vector3>>(fred, fov);

            PathFindingResult pathFindingResult = PathFinding.FindPath(jeff.Location, destination, map);
            List<ActionResult> movementResults = CharacterMovement.MoveCharacter(jeff, map, pathFindingResult, diceRolls, new List<KeyValuePair<Character, List<Vector3>>>() { fredFOV });

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(4, jeff.HitpointsCurrent);
            Assert.AreEqual(destination, jeff.Location);
            Assert.AreEqual(0, fred.Experience);
            Assert.AreEqual(0, harry.Experience);
            Assert.AreEqual(8, movementResults.Count);
            string log = @"
Jeff is moving from <8, 0, 8> to <8, 0, 7>
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 56, (dice roll: 0)
0 XP added to character Fred, for a total of 0 XP
Jeff is moving from <8, 0, 7> to <8, 0, 6>
Jeff is moving from <8, 0, 6> to <8, 0, 5>
Jeff is moving from <8, 0, 5> to <8, 0, 4>
Jeff is moving from <8, 0, 4> to <7, 0, 3>
Jeff is moving from <7, 0, 3> to <7, 0, 2>
Jeff is moving from <7, 0, 2> to <6, 0, 1>
Jeff is moving from <6, 0, 1> to <6, 0, 0>
";
            Assert.AreEqual(log, ActionResultLog.LogString(movementResults));
        }
    }
}
