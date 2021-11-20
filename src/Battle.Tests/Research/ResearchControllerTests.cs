using Battle.Logic.Research;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battle.Tests.Research
{
    [TestClass]
    [TestCategory("L0")]
    public class ResearchControllerTests
    {
        [TestMethod]
        public void ResearchItemsAreActiveTest()
        {
            //Arrange
            List<ResearchItem> list = new()
            {
                ResearchPool.CreateAdvancedWeapons(),
                ResearchPool.CreateLasers(),
                ResearchPool.CreatePlasma()
            };
            ResearchController controller = new()
            {
                ResearchItems = list
            };

            //Act
            List<ResearchItem> results = controller.GetAvailableResearchItems();

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void ResearchItemsAreCompletedTest()
        {
            //Arrange
            List<ResearchItem> list = new()
            {
                ResearchPool.CreateAdvancedWeapons(),
                ResearchPool.CreateLasers(),
                ResearchPool.CreatePlasma()
            };
            ResearchController controller = new()
            {
                ResearchItems = list
            };

            //Act
            List<ResearchItem> results = controller.GetCompletedResearchItems();

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

    }
}