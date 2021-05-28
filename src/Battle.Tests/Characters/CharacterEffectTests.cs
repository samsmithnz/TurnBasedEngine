using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterEffectTests
    {
        [TestMethod]
        public void CharacterEffectDoesDamageTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();

            //Act
            fred.ProcessEffects(1);

            //Assert
            Assert.AreEqual(11, fred.HitpointsCurrent);
        }

        [TestMethod]
        public void CharacterEffectExpiresDoesDamageTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();

            //Act
            fred.ProcessEffects(1);
            fred.ProcessEffects(2);

            //Assert
            Assert.AreEqual(0, fred.Effects.Count);
            Assert.AreEqual(11, fred.HitpointsCurrent);
        }

        [TestMethod]
        public void CharacterMultipleEffectExpiresDoesDamageTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Effects.Add(new()
            {
                Type = AbilityType.Unknown,
                TurnExpiration = 3
            });

            //Act
            fred.ProcessEffects(1);
            fred.ProcessEffects(2);

            //Assert
            Assert.AreEqual(1, fred.Effects.Count);
            Assert.AreEqual(11, fred.HitpointsCurrent);
        }

    }
}
