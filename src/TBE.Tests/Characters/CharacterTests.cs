using TBE.Logic.AbilitiesAndEffects;
using TBE.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));

            //Act
            List<CharacterAction> actions = fred.GetCurrentActions();

            //Assert
            TestFred(fred, actions);
        }

        [TestMethod]
        public void CharacterHarryTest()
        {
            //Arrange
            Character harry = CharacterPool.CreateHarryHero(null, new(5, 0, 5));

            //Act            

            //Assert
            TestHarry(harry);
        }

        [TestMethod]
        public void CharacterJethroTest()
        {
            //Arrange
            Character jethro = CharacterPool.CreateJethroBaddie(null, new(8, 0, 8));
            jethro.CoverState.InFullCover = true;

            //Act            

            //Assert
            TestJethro(jethro);
        }

        private static void TestFred(Character character, List<CharacterAction> actions)
        {
            Assert.IsNotNull(character);
            Assert.AreEqual("Fred", character.Name);
            Assert.AreEqual("Fred is from Canada and is a nice guy.", character.Background);
            Assert.AreEqual(4, character.HitpointsMax);
            Assert.AreEqual(4, character.HitpointsCurrent);
            Assert.AreEqual(0, character.ArmorPointsMax);
            Assert.AreEqual(0, character.ArmorPointsCurrent);
            Assert.AreEqual(2, character.ActionPointsMax);
            Assert.AreEqual(2, character.ActionPointsCurrent);
            Assert.AreEqual(70, character.ChanceToHit);
            Assert.AreEqual(0, character.XP);
            Assert.AreEqual(1, character.Level);
            Assert.AreEqual(false, character.LevelUpIsReady);
            Assert.AreEqual(10, character.Speed);
            Assert.AreEqual(75, character.Intelligence);
            Assert.AreEqual(new(0, 0, 0), character.Location);
            Assert.AreEqual(8, character.MobilityRange);
            Assert.AreEqual(30, character.ShootingRange);
            Assert.AreEqual(40, character.FOVRange);
            Assert.AreEqual(false, character.CoverState.InHalfCover);
            Assert.AreEqual(false, character.CoverState.InFullCover);
            Assert.AreEqual(false, character.InOverwatch);
            Assert.AreEqual(false, character.HunkeredDown);
            Assert.IsNotNull(character.Abilities);
            Assert.AreEqual(1, character.Abilities.Count);
            Assert.AreEqual("Ability", character.Abilities[0].Name);
            Assert.AreEqual(AbilityType.Unknown, character.Abilities[0].Type);
            Assert.AreEqual(0, character.Abilities[0].Adjustment);
            Assert.IsNotNull(character.Effects);
            Assert.AreEqual(1, character.Effects.Count);
            Assert.AreEqual("Fire", character.Effects[0].Name);
            Assert.AreEqual(AbilityType.FireDamage, character.Effects[0].Type);
            Assert.AreEqual(1, character.Effects[0].Adjustment);
            Assert.AreEqual(2, character.Effects[0].TurnExpiration);

            Assert.IsTrue(actions != null);
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual("2", actions[0].KeyBinding);
        }

        private static void TestJethro(Character character)
        {
            Assert.IsNotNull(character);
            Assert.AreEqual("Jethro", character.Name);
            Assert.AreEqual(4, character.HitpointsMax);
            Assert.AreEqual(4, character.HitpointsCurrent);
            Assert.AreEqual(0, character.ArmorPointsMax);
            Assert.AreEqual(0, character.ArmorPointsCurrent);
            Assert.AreEqual(2, character.ActionPointsMax);
            Assert.AreEqual(2, character.ActionPointsCurrent);
            Assert.AreEqual(70, character.ChanceToHit);
            Assert.AreEqual(0, character.XP);
            Assert.AreEqual(1, character.Level);
            Assert.AreEqual(false, character.LevelUpIsReady);
            Assert.AreEqual(11, character.Speed);
            Assert.AreEqual(25, character.Intelligence);
            Assert.AreEqual(new(8, 0, 8), character.Location);
            Assert.AreEqual(2, character.ActionPointsCurrent);
            Assert.AreEqual(8, character.MobilityRange);
            Assert.AreEqual(30, character.ShootingRange);
            Assert.AreEqual(40, character.FOVRange);
            Assert.AreEqual(false, character.CoverState.InHalfCover);
            Assert.AreEqual(true, character.CoverState.InFullCover);
            Assert.AreEqual(false, character.InOverwatch);
            Assert.AreEqual(false, character.HunkeredDown);
        }

        private static void TestHarry(Character character)
        {
            Assert.IsNotNull(character);
            Assert.AreEqual("Harry", character.Name);
            Assert.AreEqual(12, character.HitpointsCurrent);
            Assert.AreEqual(1, character.ArmorPointsCurrent);
            Assert.AreEqual(70, character.ChanceToHit);
            Assert.AreEqual(0, character.XP);
            Assert.AreEqual(1, character.Level);
            Assert.AreEqual(false, character.LevelUpIsReady);
            Assert.AreEqual(12, character.Speed);
            Assert.AreEqual(75, character.Intelligence);
            Assert.AreEqual(new(5, 0, 5), character.Location);
            Assert.AreEqual(2, character.ActionPointsCurrent);
            Assert.AreEqual(8, character.MobilityRange);
            Assert.AreEqual(30, character.ShootingRange);
            Assert.AreEqual(40, character.FOVRange);
            Assert.AreEqual(false, character.CoverState.InHalfCover);
            Assert.AreEqual(false, character.CoverState.InFullCover);
            Assert.AreEqual(false, character.InOverwatch);
            Assert.AreEqual(false, character.HunkeredDown);
        }
    }
}
