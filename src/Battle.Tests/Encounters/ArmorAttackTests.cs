﻿using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Weapons;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Encounters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class ArmorAttackTests
    {

        [TestMethod]
        public void FredAttacksJeffWithRifleAndArmorBlocksKillTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.Hitpoints = 5;
            jeff.ArmorPoints = 5;
            string[,] map = MapUtility.InitializeMap(10, 10);
            Queue<int> diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.TargetCharacter.Hitpoints);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 20, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor prevented 5 damage to character Jeff
0 damage dealt to character Jeff, character HP is now 5
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndArmorSavesAKillTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.Hitpoints = 4;
            jeff.ArmorPoints = 2;
            string[,] map = MapUtility.InitializeMap(10, 10);
            Queue<int> diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(1, result.TargetCharacter.Hitpoints);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 20, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor prevented 2 damage to character Jeff
3 damage dealt to character Jeff, character HP is now 1
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndArmorShreddingAllowsAKillTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Shredder", AbilityTypeEnum.ArmorShredding, 2));
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.Hitpoints = 3;
            jeff.ArmorPoints = 2;
            string[,] map = MapUtility.InitializeMap(10, 10);
            Queue<int> diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.TargetCharacter.Hitpoints);
            Assert.AreEqual(0, result.TargetCharacter.ArmorPoints);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 20, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
2 armor points shredded
3 damage dealt to character Jeff, character HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHighArmorShreddingTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Shredder", AbilityTypeEnum.ArmorShredding, 2));
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.Hitpoints = 3;
            jeff.ArmorPoints = 3;
            string[,] map = MapUtility.InitializeMap(10, 10);
            Queue<int> diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(1, result.TargetCharacter.Hitpoints);
            Assert.AreEqual(1, result.TargetCharacter.ArmorPoints);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 20, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
2 armor points shredded
Armor prevented 1 damage to character Jeff
2 damage dealt to character Jeff, character HP is now 1
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

    }
}