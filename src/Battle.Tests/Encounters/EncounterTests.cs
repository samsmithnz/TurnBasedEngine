using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Utility;
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
        //Fred hits Jeff with a sword, causing 10 points of damage
        [TestMethod]
        public void FredSwordJeffHitEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon sword = WeaponPool.CreateSword();
            Character jeff = CharacterPool.CreateJeff();
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, sword, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(2, result.TargetCharacter.HP);
        }

        //Fred misses Jeff with a sword, causing zero points of damage
        [TestMethod]
        public void FredSwordJeffMissEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 75;
            Weapon sword = WeaponPool.CreateSword();
            Character jeff = CharacterPool.CreateJeff();
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, sword, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            Assert.AreEqual(12, result.TargetCharacter.HP);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void FredSwordJeffNoRandomNumbersEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 75;
            Weapon sword = WeaponPool.CreateSword();
            Character jeff = CharacterPool.CreateJeff();
            List<int> randomNumbers = null;

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, sword, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result == null);
        }

        //Fred hits Jeff with a sword, causing 10 points of damage, and killing him
        [TestMethod]
        public void FredSwordJeffHitAndKillEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon sword = WeaponPool.CreateSword();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 1;
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, sword, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

        //Fred hits Jeff with a sword, causing 10 points of damage, killing him, and leveling up
        [TestMethod]
        public void FredSwordJeffHitAndKillWithLevelUpEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Experience = 200;
            fred.Level = 2;
            Weapon sword = WeaponPool.CreateSword();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 1;
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, sword, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(300, result.SourceCharacter.Experience);
            Assert.AreEqual(2, result.SourceCharacter.Level);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

    }
}