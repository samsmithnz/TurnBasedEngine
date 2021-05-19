using Battle.Logic.FieldOfView;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Tests.FieldOfView
{
    [TestClass]
    [TestCategory("L0")]
    public class AreaEffectFieldOfViewTests
    {
        [TestMethod]
        public void Radius1AreaOfEffectAt4x4zTest()
        {
            //Arrange
            string[,] map = MapUtility.InitializeMap(5, 5);
            Vector3 location = new(4, 0, 4);
            int radius = 1;

            //Act
            List<Vector3> results = FieldOfViewAreaEffectCalculator.GetAreaOfEffect(map, location, radius);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(new Vector3(4, 0, 4), results[0]);
            Assert.AreEqual(new Vector3(3, 0, 4), results[1]);
            Assert.AreEqual(new Vector3(4, 0, 3), results[2]);
        }

        [TestMethod]
        public void Radius1AreaOfEffectAt1x1zTest()
        {
            //Arrange
            string[,] map = MapUtility.InitializeMap(5, 5);
            Vector3 location = new(1, 0, 1);
            int radius = 1;

            //Act
            List<Vector3> results = FieldOfViewAreaEffectCalculator.GetAreaOfEffect(map, location, radius);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(new Vector3(1, 0, 1), results[0]);
            Assert.AreEqual(new Vector3(0, 0, 1), results[1]);
            Assert.AreEqual(new Vector3(1, 0, 0), results[2]);
            Assert.AreEqual(new Vector3(1, 0, 2), results[3]);
            Assert.AreEqual(new Vector3(2, 0, 1), results[4]);
        }

        [TestMethod]
        public void Radius1AreaOfEffectAt0x0zTest()
        {
            //Arrange
            string[,] map = MapUtility.InitializeMap(5, 5);
            Vector3 location = new(0, 0, 0);
            int radius = 1;

            //Act
            List<Vector3> results = FieldOfViewAreaEffectCalculator.GetAreaOfEffect(map, location, radius);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(new Vector3(0, 0, 0), results[0]);
            Assert.AreEqual(new Vector3(0, 0, 1), results[1]);
            Assert.AreEqual(new Vector3(1, 0, 0), results[2]);
        }


        [TestMethod]
        public void Radius3AreaOfEffectAt4x4zTest()
        {
            //Arrange
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 location = new(4, 0, 4);
            int radius = 3;

            //Act
            List<Vector3> results = FieldOfViewAreaEffectCalculator.GetAreaOfEffect(map, location, radius);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(37, results.Count);
            Assert.AreEqual(new Vector3(4, 0, 4), results[0]);
            Assert.AreEqual(new Vector3(3, 0, 4), results[1]);
            Assert.AreEqual(new Vector3(4, 0, 3), results[2]);
            Assert.AreEqual(new Vector3(7, 0, 5), results[36]);
        }

        [TestMethod]
        public void Radius3AreaOfEffectAt0x0zTest()
        {
            //Arrange
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 location = new(0, 0, 0);
            int radius = 3;

            //Act
            List<Vector3> results = FieldOfViewAreaEffectCalculator.GetAreaOfEffect(map, location, radius);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(13, results.Count);
            Assert.AreEqual(new Vector3(0, 0, 0), results[0]);
            Assert.AreEqual(new Vector3(3, 0, 1), results[12]);
        }



    }
}
