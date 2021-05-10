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
    public class FieldOfViewTests
    {
        [TestMethod]
        public void BasicShallowLineWithNoCoverTest()
        {
            //Arrange
            //string[,] map = MapUtility.InitializeMap(5, 5);

            //Act
            List<Vector3> results = FieldOfViewCalculator.GetPointsOnLine(1, 3, 4, 2).ToList<Vector3>();

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(4, results.Count);
            Assert.AreEqual(new Vector3(1, 0, 3), results[0]);
            Assert.AreEqual(new Vector3(2, 0, 3), results[1]);
            Assert.AreEqual(new Vector3(3, 0, 2), results[2]);
            Assert.AreEqual(new Vector3(4, 0, 2), results[3]);
        }

        [TestMethod]
        public void BasicSteepLineWithNoCoverTest()
        {
            //Arrange
            //string[,] map = MapUtility.InitializeMap(5, 5);

            //Act
            List<Vector3> results = FieldOfViewCalculator.GetPointsOnLine(1, 3, 3, 1).ToList<Vector3>();

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(new Vector3(1, 0, 3), results[0]);
            Assert.AreEqual(new Vector3(2, 0, 2), results[1]);
            Assert.AreEqual(new Vector3(3, 0, 1), results[2]);
        }

        [TestMethod]
        public void BasicShallowLineWithCoverTest()
        {
            //Arrange
            string[,] map = MapUtility.InitializeMap(5, 5);
            map[3, 2] = "W";

            //Act
            List<Vector3> results = FieldOfViewCalculator.GetPointsOnLine(1, 3, 4, 2).ToList<Vector3>();
            List<Vector3> newResults = new();
            foreach (Vector3 item in results)
            {
                if (map[(int)item.X, (int)item.Z] != "")
                {
                    break;
                }
                else
                {
                    newResults.Add(item);
                }
            }

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(2, newResults.Count);
            Assert.AreEqual(new Vector3(1, 0, 3), newResults[0]);
            Assert.AreEqual(new Vector3(2, 0, 3), newResults[1]);
        }

        [TestMethod]
        public void FieldOfViewRange1NNoCoverTest()
        {
            //Arrange            
            //  "P" = player/fred
            //  "■" = cover
            //  "□" = open ground
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ P □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            string[,] map = MapUtility.InitializeMap(10, 10);
            int range = 1;
            Vector3 startingLocation = new(4, 0, 4);

            //Act
            List<Vector3> results = FieldOfViewCalculator.GetFieldOfView(map, startingLocation, range);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(8, results.Count);
        }



        [TestMethod]
        public void FieldOfViewRange1WithCoverTest()
        {
            //Arrange            
            //  "P" = player/fred
            //  "■" = cover
            //  "□" = open ground
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ ■ P ■ □ □ □
            //  □ □ □ □ ■ ■ ■ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            string[,] map = MapUtility.InitializeMap(10, 10);
            int range = 1;
            Vector3 startingLocation = new(4, 0, 4);
            map[3, 4] = "W";
            map[3, 3] = "W";
            map[4, 3] = "W";
            map[5, 3] = "W";
            map[5, 4] = "W";

            //Act
            List<Vector3> results = FieldOfViewCalculator.GetFieldOfView(map, startingLocation, range);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        public void FieldOfViewRange12NoCoverTest()
        {
            //Arrange            
            //  "P" = player/fred
            //  "■" = cover
            //  "□" = open ground
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ P □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □ □ □ □
            string[,] map = MapUtility.InitializeMap(10, 10);
            int range = 12;
            Vector3 startingLocation = new(4, 0, 4);

            //Act
            List<Vector3> results = FieldOfViewCalculator.GetFieldOfView(map, startingLocation, range);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(99, results.Count);
        }

        [TestMethod]
        public void FieldOfViewRange5WithCoverTest()
        {
            //Arrange            
            //  "P" = player/fred
            //  "■" = cover
            //  "□" = open ground

            //  □ □ □ □ □ 
            //  □ □ □ □ □ 
            //  □ ■ P ■ □ 
            //  □ ■ ■ ■ □ 
            //  □ □ □ □ □ 
            string[,] map = MapUtility.InitializeMap(5, 5);
            int range = 10;
            Vector3 startingLocation = new(2, 0, 2);
            map[1, 2] = "W";
            map[1, 1] = "W";
            map[2, 1] = "W";
            map[3, 1] = "W";
            map[3, 2] = "W";

            //Act
            List<Vector3> results = FieldOfViewCalculator.GetFieldOfView(map, startingLocation, range);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(9, results.Count);
            foreach (Vector3 item in results)
            {
                Assert.AreNotEqual(0, item.Z);
                Assert.AreNotEqual(1, item.Z);
                Assert.AreNotEqual(2, item.Z);
            }
        }

    }
}
