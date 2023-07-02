using Battle.Logic.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Items
{
    [TestClass]
    [TestCategory("L0")]
    public class ItemTests
    {
        [TestMethod]
        public void ItemMedkitTest()
        {
            //Arrange
            Item medKit = ItemPool.CreateMedKit();

            //Act            

            //Assert
            TestMedKit(medKit);
        }

        private static void TestMedKit(Item medKit)
        {
            Assert.IsNotNull(medKit);
            Assert.AreEqual("MedKit", medKit.Name);
            Assert.AreEqual(3, medKit.Adjustment);
            Assert.AreEqual(1, medKit.Range);
            Assert.AreEqual(1, medKit.ClipSize);
            Assert.AreEqual(1, medKit.ClipRemaining);
            Assert.AreEqual(1, medKit.ActionPointsRequired);
            Assert.AreEqual(ItemType.MedKit, medKit.Type);
        }

    }
}