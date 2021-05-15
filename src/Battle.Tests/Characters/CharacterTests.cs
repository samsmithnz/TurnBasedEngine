using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterTests
    {
        [TestMethod]
        public void CharacterFredTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();

            //Act            

            //Assert
            TestFred(fred);
        }

        [TestMethod]
        public void CharacterHarryTest()
        {
            //Arrange
            Character harry = CharacterPool.CreateHarry();

            //Act            

            //Assert
            TestHarry(harry);
        }

        [TestMethod]
        public void CharacterJeffTest()
        {
            //Arrange
            Character jeff = CharacterPool.CreateJeff();
            jeff.InFullCover = true;

            //Act            

            //Assert
            TestJeff(jeff);
        }

        private static void TestFred(Character character)
        {
            Assert.IsNotNull(character);
            Assert.AreEqual("Fred", character.Name);
            Assert.AreEqual(12, character.Hitpoints);
            Assert.AreEqual(0, character.ArmorPoints);
            Assert.AreEqual(70, character.ChanceToHit);
            Assert.AreEqual(0, character.Experience);
            Assert.AreEqual(1, character.Level);
            Assert.AreEqual(false, character.LevelUpIsReady);
            Assert.AreEqual(10, character.Speed);
            Assert.AreEqual(new Vector3(0,0, 0), character.Location);
            Assert.AreEqual(2, character.ActionPoints);
            Assert.AreEqual(10, character.Range);
            Assert.AreEqual(false, character.InHalfCover);
            Assert.AreEqual(false, character.InFullCover);
            Assert.AreEqual(false, character.InOverwatch);
            Assert.IsNotNull(character.Abilities);
            Assert.AreEqual(1, character.Abilities.Count);
            Assert.AreEqual("Ability", character.Abilities[0].Name);
            Assert.AreEqual(AbilityTypeEnum.Unknown, character.Abilities[0].Type);
            Assert.AreEqual(0, character.Abilities[0].Adjustment);
            Assert.IsNotNull(character.Effects);
            Assert.AreEqual(1, character.Effects.Count);
            Assert.AreEqual("Fire", character.Effects[0].Name);
            Assert.AreEqual(AbilityTypeEnum.FireDamage , character.Effects[0].Type);
            Assert.AreEqual(1, character.Effects[0].Adjustment);
            Assert.AreEqual(2, character.Effects[0].TurnExpiration);
        }

        private static void TestJeff(Character character)
        {
            Assert.IsNotNull(character);
            Assert.AreEqual("Jeff", character.Name);
            Assert.AreEqual(12, character.Hitpoints);
            Assert.AreEqual(0, character.ArmorPoints);
            Assert.AreEqual(70, character.ChanceToHit);
            Assert.AreEqual(0, character.Experience);
            Assert.AreEqual(1, character.Level);
            Assert.AreEqual(false, character.LevelUpIsReady);
            Assert.AreEqual(11, character.Speed);
            Assert.AreEqual(new Vector3(8, 0, 8), character.Location);
            Assert.AreEqual(2, character.ActionPoints);
            Assert.AreEqual(10, character.Range);
            Assert.AreEqual(false, character.InHalfCover);
            Assert.AreEqual(true, character.InFullCover);
            Assert.AreEqual(false, character.InOverwatch);
        }

        private static void TestHarry(Character character)
        {
            Assert.IsNotNull(character);
            Assert.AreEqual("Harry", character.Name);
            Assert.AreEqual(12, character.Hitpoints);
            Assert.AreEqual(1, character.ArmorPoints);
            Assert.AreEqual(70, character.ChanceToHit);
            Assert.AreEqual(0, character.Experience);
            Assert.AreEqual(1, character.Level);
            Assert.AreEqual(false, character.LevelUpIsReady);
            Assert.AreEqual(12, character.Speed);
            Assert.AreEqual(new Vector3(5, 0, 5), character.Location);
            Assert.AreEqual(2, character.ActionPoints);
            Assert.AreEqual(10, character.Range);
            Assert.AreEqual(true, character.InHalfCover);
            Assert.AreEqual(false, character.InFullCover);
            Assert.AreEqual(false, character.InOverwatch);
        }
    }
}
