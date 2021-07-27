using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Items;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 5;
            jeff.ArmorPointsCurrent = 5;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(5, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor prevented 5 damage to character Jeff
0 damage dealt to character Jeff, HP is now 5
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndArmorBlocksAnyDamageTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 5;
            jeff.ArmorPointsCurrent = 5;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 20, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(3, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 20)
Critical chance: 70, (dice roll: 0)
Armor prevented 3 damage to character Jeff
0 damage dealt to character Jeff, HP is now 5
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndArmorSavesAKillTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 4;
            jeff.ArmorPointsCurrent = 2;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(1, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(2, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor prevented 2 damage to character Jeff
3 damage dealt to character Jeff, HP is now 1
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndArmorShreddingAllowsAKillTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.ShredderAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 3;
            jeff.ArmorPointsCurrent = 2;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-2, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.TargetCharacter.ArmorPointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(2, result.ArmorShredded);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
2 armor points shredded
5 damage dealt to character Jeff, HP is now -2
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHighArmorShreddingTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.ShredderAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 5;
            jeff.ArmorPointsCurrent = 3;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(1, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(1, result.TargetCharacter.ArmorPointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(1, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
2 armor points shredded
Armor prevented 1 damage to character Jeff
4 damage dealt to character Jeff, HP is now 1
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndSuperHighArmorTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 3;
            jeff.ArmorPointsCurrent = 10;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(3, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.TargetCharacter.ArmorPointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(5, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor prevented 5 damage to character Jeff
0 damage dealt to character Jeff, HP is now 3
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndSuperHighArmorAndArmorPiercingTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.ArmorPiercingAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 3;
            jeff.ArmorPointsCurrent = 10;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-2, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.TargetCharacter.ArmorPointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(0, result.ArmorShredded);
            Assert.AreEqual(0, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor was ignored due to 'armor piercing' ability
5 damage dealt to character Jeff, HP is now -2
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

    }
}