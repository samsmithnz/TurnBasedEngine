using TBE.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TBE.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterStatusTests
    {
        [TestMethod]
        public void Character_NewCharacter_HasNormalStatus()
        {
            //Arrange
            Character character = new Character();

            //Act

            //Assert
            Assert.AreEqual(CharacterStatus.Normal, character.Status);
            Assert.AreEqual(0, character.StatusRecoveryTime);
            Assert.IsTrue(character.IsAvailable);
        }

        [TestMethod]
        public void Character_SetInjured_StatusIsInjured()
        {
            //Arrange
            Character character = new Character();
            int recoveryDays = 5;

            //Act
            character.SetInjured(recoveryDays);

            //Assert
            Assert.AreEqual(CharacterStatus.Injured, character.Status);
            Assert.AreEqual(5, character.StatusRecoveryTime);
            Assert.IsFalse(character.IsAvailable);
        }

        [TestMethod]
        public void Character_SetUnavailable_StatusIsUnavailable()
        {
            //Arrange
            Character character = new Character();
            int recoveryDays = 3;

            //Act
            character.SetUnavailable(recoveryDays);

            //Assert
            Assert.AreEqual(CharacterStatus.Unavailable, character.Status);
            Assert.AreEqual(3, character.StatusRecoveryTime);
            Assert.IsFalse(character.IsAvailable);
        }

        [TestMethod]
        public void Character_ProcessDayOfRecovery_DecrementsRecoveryTime()
        {
            //Arrange
            Character character = new Character();
            character.SetInjured(3);

            //Act
            bool recovered = character.ProcessDayOfRecovery();

            //Assert
            Assert.IsFalse(recovered);
            Assert.AreEqual(CharacterStatus.Injured, character.Status);
            Assert.AreEqual(2, character.StatusRecoveryTime);
            Assert.IsFalse(character.IsAvailable);
        }

        [TestMethod]
        public void Character_ProcessDayOfRecovery_RecoversToNormalWhenTimeReachesZero()
        {
            //Arrange
            Character character = new Character();
            character.SetInjured(1);

            //Act
            bool recovered = character.ProcessDayOfRecovery();

            //Assert
            Assert.IsTrue(recovered);
            Assert.AreEqual(CharacterStatus.Normal, character.Status);
            Assert.AreEqual(0, character.StatusRecoveryTime);
            Assert.IsTrue(character.IsAvailable);
        }

        [TestMethod]
        public void Character_ProcessDayOfRecovery_NormalCharacterReturnsFalse()
        {
            //Arrange
            Character character = new Character();

            //Act
            bool recovered = character.ProcessDayOfRecovery();

            //Assert
            Assert.IsFalse(recovered);
            Assert.AreEqual(CharacterStatus.Normal, character.Status);
        }

        [TestMethod]
        public void Character_ProcessMultipleDaysOfRecovery_RecoversCorrectly()
        {
            //Arrange
            Character character = new Character();
            character.SetUnavailable(3);

            //Act & Assert - Day 1
            bool recovered = character.ProcessDayOfRecovery();
            Assert.IsFalse(recovered);
            Assert.AreEqual(2, character.StatusRecoveryTime);

            //Act & Assert - Day 2
            recovered = character.ProcessDayOfRecovery();
            Assert.IsFalse(recovered);
            Assert.AreEqual(1, character.StatusRecoveryTime);

            //Act & Assert - Day 3
            recovered = character.ProcessDayOfRecovery();
            Assert.IsTrue(recovered);
            Assert.AreEqual(CharacterStatus.Normal, character.Status);
            Assert.AreEqual(0, character.StatusRecoveryTime);
            Assert.IsTrue(character.IsAvailable);
        }

        [TestMethod]
        public void Character_SetInjuredThenRecover_CanBeInjuredAgain()
        {
            //Arrange
            Character character = new Character();
            character.SetInjured(1);
            character.ProcessDayOfRecovery();
            Assert.AreEqual(CharacterStatus.Normal, character.Status);

            //Act
            character.SetInjured(2);

            //Assert
            Assert.AreEqual(CharacterStatus.Injured, character.Status);
            Assert.AreEqual(2, character.StatusRecoveryTime);
            Assert.IsFalse(character.IsAvailable);
        }

        [TestMethod]
        public void Character_FromPool_HasNormalStatus()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));

            //Act

            //Assert
            Assert.AreEqual(CharacterStatus.Normal, fred.Status);
            Assert.AreEqual(0, fred.StatusRecoveryTime);
            Assert.IsTrue(fred.IsAvailable);
        }
    }
}
