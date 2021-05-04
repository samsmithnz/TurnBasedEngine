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
        //Fred hits Jeff with a rifle, causing 10 points of damage
        [TestMethod]
        public void FredrifleJeffHitEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(2, result.TargetCharacter.HP);
        }

        //Fred misses Jeff with a rifle, causing zero points of damage
        [TestMethod]
        public void FredrifleJeffMissEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 75;
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = false;
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.SourceCharacter.Experience);
            Assert.AreEqual(12, result.TargetCharacter.HP);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void FredrifleJeffNoRandomNumbersEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 75;
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            List<int> randomNumbers = null;

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result == null);
        }

        //Fred hits Jeff with a rifle, causing 10 points of damage, and killing him
        [TestMethod]
        public void FredrifleJeffHitAndKillEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 1;
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

        //Fred hits Jeff with a rifle, causing 10 points of damage, killing him, and leveling up
        [TestMethod]
        public void FredrifleJeffHitAndKillWithLevelUpEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.Experience = 200;
            fred.Level = 2;
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.HP = 1;
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(300, result.SourceCharacter.Experience);
            Assert.AreEqual(2, result.SourceCharacter.Level);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            Assert.AreEqual(0, result.TargetCharacter.HP);
        }

        [TestMethod]
        public void FredShootsAtJeffInHalfCoverTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon rifle = WeaponPool.CreateRifle();
            Character jeff = CharacterPool.CreateJeff();
            jeff.InHalfCover = true;
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(2, result.TargetCharacter.HP);
        }

        //[TestMethod]
        //public void FredShootsAtJeffInFullCoverTest()
        //{
        //    //Arrange
        //    Character fred = CharacterPool.CreateFred();
        //    Weapon rifle = WeaponPool.CreateRifle();
        //    Character jeff = CharacterPool.CreateJeff();
        //    jeff.InFullCover = true;
        //    List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

        //    //Act
        //    EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, randomNumbers);

        //    //Assert
        //    Assert.IsTrue(result != null);
        //    Assert.AreEqual(0, result.SourceCharacter.Experience);
        //    Assert.AreEqual(12, result.TargetCharacter.HP);
        //}

    }
}