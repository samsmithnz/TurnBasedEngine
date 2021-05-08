using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Weapons;
using Battle.Tests.Characters;
using Battle.Tests.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Encounters
{
    [TestClass]
    [TestCategory("L0")]
    public class EncounterTests
    {
        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndMissesTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = false;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

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
            Weapon rifle = fred.WeaponEquiped;
            rifle.ChanceToHitAdjustment = 20;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = false;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = null;

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 5;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 12;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 0 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 12;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 0 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 5;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.TargetCharacter.HP);
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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InHalfCover = true;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverAndHitsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            jeff.Location = new System.Numerics.Vector3(5, 0, 0);
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 15;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsWithPlus5DamageTwiceAbilityTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Sharp Shooter", AbilityTypeEnum.Damage, 3));
            fred.Abilities.Add(new("Sharp Shooter2", AbilityTypeEnum.Damage, 7));
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 15;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalChanceAbilityBonusTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Platform Stability", AbilityTypeEnum.CriticalChance, 10));
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 12;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 30 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

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
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 15;
            string[,] map = GenerateMap(10, 10);
            List<int> diceRolls = new() { 65, 100, 0 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(    15, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void BFredAttacksWithRifleJeffBehindCoverAndKillsHimTest()
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
            string[,] map = GenerateMap(10, 10);
            map[2, 3] = "W"; //Add cover 
            Character fred = CharacterPool.CreateFred();
            fred.Location = new Vector3(2, 0, 0);
            fred.Abilities.Add(new("Bring em on", AbilityTypeEnum.CriticalDamage, 3));
            Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(2, 0, 4);
            jeff.HP = 15;
            List<int> diceRolls = new() { 65, 100, 100 }; //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(10, result.TargetCharacter.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
        }


        private static string[,] GenerateMap(int xMax, int zMax)
        {
            string[,] map = new string[xMax, zMax];

            //Initialize the map
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    map[x, z] = "";
                }
            }

            return map;
        }

    }
}