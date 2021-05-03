using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        private static void TestFred(Character fred)
        {
            Assert.IsNotNull(fred);
            Assert.AreEqual("Fred", fred.Name);
            Assert.AreEqual(12, fred.HP);
            Assert.AreEqual(70, fred.ChanceToHit);
            Assert.AreEqual(10, fred.Initiative);
            Assert.AreEqual(0, fred.Modifier);
            Assert.AreEqual(0, fred.Experience);
            Assert.AreEqual(1, fred.Level);
            Assert.AreEqual(false, fred.LevelUpIsReady);
        }
    }
}
