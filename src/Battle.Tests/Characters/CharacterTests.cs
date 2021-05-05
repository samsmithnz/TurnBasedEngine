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
            Assert.AreEqual(new System.Numerics.Vector3(0,0, 0), fred.Location);
            Assert.AreEqual(2, fred.ActionPoints);
            Assert.AreEqual(10, fred.Range);
            Assert.AreEqual(false, fred.InHalfCover);
            Assert.AreEqual(false, fred.InFullCover);
        }

        private static void TestHarry(Character harry)
        {
            Assert.IsNotNull(harry);
            Assert.AreEqual("Harry", harry.Name);
            Assert.AreEqual(12, harry.HP);
            Assert.AreEqual(70, harry.ChanceToHit);
            Assert.AreEqual(5, harry.Initiative);
            Assert.AreEqual(0, harry.Modifier);
            Assert.AreEqual(0, harry.Experience);
            Assert.AreEqual(1, harry.Level);
            Assert.AreEqual(false, harry.LevelUpIsReady);
            Assert.AreEqual(new System.Numerics.Vector3(5, 5, 5), harry.Location);
            Assert.AreEqual(2, harry.ActionPoints);
            Assert.AreEqual(10, harry.Range);
            Assert.AreEqual(true, harry.InHalfCover);
            Assert.AreEqual(false, harry.InFullCover);
        }

        private static void TestJeff(Character jeff)
        {
            Assert.IsNotNull(jeff);
            Assert.AreEqual("Jeff", jeff.Name);
            Assert.AreEqual(12, jeff.HP);
            Assert.AreEqual(70, jeff.ChanceToHit);
            Assert.AreEqual(10, jeff.Initiative);
            Assert.AreEqual(0, jeff.Modifier);
            Assert.AreEqual(0, jeff.Experience);
            Assert.AreEqual(1, jeff.Level);
            Assert.AreEqual(false, jeff.LevelUpIsReady);
            Assert.AreEqual(new System.Numerics.Vector3(10, 10, 10), jeff.Location);
            Assert.AreEqual(2, jeff.ActionPoints);
            Assert.AreEqual(10, jeff.Range);
            Assert.AreEqual(false, jeff.InHalfCover);
            Assert.AreEqual(true, jeff.InFullCover);
        }
    }
}
