using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.FieldOfView;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Battle.Tests.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Tests.Encounters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class AreaAttackTests
    {

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
            List<Character> characterList = new() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.WeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HP);
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
            List<Character> characterList = new() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.WeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, jeff.HP);
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
            List<Character> characterList = new() { fred, jeff, harry };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.WeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HP);
            Assert.AreEqual(0, harry.HP);
            Assert.AreEqual(200, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndInjuriesJeffAndKillsHarryTest()
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
            jeff.HP = 15;
            Character harry = CharacterPool.CreateHarry();
            harry.Location = new Vector3(3, 0, 3);
            harry.HP = 4;
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jeff, harry };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.WeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(12, fred.HP);
            Assert.AreEqual(0, harry.HP);
            Assert.AreEqual(110, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FredThrowsGrenadeWithCriticalAbilityAndKillsJeffTest()
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
            fred.Abilities.Add(new("Biggest Booms", AbilityTypeEnum.CriticalDamage, 2));
            fred.Abilities.Add(new("Biggest Booms", AbilityTypeEnum.CriticalChance, 20));
            fred.WeaponEquipped = WeaponPool.CreateGrenade();
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(1, 0, 3);
            jeff.HP = 6;
            Character harry = CharacterPool.CreateHarry();
            harry.Location = new Vector3(3, 0, 3);
            harry.HP = 4;
            List<int> diceRolls = new() { 65, 100, 80 }; //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jeff, harry };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.WeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.AllCharacters != null);
            Assert.AreEqual(6, result.DamageDealt);
            //Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HP);
            Assert.AreEqual(-2, harry.HP);
            Assert.IsTrue(result.TargetCharacter != null);
            Assert.IsTrue(result.AllCharacters.Count > 0);
            Assert.AreEqual(200, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
        }

        [TestMethod]
        public void FOVFredThrowsGrenadeAndDestoriesCoverUpdatingFOVTest()
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
            string[,] map = MapUtility.InitializeMap(5, 5);
            map[2, 3] = "W"; //Add cover 
            Character fred = CharacterPool.CreateFred();
            fred.WeaponEquipped = WeaponPool.CreateGrenade();
            fred.Location = new Vector3(2, 0, 0);
            Character jeff = CharacterPool.CreateJeff();
            jeff.Location = new Vector3(2, 0, 4);
            jeff.HP = 5;
            List<int> diceRolls = new() { 65, 100, 0 }; //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jeff };

            //Act 1: get the FOV
            List<Vector3> results = FieldOfViewCalculator.GetFieldOfView(map, fred.Location, 10);
            
            //Assert: check the initial FOV
            Assert.AreEqual(22, results.Count);
            bool foundItem = false;
            foreach (Vector3 item in results)
            {
                if (item.X == 2 && item.Z == 3)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == false);
            foundItem = false;
            foreach (Vector3 item in results)
            {
                if (item.X == 2 && item.Z == 4)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == false);
            //Assert.AreEqual(new Vector3(1, 0, 3), results[0]);

            //Act 2: Now destroy the cover
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.WeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);
            results = FieldOfViewCalculator.GetFieldOfView(map, fred.Location, 10);
            
            //Assert 2: Check the FOV now
            Assert.AreEqual(24, results.Count);
            foundItem = false;
            foreach (Vector3 item in results)
            {
                if (item.X == 2 && item.Z == 3)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == true);
            foundItem = false;
            foreach (Vector3 item in results)
            {
                if (item.X == 2 && item.Z == 4)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == true);

            //Assert 3: Check the area attack result 
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, jeff.HP);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void NoDiceRollsEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.WeaponEquipped = WeaponPool.CreateGrenade();
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = MapUtility.InitializeMap(10, 10);
            List<int> diceRolls = null;
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.WeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result == null);
        }

    }
}