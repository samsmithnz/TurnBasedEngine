using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Items;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Encounters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class RegularAttackTests
    {
        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 12;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            int chanceToHit = EncounterCore.GetChanceToHit(fred, rifle, jeff);
            int chanceToCrit = EncounterCore.GetChanceToCrit(fred, rifle, jeff, map, false);
            DamageOptions damageOptions = EncounterCore.GetDamageRange(fred, rifle);
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.IsNotNull(damageOptions);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);
            Assert.AreEqual(8, damageOptions.CriticalDamageLow);
            Assert.AreEqual(12, damageOptions.CriticalDamageHigh);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndMissesTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.InFullCover = false;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 44 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 55, (dice roll: 44)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleModifiersAndHitsTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquipped;
            rifle.ChanceToHitAdjustment = 20;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.InFullCover = false;
            jeff.HitpointsCurrent = 12;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 65, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void NoDiceRollsEncounterTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Queue<int> diceRolls = null;

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 5;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 20 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 20)
5 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalHitTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 12;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 30 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 30)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCritsTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 5;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        //Fred hits Jeff with a rifle, causing 10 points of damage, killing him, and leveling up
        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleCausingFredToLevelUpTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Experience = 0;
            fred.Level = 1;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 5;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 30 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(1, result.SourceCharacter.Level);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 30)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now -7
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInHalfCoverAndHitsTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.ChanceToHit = 85;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.InHalfCover = true;
            jeff.HitpointsCurrent = 12;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 75, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverAndMissesTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.InFullCover = true;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 55 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 40, (dice roll: 55)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverDiagAndMissesWithNegativeChanceToHitTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.ChanceToHit = 30;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.InFullCover = true;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 0, (dice roll: 65)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }



        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverStraightOnAndMissesWithNegativeChanceToHitTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.ChanceToHit = 30;
            fred.Experience = 50;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.InFullCover = true;
            jeff.SetLocation(new Vector3(5, 0, 0), map);
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(50, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 19, (dice roll: 65)
0 XP added to character Fred, for a total of 50 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsWithPlus10DamageAbilityTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.SharpShooterAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 15;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-15, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-22, (dice roll: 100)
22 damage dealt to character Jeff, HP is now -7
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsWithPlus5DamageTwiceAbilityTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.SharpShooterAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 15;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-15, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-22, (dice roll: 100)
22 damage dealt to character Jeff, HP is now -7
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalChanceAbilityBonusTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.PlatformStabilityAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 12;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 30 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 80, (dice roll: 30)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }



        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalDamageAbilityBonusTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.BringEmOnAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 15;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 30 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(15, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 30)
Critical damage range: 11-15, (dice roll: 100)
15 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksWithRifleJeffBehindCoverAndInjuriesHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . E . .
            //  . . ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            fred.Abilities.Add(AbilityPool.BringEmOnAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(2, 0, 4), map);
            jeff.HitpointsCurrent = 15;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(10, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 100, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 20, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 10
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksWithRifleJeffWhoIsFlankedAndKillsHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            fred.Abilities.Add(AbilityPool.BringEmOnAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map);
            jeff.HitpointsCurrent = 15;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 70 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(15, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 100, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 70)
Critical damage range: 11-15, (dice roll: 100)
15 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksPlayerWhoIsOffMapTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //            E
            //  . . . . .
            //  . . ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(5, 0, 5), map);
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 65, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            try
            {
                EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual("The character is off the map", ex.Message);
            }
        }

        [TestMethod]
        public void FredAttacksWithRifleJeffBehindFullCoverAndHunkeredDownMissingHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . E . .
            //  . . ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(2, 0, 4), map);
            jeff.HitpointsCurrent = 15;
            jeff.InFullCover = true;
            jeff.HunkeredDown = true;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(15, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 24, (dice roll: 65)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksWithRifleJeffBehindHalfCoverAndHunkeredDownMissingHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . E . .
            //  . . □ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.HalfCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(2, 0, 4), map);
            jeff.HitpointsCurrent = 15;
            jeff.InFullCover = true;
            jeff.HunkeredDown = true;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(15, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Missed: Chance to hit: 24, (dice roll: 65)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksWithRifleJeffBehindFullCoverAndHunkeredDownInjuriesHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . E . .
            //  . . ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(2, 0, 4), map);
            jeff.HitpointsCurrent = 15;
            jeff.InHalfCover = true;
            jeff.HunkeredDown = true;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(10, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 64, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, hunkered down
5 damage dealt to character Jeff, HP is now 10
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }


        [TestMethod]
        public void FredAttacksJeffWithRifleWithNoAmmoTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            rifle.AmmoCurrent = 0;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            //Assert.AreEqual(7, result.TargetCharacter.Hitpoints);
            //Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Rifle has no ammo remaining and the attack cannot be completed
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWithNoAmmoAndReloadsFirstTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Weapon rifle = fred.WeaponEquipped;
            rifle.AmmoCurrent = 0;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.HitpointsCurrent = 12;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            fred.WeaponEquipped.Reload();
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(3, result.SourceCharacter.WeaponEquipped.AmmoCurrent);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

    }
}