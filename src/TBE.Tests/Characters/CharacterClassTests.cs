using TBE.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TBE.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterClassTests
    {
        [TestMethod]
        public void Character_NewCharacter_HasDefaultRecruitClass()
        {
            //Arrange
            Character character = new Character();

            //Act

            //Assert
            Assert.AreEqual(CharacterClass.Recruit, character.CharacterClass);
        }

        [TestMethod]
        public void Character_SetClassToSniper_ClassIsSniper()
        {
            //Arrange
            Character character = new Character();

            //Act
            character.CharacterClass = CharacterClass.Sniper;

            //Assert
            Assert.AreEqual(CharacterClass.Sniper, character.CharacterClass);
        }

        [TestMethod]
        public void Character_SetClassToAssault_ClassIsAssault()
        {
            //Arrange
            Character character = new Character();

            //Act
            character.CharacterClass = CharacterClass.Assault;

            //Assert
            Assert.AreEqual(CharacterClass.Assault, character.CharacterClass);
        }

        [TestMethod]
        public void Character_SetClassToSupport_ClassIsSupport()
        {
            //Arrange
            Character character = new Character();

            //Act
            character.CharacterClass = CharacterClass.Support;

            //Assert
            Assert.AreEqual(CharacterClass.Support, character.CharacterClass);
        }

        [TestMethod]
        public void Character_SetClassToHeavy_ClassIsHeavy()
        {
            //Arrange
            Character character = new Character();

            //Act
            character.CharacterClass = CharacterClass.Heavy;

            //Assert
            Assert.AreEqual(CharacterClass.Heavy, character.CharacterClass);
        }

        [TestMethod]
        public void Character_FredFromPool_IsSupportClass()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));

            //Act

            //Assert
            Assert.AreEqual(CharacterClass.Support, fred.CharacterClass);
        }

        [TestMethod]
        public void Character_HarryFromPool_IsSniperClass()
        {
            //Arrange
            Character harry = CharacterPool.CreateHarryHero(null, new(0, 0, 0));

            //Act

            //Assert
            Assert.AreEqual(CharacterClass.Sniper, harry.CharacterClass);
        }

        [TestMethod]
        public void Character_JeffFromPool_IsAssaultClass()
        {
            //Arrange
            Character jeff = CharacterPool.CreateJeffHero(null, new(0, 0, 0));

            //Act

            //Assert
            Assert.AreEqual(CharacterClass.Assault, jeff.CharacterClass);
        }

        [TestMethod]
        public void Character_JethroFromPool_IsAssaultClass()
        {
            //Arrange
            Character jethro = CharacterPool.CreateJethroBaddie(null, new(0, 0, 0));

            //Act

            //Assert
            Assert.AreEqual(CharacterClass.Assault, jethro.CharacterClass);
        }

        [TestMethod]
        public void Character_BartFromPool_IsHeavyClass()
        {
            //Arrange
            Character bart = CharacterPool.CreateBartBaddie(null, new(0, 0, 0));

            //Act

            //Assert
            Assert.AreEqual(CharacterClass.Heavy, bart.CharacterClass);
        }

        [TestMethod]
        public void Character_DerekFromPool_IsHeavyClass()
        {
            //Arrange
            Character derek = CharacterPool.CreateDerekBaddie(null, new(0, 0, 0));

            //Act

            //Assert
            Assert.AreEqual(CharacterClass.Heavy, derek.CharacterClass);
        }
    }
}
