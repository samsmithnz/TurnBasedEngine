using TBE.Logic.AbilitiesAndEffects;
using TBE.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TBE.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterEffectTests
    {
        [TestMethod]
        public void CharacterEffectDoesDamageTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));

            //Act
            fred.ProcessEffects(1);

            //Assert
            Assert.AreEqual(3, fred.HitpointsCurrent);
        }

        [TestMethod]
        public void CharacterEffectExpiresDoesDamageTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));

            //Act
            fred.ProcessEffects(1);
            fred.ProcessEffects(2);

            //Assert
            Assert.AreEqual(0, fred.Effects.Count);
            Assert.AreEqual(3, fred.HitpointsCurrent);
        }

        [TestMethod]
        public void CharacterMultipleEffectExpiresDoesDamageTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));
            fred.Effects.Add(new Effect()
            {
                Type = AbilityType.Unknown,
                TurnExpiration = 3
            });

            //Act
            fred.ProcessEffects(1);
            fred.ProcessEffects(2);

            //Assert
            Assert.AreEqual(1, fred.Effects.Count);
            Assert.AreEqual(3, fred.HitpointsCurrent);
        }

    }
}
