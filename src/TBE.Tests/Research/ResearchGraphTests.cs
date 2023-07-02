using TBE.Logic.Research;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Research
{
    [TestClass]
    [TestCategory("L0")]
    public class ResearchGraphTests
    {
        [TestMethod]
        public void ResearchLaserGraphTest()
        {
            //Arrange
            ResearchItem item = ResearchPool.CreateLasers();

            //Act            


            //Assert
            TestLasers(item);
        }
      
        private static void TestLasers(ResearchItem item)
        {
            Assert.IsNotNull(item);
            Assert.AreEqual("Laser weapons", item.Name);
            Assert.IsNotNull(item.ResearchPrereq);
            Assert.AreEqual(ResearchPool.CreateAdvancedWeapons().Name, item.ResearchPrereq.Name);
            Assert.IsNull(item.ItemPrereq);
            Assert.AreEqual(5, item.DaysToComplete);
            Assert.AreEqual(3, item.DaysCompleted);
            Assert.AreEqual(2, item.ScientistsAssigned);
            Assert.AreEqual(false, item.IsComplete);
        }

    }
}