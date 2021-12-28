using Battle.Logic.Research;
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
            TestAdvancedWeapons(item);
        }

        [TestMethod]
        public void ResearchLasersTest()
        {
            //Arrange
            ResearchItem item = ResearchPool.CreateLasers();

            //Act            

            //Assert
            TestLasers(item);
        }

        [TestMethod]
        public void ResearchPlasmaTest()
        {
            //Arrange
            ResearchItem item = ResearchPool.CreatePlasma();

            //Act            

            //Assert
            TestPlasma(item);
        }

        private static void TestAdvancedWeapons(ResearchItem item)
        {
            Assert.IsNotNull(item);
            Assert.AreEqual("Advanced weapons", item.Name);
            Assert.IsNull(item.ResearchPrereq);
            Assert.IsNull(item.ItemPrereq);
            Assert.AreEqual(5, item.DaysToComplete);
            Assert.AreEqual(5, item.DaysCompleted);
            Assert.AreEqual(0, item.ScientistsAssigned);
            Assert.AreEqual(true, item.IsComplete);
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

        private static void TestPlasma(ResearchItem item)
        {
            Assert.IsNotNull(item);
            Assert.AreEqual("Plasma weapons", item.Name);
            Assert.IsNotNull(item.ResearchPrereq);
            Assert.AreEqual(ResearchPool.CreateLasers().Name, item.ResearchPrereq.Name);
            Assert.IsNull(item.ItemPrereq);
            Assert.AreEqual(5, item.DaysToComplete);
            Assert.AreEqual(0, item.DaysCompleted);
            Assert.AreEqual(0, item.ScientistsAssigned);
            Assert.AreEqual(false, item.IsComplete);
        }

    }
}