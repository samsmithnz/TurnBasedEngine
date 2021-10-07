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
        public void FredInOverwatchKillsWhileJethroMoves()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            fred.InOverwatch = true;
            //Weapon rifle = fred.WeaponEquiped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            RandomNumberQueue diceRolls = new RandomNumberQueue(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, jethro.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, jethro, pathFindingResult, diceRolls, new List<Character>() { fred }, null);

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(-8, jethro.HitpointsCurrent);
            Assert.AreEqual(new Vector3(8, 0, 7), jethro.Location);
            Assert.AreEqual(100, fred.XP);
            Assert.AreEqual(1, movementResults.Count);
            string log = @"
Jethro is moving from <8, 0, 8> to <8, 0, 7>
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 56, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jethro, HP is now -8
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));
        }

        [TestMethod]
        public void FredInOverwatchWithOpportunistKillsWhileJethroMoves()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            fred.InOverwatch = true;
            fred.Abilities.Add(AbilityPool.OpportunistAbility());
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            RandomNumberQueue diceRolls = new RandomNumberQueue(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, jethro.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, jethro, pathFindingResult, diceRolls, new List<Character>() { fred }, null);

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(-8, jethro.HitpointsCurrent);
            Assert.AreEqual(new Vector3(8, 0, 7), jethro.Location);
            Assert.AreEqual(100, fred.XP);
            Assert.AreEqual(1, movementResults.Count);
            string log = @"
Jethro is moving from <8, 0, 8> to <8, 0, 7>
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jethro, HP is now -8
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));
        }

        [TestMethod]
        public void FredInOverwatchMissesWhileJethroMoves()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            fred.InOverwatch = true;
            //Weapon rifle = fred.WeaponEquiped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            RandomNumberQueue diceRolls = new RandomNumberQueue(new List<int> { 0, 1, 2, 3, 4, 5, 0, 1 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, jethro.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, jethro, pathFindingResult, diceRolls, new List<Character>() { fred }, null);

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(4, jethro.HitpointsCurrent);
            Assert.AreEqual(destination, jethro.Location);
            Assert.AreEqual(0, fred.XP);
            Assert.AreEqual(8, movementResults.Count);
            string log = @"
Jethro is moving from <8, 0, 8> to <8, 0, 7>
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 56, (dice roll: 0)
0 XP added to character Fred, for a total of 0 XP
Jethro is moving from <8, 0, 7> to <8, 0, 6>
Jethro is moving from <8, 0, 6> to <8, 0, 5>
Jethro is moving from <8, 0, 5> to <8, 0, 4>
Jethro is moving from <8, 0, 4> to <7, 0, 3>
Jethro is moving from <7, 0, 3> to <7, 0, 2>
Jethro is moving from <7, 0, 2> to <6, 0, 1>
Jethro is moving from <6, 0, 1> to <6, 0, 0>
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));
        }

        [TestMethod]
        public void FredAndHarryInOverwatchKillsWhileJethroMoves()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            fred.InOverwatch = true;
            Character harry = CharacterPool.CreateHarryHeroSidekick(map, new Vector3(5, 0, 5));
            harry.InOverwatch = true;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            jethro.HitpointsCurrent = 25;
            RandomNumberQueue diceRolls = new RandomNumberQueue(new List<int> { 100, 100, 100, 100, 100, 100, 0, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, jethro.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, jethro, pathFindingResult, diceRolls, new List<Character>() { fred, harry }, null);

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(0, jethro.HitpointsCurrent);
            Assert.AreEqual(new Vector3(8, 0, 7), jethro.Location);
            Assert.AreEqual(100, fred.XP);
            Assert.AreEqual(10, harry.XP);
            Assert.AreEqual(1, movementResults.Count);
            Assert.AreEqual(2, movementResults[0].OverwatchEncounterResults.Count);
            string log = @"
Jethro is moving from <8, 0, 8> to <8, 0, 7>
Harry is attacking with Sniper Rifle, targeted on Jethro
Hit: Chance to hit: 42, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 9-13, (dice roll: 100)
13 damage dealt to character Jethro, HP is now 12
10 XP added to character Harry, for a total of 10 XP
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 56, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));

        }

        [TestMethod]
        public void FredAndHarryInOverwatchMissesWhileJethroMoves()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new Vector3(6, 0, 0);
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            fred.InOverwatch = true;
            Character harry = CharacterPool.CreateHarryHeroSidekick(map, new Vector3(5, 0, 5));
            harry.InOverwatch = true;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            RandomNumberQueue diceRolls = new RandomNumberQueue(new List<int> { 0, 1, 2, 3, 4, 5, 0, 1 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, jethro.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, jethro, pathFindingResult, diceRolls, new List<Character>() { fred }, null);

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(4, jethro.HitpointsCurrent);
            Assert.AreEqual(destination, jethro.Location);
            Assert.AreEqual(0, fred.XP);
            Assert.AreEqual(0, harry.XP);
            Assert.AreEqual(8, movementResults.Count);
            string log = @"
Jethro is moving from <8, 0, 8> to <8, 0, 7>
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 56, (dice roll: 0)
0 XP added to character Fred, for a total of 0 XP
Jethro is moving from <8, 0, 7> to <8, 0, 6>
Jethro is moving from <8, 0, 6> to <8, 0, 5>
Jethro is moving from <8, 0, 5> to <8, 0, 4>
Jethro is moving from <8, 0, 4> to <7, 0, 3>
Jethro is moving from <7, 0, 3> to <7, 0, 2>
Jethro is moving from <7, 0, 2> to <6, 0, 1>
Jethro is moving from <6, 0, 1> to <6, 0, 0>
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));
        }
    }
}
