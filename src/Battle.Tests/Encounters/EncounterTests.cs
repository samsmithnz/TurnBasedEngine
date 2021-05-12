using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Weapons;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Battle.Tests.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Encounters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class EncounterTests
    {
        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 80, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            int chanceToHit = Encounter.GetChanceToHit(fred, rifle, jeff);
            int chanceToCrit = Encounter.GetChanceToCrit(fred, rifle, jeff, map);
            DamageOptions damageOptions = Encounter.GetDamageRange(fred, rifle);
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.IsNotNull(damageOptions);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);
            Assert.AreEqual(8, damageOptions.CriticalDamageLow);
            Assert.AreEqual(12, damageOptions.CriticalDamageHigh);
            Assert.AreEqual(7, result.TargetCharacter.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndMissesTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = false;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 44 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.TargetCharacter.HP);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleModifiersAndHitsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquipped;
            rifle.ChanceToHitAdjustment = 20;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = false;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void NoDiceRollsEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = null;

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 5;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 20 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalHitTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 12;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 30 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCritsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 5;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        //Fred hits Jeff with a rifle, causing 10 points of damage, killing him, and leveling up
        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleCausingFredToLevelUpTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Experience = 0;
            fred.Level = 1;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 5;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 30 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(1, result.SourceCharacter.Level);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInHalfCoverAndHitsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 85;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InHalfCover = true;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverAndMissesTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 55 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.TargetCharacter.HP);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverDiagAndMissesWithNegativeChanceToHitTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 30;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.TargetCharacter.HP);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
        }



        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverStraightOnAndMissesWithNegativeChanceToHitTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 30;
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            jeff.Location = new System.Numerics.Vector3(5, 0, 0);
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.TargetCharacter.HP);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsWithPlus10DamageAbilityTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Sharp Shooter", AbilityTypeEnum.Damage, 10));
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 15;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsWithPlus5DamageTwiceAbilityTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Sharp Shooter", AbilityTypeEnum.Damage, 3));
            fred.Abilities.Add(new("Sharp Shooter2", AbilityTypeEnum.Damage, 7));
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 15;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalChanceAbilityBonusTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Platform Stability", AbilityTypeEnum.CriticalChance, 10));
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 12;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 30 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }



        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalDamageAbilityBonusTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Bring em on", AbilityTypeEnum.CriticalDamage, 3));
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 15;
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 30 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(15, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredAttacksWithRifleJeffBehindCoverAndInjuriesHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "■" = cover
            //  "□" = open ground
            //  □ □ E □ □
            //  □ □ ■ □ □ 
            //  □ □ □ □ □ 
            //  □ □ □ □ □
            //  □ □ P □ □
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[2, 3] = "W"; //Add cover 
            Character fred = CharacterPool.CreateFred();
            fred.Location = new Vector3(2, 0, 0);
            fred.Abilities.Add(new("Bring em on", AbilityTypeEnum.CriticalDamage, 3));
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(2, 0, 4);
            jeff.HP = 15;
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(10, result.TargetCharacter.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredAttacksWithRifleJeffWhoIsFlankedAndKillsHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "■" = cover
            //  "□" = open ground
            //  □ □ □ □ □
            //  □ E ■ □ □ 
            //  □ □ □ □ □ 
            //  □ □ □ □ □
            //  □ □ P □ □
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[2, 3] = "W"; //Add cover 
            Character fred = CharacterPool.CreateFred();
            fred.Location = new Vector3(2, 0, 0);
            fred.Abilities.Add(new("Bring em on", AbilityTypeEnum.CriticalDamage, 3));
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(1, 0, 3);
            jeff.HP = 15;
            List<int> diceRolls = new() { 65, 100, 70 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(15, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredAttacksPlayerWhoIsOffMapTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "■" = cover
            //  "□" = open ground
            //            E
            //  □ □ □ □ □
            //  □ □ ■ □ □ 
            //  □ □ □ □ □ 
            //  □ □ □ □ □
            //  □ □ P □ □
            string[,] map = MapUtility.InitializeMap(5, 5);
            map[2, 3] = "W"; //Add cover 
            Character fred = CharacterPool.CreateFred();
            fred.Location = new Vector3(2, 0, 0);
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(5, 0, 5);
            List<int> diceRolls = new() { 65, 65, 0 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            try
            {
                EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls, null);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual("The character is off the map", ex.Message);
            }
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJeffTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "■" = cover
            //  "□" = open ground
            //  □ □ □ □ □
            //  □ E ■ □ □ 
            //  □ □ □ □ □ 
            //  □ □ □ □ □
            //  □ □ P □ □
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[2, 3] = "W"; //Add cover 
            Character fred = CharacterPool.CreateFred();
            fred.WeaponEquipped = WeaponPool.CreateGrenade();
            fred.Location = new Vector3(2, 0, 0);
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(1, 0, 3);
            jeff.HP = 4;
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, fred.WeaponEquipped, jeff, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndInjuriesJeffTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "■" = cover
            //  "□" = open ground
            //  □ □ □ □ □
            //  □ E ■ □ □ 
            //  □ □ □ □ □ 
            //  □ □ □ □ □
            //  □ □ P □ □
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[2, 3] = "W"; //Add cover 
            Character fred = CharacterPool.CreateFred();
            fred.WeaponEquipped = WeaponPool.CreateGrenade();
            fred.Location = new Vector3(2, 0, 0);
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(1, 0, 3);
            jeff.HP = 5;
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, fred.WeaponEquipped, jeff, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, result.TargetCharacter.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJeffAndHarryTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "H" = enemy/harry
            //  "■" = cover
            //  "□" = open ground
            //  □ □ □ □ □
            //  □ E ■ H □ 
            //  □ □ □ □ □ 
            //  □ □ □ □ □
            //  □ □ P □ □
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[2, 3] = "W"; //Add cover 
            Character fred = CharacterPool.CreateFred();
            fred.WeaponEquipped = WeaponPool.CreateGrenade();
            fred.Location = new Vector3(2, 0, 0);
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(1, 0, 3);
            jeff.HP = 4;
            Character harry = CharacterPool.CreateHarry();
            harry.Location = new Vector3(3, 0, 3);
            harry.HP = 4;
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, fred.WeaponEquipped, jeff, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HP);
            Assert.AreEqual(0, harry.HP);
            Assert.AreEqual(200, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        //[TestMethod]
        //public void FredThrowsGrenadeAndInjuriesJeffAndKillsHarryTest()
        //{
        //    //Arrange
        //    //  "P" = player/fred
        //    //  "E" = enemy/jeff
        //    //  "H" = enemy/harry
        //    //  "■" = cover
        //    //  "□" = open ground
        //    //  □ □ □ □ □
        //    //  □ E ■ H □ 
        //    //  □ □ □ □ □ 
        //    //  □ □ □ □ □
        //    //  □ □ P □ □
        //    string[,] map = MapUtility.InitializeMap(10, 10);
        //    map[2, 3] = "W"; //Add cover 
        //    Character fred = CharacterPool.CreateFred();
        //    fred.WeaponEquipped = WeaponPool.CreateGrenade();
        //    fred.Location = new Vector3(2, 0, 0);
        //    Character jeff = CharacterPool.CreateJeff();
        //    jeff.Location = new Vector3(1, 0, 3);
        //    jeff.HP = 15;
        //    List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll
        //    Vector3 targetThrowingLocation = new(2, 0, 4);

        //    //Act
        //    EncounterResult result = Encounter.AttackCharacter(fred, fred.WeaponEquipped, jeff, map, diceRolls, targetThrowingLocation);

        //    //Assert
        //    Assert.IsTrue(result != null);
        //    Assert.AreEqual(15, result.DamageDealt);
        //    Assert.AreEqual(true, result.IsCriticalHit);
        //    Assert.AreEqual(0, result.TargetCharacter.HP);
        //    Assert.AreEqual(100, result.SourceCharacter.Experience);
        //    Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        //}

        //[TestMethod]
        //public void FredThrowsGrenadeWithCriticalAbilityAndKillsJeffTest()
        //{
        //    //Arrange
        //    //  "P" = player/fred
        //    //  "E" = enemy/jeff
        //    //  "■" = cover
        //    //  "□" = open ground
        //    //  □ □ □ □ □
        //    //  □ E ■ □ □ 
        //    //  □ □ □ □ □ 
        //    //  □ □ □ □ □
        //    //  □ □ P □ □
        //    string[,] map = MapUtility.InitializeMap(10, 10);
        //    map[2, 3] = "W"; //Add cover 
        //    Character fred = CharacterPool.CreateFred();
        //    fred.Location = new Vector3(2, 0, 0);
        //    fred.Abilities.Add(new("Biggest Booms", AbilityTypeEnum.CriticalDamage, 2));
        //    fred.Abilities.Add(new("Biggest Booms", AbilityTypeEnum.CriticalChance, 20));
        //    Weapon rifle = fred.WeaponEquipped;
        //    Character jeff = CharacterPool.CreateJeff();
        //    jeff.Location = new Vector3(1, 0, 3);
        //    jeff.HP = 15;
        //    List<int> diceRolls = new() { 65, 100, 70 }; //Chance to hit roll, damage roll, critical chance roll
        //    Vector3 targetThrowingLocation = new(2, 0, 4);

        //    //Act
        //    EncounterResult result = Encounter.AttackCharacter(fred, fred.WeaponEquipped, jeff, map, diceRolls, targetThrowingLocation);

        //    //Assert
        //    Assert.IsTrue(result != null);
        //    Assert.AreEqual(15, result.DamageDealt);
        //    Assert.AreEqual(true, result.IsCriticalHit);
        //    Assert.AreEqual(0, result.TargetCharacter.HP);
        //    Assert.AreEqual(100, result.SourceCharacter.Experience);
        //    Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        //}

    }
}