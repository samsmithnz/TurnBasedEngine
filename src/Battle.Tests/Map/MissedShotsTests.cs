using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class MissedShotsTests
    {
        [TestMethod]
        public void NorthEastTargetTest()
        {
            //Arrange
            Vector3 source = new(1, 0, 1);
            Vector3 target = new(5, 0, 5);
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(source, target, map);

            //Assert
            Assert.AreEqual(new Vector3(13, 0, 13), result);
        }

        [TestMethod]
        public void SouthEastTargetTest()
        {
            //Arrange
            Vector3 source = new(1, 0, 9);
            Vector3 target = new(5, 0, 5);
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(source, target, map);

            //Assert
            Assert.AreEqual(new Vector3(13, 0, -3), result);
        }

        [TestMethod]
        public void SouthWestTargetTest()
        {
            //Arrange
            Vector3 source = new(9, 0, 1);
            Vector3 target = new(5, 0, 5);
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(source, target, map);

            //Assert
            Assert.AreEqual(new Vector3(-3, 0, 13), result);
        }

        [TestMethod]
        public void NorthWestTargetTest()
        {
            //Arrange
            Vector3 source = new(9, 0, 1);
            Vector3 target = new(5, 0, 5);
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(source, target, map);

            //Assert
            Assert.AreEqual(new Vector3(-3, 0, 13), result);
        }

        [TestMethod]
        public void NorthEastSmallTargetTest()
        {
            //Arrange
            Vector3 source = new(0, 0, 0);
            Vector3 target = new(2, 0, 2);
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(source, target, map);

            //Assert
            Assert.AreEqual(new Vector3(10, 0, 10), result);
        }


    }
}
