using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Weapons;
using Battle.Tests.Characters;
using Battle.Tests.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(7, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndMissesTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 45;
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = false;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            Assert.AreEqual(12, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleModifiersAndHitsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 45;
            Weapon rifle = WeaponPool.CreateRifle();
            rifle.ChanceToHitAdjustment = 20;
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = false;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(7, result.TargetCharacter.HP);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void NoRandomNumbersEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = null;

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 5;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalHitTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 7;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 0 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(7, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCritsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 7;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 0 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(7, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

        //Fred hits Jeff with a rifle, causing 10 points of damage, killing him, and leveling up
        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleCausingFredToLevelUpTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Experience = 0;
            fred.Level = 1;
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 1;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(1, result.SourceCharacter.Level);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInHalfCoverAndHitsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 85;
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.InHalfCover = true;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(7, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverAndHitsTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            Assert.AreEqual(12, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverDiagAndMissesWithNegativeChanceToHitTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 30;
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            Assert.AreEqual(12, result.TargetCharacter.HP);
        }



        [TestMethod]
        public void FredAttacksJeffWithRifleWhoIsInFullCoverStraightOnAndMissesWithNegativeChanceToHitTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 30;
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;
            jeff.Location = new System.Numerics.Vector3(5, 0, 0);
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            Assert.AreEqual(12, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsWithPlus10DamageAbilityTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Sharp Shooter", AbilityTypeEnum.Damage, 10));
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsWithPlus5DamageTwiceAbilityTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Sharp Shooter", AbilityTypeEnum.Damage, 3));
            fred.Abilities.Add(new("Sharp Shooter2", AbilityTypeEnum.Damage, 7));
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 100 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }



        [TestMethod]
        public void FredAttacksAndKillsJeffWithRifleAndCriticalAbilityBonusTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Abilities.Add(new("Bring em on", AbilityTypeEnum.CriticalDamage, 3));
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 10;
            string[,] map = GenerateMap(5, 5);
            List<int> randomNumbers = new() { 65, 100, 0 };

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(10, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HP);
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