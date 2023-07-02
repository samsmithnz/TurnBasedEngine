using TBE.Logic.Characters;
using TBE.Logic.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterItemTests
    {
        [TestMethod]
        public void MedkitItemFredTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));
            fred.HitpointsCurrent = 1;
            fred.UtilityItemEquipped = ItemPool.CreateMedKit();

            //Act
            fred.UseItem(fred.UtilityItemEquipped);

            //Assert
            Assert.AreEqual(4, fred.HitpointsCurrent);
            Assert.AreEqual(0, fred.UtilityItemEquipped.ClipRemaining = 0);
        }


        [TestMethod]
        public void UnknownItemFredTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero(null, new(0, 0, 0));
            fred.HitpointsCurrent = 1;
            fred.UtilityItemEquipped = ItemPool.CreateMedKit();
            fred.UtilityItemEquipped.Type = Logic.Items.ItemType.Unknown;

            //Act
            fred.UseItem(fred.UtilityItemEquipped);

            //Assert
            Assert.AreEqual(1, fred.HitpointsCurrent);
            Assert.AreEqual(1, fred.UtilityItemEquipped.ClipRemaining);
        }
    }
}
