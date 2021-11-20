using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Items;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void FredAttacksJethroWithRifleAndArmorBlocksKillTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 5;
            jethro.ArmorPointsCurrent = 5;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(5, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor prevented 5 damage to character Jethro
0 damage dealt to character Jethro, HP is now 5
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndArmorBlocksAnyDamageTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 5;
            jethro.ArmorPointsCurrent = 5;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 20, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(3, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 20)
Critical chance: 70, (dice roll: 0)
Armor prevented 3 damage to character Jethro
0 damage dealt to character Jethro, HP is now 5
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndArmorSavesAKillTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 4;
            jethro.ArmorPointsCurrent = 2;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(1, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(2, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor prevented 2 damage to character Jethro
3 damage dealt to character Jethro, HP is now 1
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndArmorShreddingAllowsAKillTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.Abilities.Add(AbilityPool.ShredderAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 3;
            jethro.ArmorPointsCurrent = 2;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-2, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.TargetCharacter.ArmorPointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(2, result.ArmorShredded);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
2 armor points shredded
5 damage dealt to character Jethro, HP is now -2
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndHighArmorShreddingTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.Abilities.Add(AbilityPool.ShredderAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 5;
            jethro.ArmorPointsCurrent = 3;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(1, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(1, result.TargetCharacter.ArmorPointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(1, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
2 armor points shredded
Armor prevented 1 damage to character Jethro
4 damage dealt to character Jethro, HP is now 1
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndSuperHighArmorTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 3;
            jethro.ArmorPointsCurrent = 10;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(3, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.TargetCharacter.ArmorPointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(5, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor prevented 5 damage to character Jethro
0 damage dealt to character Jethro, HP is now 3
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndSuperHighArmorAndArmorPiercingTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.Abilities.Add(AbilityPool.ArmorPiercingAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 3;
            jethro.ArmorPointsCurrent = 10;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-2, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.TargetCharacter.ArmorPointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(0, result.ArmorShredded);
            Assert.AreEqual(0, result.ArmorAbsorbed);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
Armor was ignored due to 'armor piercing' ability
5 damage dealt to character Jethro, HP is now -2
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

    }
}