using Battle.Logic.PathFinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Tests.PathFinding
{
    [TestClass]
    [TestCategory("L0")]
    public class PathFindingTests
    {

        [TestMethod]
        public void Test_WithoutWalls_CanFindPath()
        {
            //Arrange
            Vector3 startLocation = new(1,0, 2);
            Vector3 endLocation = new(5,0, 2);
            string[,] map = InitializeMap(7, 5);

            //Act
            PathResult pathResult = Path.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(pathResult);
            Assert.IsNotNull(pathResult.Path);
            Assert.IsTrue(pathResult.Path.Any());
            Assert.AreEqual(4, pathResult.Path.Count);
        }

        [TestMethod]
        public void Test_WithOpenWall_CanFindPathAroundWall()
        {
            //Arrange
            //  □ □ □ ■ □ □ □
            //  □ □ □ ■ □ □ □
            //  □ S □ ■ □ F □
            //  □ □ * ■ ■ * □
            //  □ □ □ * * □ □

            // Path: 1,2 ; 2,1 ; 3,0 ; 4,0 ; 5,1 ; 5,2
            Vector3 startLocation = new(1,0, 2);
            Vector3 endLocation = new(5,0, 2);
            string[,] map = InitializeMap(7, 5);
            map[3, 4] = "W";
            map[3, 3] = "W";
            map[3, 2] = "W";
            map[3, 1] = "W";
            map[4, 1] = "W";

            //Act
            PathResult pathResult = Path.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(pathResult);
            Assert.IsNotNull(pathResult.Path);
            Assert.IsTrue(pathResult.Path.Any());
            Assert.IsTrue(pathResult.GetLastTile() != null);
            Assert.IsTrue(pathResult.GetLastTile().TraversalCost == 6);
            Assert.IsTrue(pathResult.Path.Count == 5);
        }

        [TestMethod]
        public void Test_WithClosedWall_CannotFindPath()
        {
            //Arrange
            //  □ □ □ ■ □ □ □
            //  □ □ □ ■ □ □ □
            //  □ S □ ■ □ F □
            //  □ □ □ ■ □ □ □
            //  □ □ □ ■ □ □ □

            // No path
            Vector3 startLocation = new(1, 0, 2);
            Vector3 endLocation = new(5, 0, 2);
            string[,] map = InitializeMap(7, 5);
            map[3, 4] = "W";
            map[3, 3] = "W";
            map[3, 2] = "W";
            map[3, 1] = "W";
            map[3, 0] = "W";

            //Act
            PathResult pathResult = Path.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(pathResult);
            Assert.IsNotNull(pathResult.Path);
            Assert.IsFalse(pathResult.Path.Any());
            Assert.IsFalse(pathResult.Tiles.Any());
        }


        [TestMethod]
        public void Test_WithMazeWall()
        {
            //Arrange
            //  S ■ ■ □ ■ ■ F
            //  * ■ □ ■ □ ■ □
            //  * ■ □ ■ □ ■ □
            //  * ■ □ ■ □ ■ □
            //  ■ □ ■ ■ ■ □ ■

            // long path
            Vector3 startLocation = new(0, 0, 4);
            Vector3 endLocation = new(6, 0, 4);
            string[,] map = InitializeMap(7, 5);
            map[0, 0] = "W";
            map[1, 4] = "W";
            map[1, 3] = "W";
            map[1, 2] = "W";
            map[1, 1] = "W";
            map[2, 4] = "W";
            map[2, 0] = "W";
            map[3, 3] = "W";
            map[3, 2] = "W";
            map[3, 1] = "W";
            map[3, 0] = "W";
            map[4, 4] = "W";
            map[4, 0] = "W";
            map[5, 4] = "W";
            map[5, 3] = "W";
            map[5, 2] = "W";
            map[5, 1] = "W";
            map[6, 0] = "W";

            //Act
            PathResult pathResult = Path.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(pathResult);
            Assert.IsNotNull(pathResult.Path);
            Assert.IsTrue(pathResult.Path.Any());
            Assert.IsTrue(pathResult.GetLastTile() != null);
            Assert.IsTrue(pathResult.GetLastTile().TraversalCost == 18);
            Assert.AreEqual(16, pathResult.Path.Count);
        }


        [TestMethod]
        public void Test_GiantRandomMap_WithInefficentPath()
        {
            //Arrange
            Vector3 startLocation = new(0, 0, 0);
            Vector3 endLocation = new(69, 0, 39);
            string[,] map = CreateGiantMap();

            //Act
            PathResult pathResult = Path.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(pathResult);
            Assert.IsNotNull(pathResult.Path);
            Assert.IsTrue(pathResult.Path.Any());
            Assert.AreEqual(97, pathResult.Path.Count);
            CreateDebugPictureOfMapAndRoute(70, 40, pathResult.Path, map);
        }

        [TestMethod]
        public void Test_Contained_RangeOf1_NoPath()
        {
            // 4 □ □ □ □ F 
            // 3 □ ■ ■ ■ □ 
            // 2 □ ■ S ■ □ 
            // 1 □ ■ ■ ■ □ 
            // 0 □ □ □ □ □ 
            //   0 1 2 3 4 

            //Arrange
            int height = 5;
            int width = 5;
            string[,] map = InitializeMap(width, height);
            map[1, 1] = "W";
            map[1, 2] = "W";
            map[1, 3] = "W";
            map[2, 1] = "W";
            map[2, 3] = "W";
            map[3, 1] = "W";
            map[3, 2] = "W";
            map[3, 3] = "W";
            Vector3 startLocation = new(2, 0, 2);
            Vector3 endLocation = new(2, 0, 4);

            //Act
            PathResult pathResult = Path.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(pathResult);
            Assert.IsNotNull(pathResult.Path);
            Assert.IsTrue(pathResult.Path.Count == 0);
            Assert.IsTrue(pathResult.Tiles.Count == 0);
            Assert.IsTrue(pathResult.GetLastTile() == null);
            //Assert.IsTrue(pathResult.GetLastTile().TraversalCost == 2);
        }

        [TestMethod]
        public void Test_Contained_RangeOf1_StartIsSameAsFinish()
        {
            // 4 □ □ □ □ □ 
            // 3 □ ■ ■ ■ □ 
            // 2 □ ■ S ■ □ 
            // 1 □ ■ ■ ■ □ 
            // 0 □ □ □ □ □ 
            //   0 1 2 3 4 

            //Arrange
            Vector3 startLocation = new(2, 0, 2);
            Vector3 endLocation = new(2, 0, 2);
            int height = 5;
            int width = 5;
            string[,] map = InitializeMap(width, height);
            map[1, 1] = "W";
            map[1, 2] = "W";
            map[1, 3] = "W";
            map[2, 1] = "W";
            map[2, 3] = "W";
            map[3, 1] = "W";
            map[3, 2] = "W";
            map[3, 3] = "W";

            //Act
            PathResult pathResult = Path.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(pathResult);
            Assert.IsNotNull(pathResult.Path);
            Assert.IsTrue(pathResult.Path.Count == 0);
            Assert.IsTrue(pathResult.Tiles.Count == 0);
            Assert.IsTrue(pathResult.GetLastTile() == null);
        }

        #region "private helper functions"

        private static string[,] InitializeMap(int xMax, int zMax)
        {
            //  □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □
            //  □ S □ □ □ F □
            //  □ □ □ □ □ □ □
            //  □ □ □ □ □ □ □

            string[,] map = new string[xMax, zMax];
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    map[x, z] = "";
                }
            }

            return map;
        }


        private static void CreateDebugPictureOfMapAndRoute(int xMax, int zMax, List<Vector3> path, string[,] map)
        {
            string[,] mapDebug = new string[xMax, zMax];
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (map[x, z] == "")
                    {
                        mapDebug[x, z] = " □";
                    }
                    else if (map[x, z] != "")
                    {
                        mapDebug[x, z] = " ■";
                    }
                }
            }

            int i = 0;
            foreach (Vector3 item in path)
            {
                if (i == 0)
                {
                    mapDebug[0, 0] = " S";
                }
                if (i == path.Count - 1)
                {
                    mapDebug[(int)item.X, (int)item.Z] = " F";
                }
                else
                {
                    mapDebug[(int)item.X, (int)item.Z] = " *";
                }
                i++;
            }

            //for (int z = 0; z < zMax; z++)
            for (int z = zMax - 1; z >= 0; z--)
            {
                for (int x = 0; x < xMax; x++)
                {
                    System.Diagnostics.Debug.Write(mapDebug[x, z]);
                }
                System.Diagnostics.Debug.WriteLine("");
            }

        }


        #region "Create huge map"

        private static string[,] CreateGiantMap()
        {
            //□ ■ □ □ □ ■ ■ □ ■ ■ ■ ■ □ □ □ ■ □ ■ □ □ □ ■ ■ □ □ ■ □ □ □ □ ■ □ □ □ ■ □ ■ □ ■ □ ■ ■ ■ ■ ■ □ ■ ■ ■ □ □ ■ ■ ■ □ □ □ □ ■ □ □ □ □ □ ■ ■ □ □ □ F
            //■ ■ □ ■ □ ■ ■ □ □ □ ■ ■ □ ■ □ □ □ □ □ ■ □ □ □ □ □ □ □ ■ □ □ □ ■ □ ■ □ □ □ ■ □ □ □ □ □ □ ■ □ □ ■ ■ □ □ □ ■ ■ ■ □ □ □ ■ □ □ ■ ** □ ■ □ □ * ■
            //■ □ ■ □ ■ ■ ■ □ ■ □ □ □ ■ ■ □ □ □ □ □ ■ □ ■ □ □ ■ □ ■ □ □ □ □ □ □ □ □ □ □ □ □ □ ■ □ ■ □ □ □ □ □ □ □ ■ □ ■ ■ ■ □ □ □ □ □ ** ■ ■ * ■ ■ * ■ □
            //■ □ ■ □ ■ ■ ■ □ □ □ □ □ ■ ■ ■ □ □ ■ □ □ □ □ ■ □ □ □ □ □ □ ■ □ □ □ □ □ ■ □ □ □ □ □ ■ ■ ■ □ ■ ■ ■ □ ■ □ □ □ ■ □ □ □ ■ □ * ■ □ ■ □ □ * * □ ■ □
            //□ ■ □ □ □ ■ □ □ □ □ □ ■ ■ ■ □ □ □ □ ■ ■ ■ ■ ■ □ □ ■ □ □ ■ □ □ ■ ■ ■ ■ □ □ ■ ■ □ ■ ■ ■ □ ■ □ □ □ □ ■ □ □ ■ ■ □ □ □ □ * ■ □ ■ ■ □ ■ ■ □ ■ □ ■
            //■ ■ □ ■ □ ■ □ □ □ ■ ■ □ ■ □ □ □ □ ■ ■ □ □ ■ □ □ □ □ ■ □ □ ■ ■ □ ■ □ ■ □ □ □ □ □ ■ □ ■ ■ ■ ■ □ □ ■ ■ □ ■ □ □ □ ■ □ * ■ □ ■ □ ■ □ ■ ■ □ ■ □ □
            //□ □ ■ □ ■ ■ ■ ■ □ ■ ■ ■ □ □ ■ □ □ □ ■ ■ □ □ □ ■ □ □ ■ □ ■ □ □ ■ □ ■ ■ □ ■ ■ □ □ □ □ □ ■ ■ □ □ □ ■ □ ■ □ □ ■ ■ □ □ * ■ ■ □ □ □ □ □ ■ □ □ ■ □
            //□ ■ □ ■ ■ □ ■ □ ■ □ □ □ □ ■ □ □ ■ □ □ ■ □ ■ □ □ ■ □ ■ ■ ■ □ □ ■ □ □ □ □ ■ □ □ ■ ■ □ ■ □ □ ■ ■ ■ ■ □ □ ■ □ ■ ■ □ * ■ □ □ □ ■ ■ □ ■ □ □ ■ □ ■
            //□ ■ □ □ □ □ □ □ ■ □ □ □ ■ ■ □ ■ □ ■ □ □ □ ■ ■ □ □ □ □ ■ □ □ ■ □ □ □ □ □ □ □ ■ □ □ ■ □ □ □ □ ■ ■ □ □ □ ■ □ ■ □ □ * ■ ■ □ □ □ ■ □ □ □ ■ □ □ ■
            //□ ■ □ □ ■ ■ □ □ □ ■ ■ ■ □ ■ □ ■ ■ □ □ □ ■ □ ■ ■ □ ■ □ ■ ■ ■ □ □ □ ■ ■ ■ □ □ □ ■ ■ □ □ ■ □ ■ ■ □ ■ □ ■ ■ ■ □ ■ * ■ ■ □ ■ □ ■ □ □ □ □ ■ □ □ □
            //■ □ □ □ □ □ □ □ ■ ■ □ ■ □ ■ ■ □ □ □ □ ■ □ □ ■ □ □ □ □ □ □ ■ ■ ■ ■ □ □ □ □ □ ■ ■ ■ □ ■ ■ ■ ■ □ □ □ ■ * □ ■ □ ■ □ * ■ ■ ■ ■ ■ ■ □ ■ ■ □ □ □ □
            //■ ■ □ □ □ ■ ■ □ ■ □ ■ □ □ □ ■ ■ ■ □ □ ■ ■ □ □ □ ■ □ □ □ ■ ■ □ ■ ■ □ □ ■ □ * * * ■ ■ ■ ■ □ ■ ■ ■ * * ■ * ■ ■ ■ * ■ ■ □ □ □ □ □ □ □ ■ □ □ □ ■
            //□ □ □ □ ■ ■ □ □ ■ □ ■ □ □ □ ■ ■ ■ ■ ■ □ □ ■ □ □ □ ■ □ □ □ ■ ■ □ □ □ * * * ■ ■ ■ * * * ■ * * * * ■ ■ ■ □ * * ■ * □ ■ ■ □ □ □ ■ ■ ■ □ □ □ □ ■
            //□ ■ □ □ ■ □ ■ □ ■ ■ □ □ ■ □ □ ■ □ □ ■ ■ □ □ □ ■ □ ■ □ □ ■ □ □ □ □ * ■ ■ ■ □ □ □ ■ □ ■ * □ □ □ □ □ □ □ □ □ ■ * □ ■ ■ □ ■ □ □ ■ ■ ■ □ ■ □ □ □
            //□ ■ □ □ □ ■ ■ ■ □ □ □ □ □ ■ □ □ ■ ■ ■ ■ ■ ■ □ □ □ ■ □ ■ ■ ■ □ ■ □ ■ * ■ □ □ □ ■ ■ □ □ ■ ■ □ □ ■ □ □ ■ □ □ □ ■ □ ■ □ ■ □ ■ ■ □ □ □ ■ □ □ □ ■
            //□ □ ■ □ □ □ □ □ □ □ □ ■ ■ ■ ■ □ □ ■ □ ■ □ ■ □ □ □ ■ ■ ■ ■ ■ ■ □ □ □ * ■ □ ■ ■ ■ □ ■ □ □ ■ □ □ ■ □ ■ □ ■ □ ■ □ ■ □ □ □ □ □ □ ■ □ ■ ■ □ □ □ ■
            //□ □ □ □ ■ □ □ □ □ ■ ■ □ ■ ■ ■ □ □ ■ ■ □ □ □ □ □ ■ □ ■ □ ■ □ □ ■ □ □ * ■ □ ■ □ □ ■ □ ■ ■ ■ □ ■ ■ □ ■ □ ■ □ □ □ ■ □ □ ■ ■ □ □ □ □ □ □ □ □ □ □
            //■ □ □ □ □ ■ ■ □ ■ □ ■ □ □ □ □ ■ □ □ □ ■ ■ □ □ □ □ ■ ■ □ □ □ ■ ■ □ * ■ ■ □ ■ ■ □ ■ ■ ■ □ ■ □ ■ □ □ □ □ □ □ □ □ □ □ □ □ ■ ■ ■ ■ □ □ ■ ■ □ □ ■
            //□ □ □ □ □ □ ■ □ □ ■ □ □ □ ■ ■ ■ □ □ □ □ ■ □ □ ■ □ □ □ ■ ■ ■ ■ ■ □ * ■ ■ ■ ■ ■ ■ ■ □ ■ ■ ■ □ ■ □ □ ■ □ ■ □ □ □ ■ □ □ □ ■ □ □ □ □ □ ■ □ ■ □ □
            //□ □ □ □ □ □ □ ■ ■ □ ■ □ □ □ ■ ■ ■ □ □ □ □ ■ ■ □ ■ ■ ■ □ ■ ■ ■ □ * ■ ■ ■ □ ■ □ ■ □ □ ■ □ □ ■ □ □ □ ■ ■ □ □ □ ■ ■ ■ ■ □ □ ■ ■ ■ □ ■ □ □ □ □ □
            //■ □ □ ■ ■ ■ □ ■ □ □ ■ □ ■ □ □ ■ □ ■ □ □ □ □ □ ■ ■ ■ □ □ □ ■ □ * □ ■ ■ □ □ ■ □ □ □ □ □ □ ■ □ □ ■ ■ □ □ ■ □ □ □ □ □ ■ □ ■ □ □ □ □ ■ ■ □ ■ □ □
            //■ ■ ■ ■ □ ■ □ □ ■ □ □ □ □ □ □ □ ■ ■ □ ■ ■ □ □ □ □ □ □ □ □ □ * ■ ■ ■ ■ ■ □ ■ □ □ ■ □ ■ □ □ ■ ■ ■ □ □ □ ■ ■ □ □ ■ □ □ ■ ■ ■ ■ ■ □ ■ □ □ ■ □ □
            //■ ■ □ ■ ■ □ ■ □ □ □ ■ □ □ □ □ □ □ □ □ ■ ■ □ □ □ □ ■ □ ■ ■ ■ □ * * * * * □ ■ □ □ □ ■ □ □ □ ■ ■ ■ □ □ □ □ ■ □ □ ■ □ □ □ □ □ ■ ■ ■ ■ □ □ ■ ■ ■
            //□ □ □ □ □ □ □ ■ ■ □ ■ ■ ■ □ □ □ ■ ■ □ ■ □ □ ■ ■ □ □ ■ ■ □ □ □ ■ □ ■ □ ■ * ■ □ □ ■ □ □ □ □ ■ □ □ □ □ □ ■ □ □ ■ □ □ □ □ □ □ □ □ ■ ■ ■ □ ■ □ □
            //■ ■ □ ■ □ ■ □ ■ ■ □ ■ □ □ □ ■ □ ■ □ □ ■ □ ■ ■ ■ □ □ ■ □ ■ □ □ ■ ■ □ ■ * □ ■ □ ■ □ □ □ □ □ □ □ ■ □ ■ □ ■ □ ■ □ □ ■ □ ■ □ ■ □ ■ □ ■ ■ ■ □ □ □
            //■ ■ ■ □ □ ■ □ ■ ■ ■ ■ □ □ ■ ■ ■ □ □ ■ ■ □ □ □ □ □ ■ □ ■ ■ □ * * ■ ■ * ■ □ □ ■ ■ □ ■ ■ ■ □ ■ □ □ ■ □ □ ■ □ □ □ □ □ □ □ ■ ■ □ □ ■ ■ □ □ □ ■ □
            //■ ■ ■ □ ■ □ ■ □ ■ ■ ■ □ □ ■ □ ■ ■ ■ ■ □ □ □ ■ ■ ■ □ ■ ■ ■ * □ □ * * □ □ □ □ □ □ □ □ □ □ ■ □ ■ □ ■ □ □ ■ ■ □ □ ■ ■ ■ □ □ □ □ □ □ □ ■ ■ □ □ □
            //□ □ ■ □ □ □ □ □ □ □ □ ■ ■ * * ■ □ ■ □ □ □ ■ □ * ■ ■ ■ * * □ □ □ □ □ □ ■ □ ■ □ □ ■ ■ ■ ■ □ ■ ■ ■ ■ □ ■ ■ □ □ □ ■ ■ ■ ■ □ □ □ □ ■ ■ □ ■ ■ ■ □
            //□ □ ■ ■ □ ■ □ ■ ■ □ □ * * □ ■ * □ ■ □ ■ □ □ * ■ * ■ * □ □ □ ■ □ □ ■ ■ □ ■ □ ■ □ □ ■ □ □ □ □ □ □ □ □ ■ □ ■ □ □ □ □ □ □ ■ ■ ■ ■ □ □ □ ■ □ □ □
            //■ □ □ ■ □ □ ■ ■ □ ■ * ■ □ ■ □ ■ * ■ ■ ■ ■ □ * ■ □ * □ □ □ ■ ■ ■ □ ■ □ ■ □ ■ □ ■ □ ■ ■ □ □ ■ ■ ■ □ □ ■ □ □ □ □ ■ ■ □ ■ □ □ □ ■ □ □ □ ■ □ □ □
            //□ ■ □ □ □ ■ ■ □ * * ■ ■ □ □ □ * ■ ■ □ ■ □ * ■ □ □ ■ ■ ■ ■ □ □ ■ □ □ □ ■ □ □ □ ■ □ □ ■ ■ ■ □ □ ■ □ □ ■ ■ □ □ □ ■ ■ □ □ □ ■ ■ □ □ ■ □ □ □ ■ ■
            //□ □ ■ □ ■ ■ ■ * ■ ■ □ ■ □ ■ □ * ■ * * ■ * □ □ □ ■ ■ ■ □ ■ ■ ■ ■ □ □ ■ □ ■ □ ■ □ □ □ ■ □ ■ ■ ■ ■ □ □ □ □ □ ■ □ ■ □ □ □ ■ □ ■ □ ■ □ □ □ □ □ □
            //■ □ □ □ ■ * * ■ □ ■ □ ■ ■ □ □ □ * □ □ * □ ■ □ □ □ □ □ □ □ □ □ □ □ □ ■ ■ □ ■ □ □ □ □ □ □ □ □ □ ■ ■ □ ■ ■ □ ■ □ □ □ ■ □ □ □ □ ■ ■ ■ □ ■ □ □ ■
            //□ □ ■ □ * ■ ■ □ □ ■ □ ■ □ ■ □ ■ ■ □ ■ □ □ □ □ ■ ■ □ ■ ■ ■ ■ □ □ □ ■ ■ □ ■ ■ □ □ □ □ □ □ ■ ■ □ ■ □ □ ■ □ □ □ □ ■ □ □ □ ■ □ ■ ■ □ □ ■ ■ □ ■ ■
            //□ ■ ■ □ * ■ □ □ □ ■ □ □ □ □ □ ■ □ □ ■ □ □ □ □ □ □ ■ □ □ ■ □ □ ■ ■ □ □ ■ ■ ■ □ □ □ □ □ ■ □ □ □ □ ■ ■ ■ ■ ■ □ □ □ □ □ □ □ ■ □ ■ □ □ □ □ □ □ ■
            //■ ■ □ * ■ □ □ □ □ ■ □ □ ■ ■ □ □ □ □ ■ ■ ■ □ □ □ ■ ■ ■ ■ ■ □ □ □ □ □ □ □ ■ ■ □ □ ■ ■ □ □ ■ □ □ □ □ □ ■ □ □ □ □ □ ■ ■ □ ■ □ □ ■ □ ■ ■ ■ ■ □ ■
            //□ □ □ * ■ □ □ □ □ □ □ ■ □ ■ □ □ □ □ □ □ □ ■ ■ □ □ □ ■ □ □ □ ■ ■ □ □ ■ □ □ □ □ □ □ ■ ■ □ ■ □ □ □ □ □ □ ■ □ □ □ ■ □ ■ □ □ □ □ □ ■ □ □ □ □ □ □
            //□ □ * ■ □ ■ □ ■ ■ □ ■ □ □ □ ■ ■ □ □ ■ □ □ □ ■ □ ■ □ □ □ □ ■ ■ □ ■ ■ ■ □ □ ■ □ ■ □ □ □ □ □ □ □ ■ ■ □ □ ■ ■ □ ■ □ □ ■ □ ■ □ ■ □ ■ □ ■ □ □ □ □
            //■ * ■ □ □ □ □ □ ■ ■ □ □ ■ ■ ■ ■ ■ ■ □ □ □ □ □ ■ □ □ □ □ □ □ □ □ □ □ □ □ ■ ■ □ □ □ ■ □ ■ □ ■ □ ■ □ ■ □ ■ ■ □ □ □ ■ □ ■ □ □ □ □ ■ □ □ ■ □ □ □
            //S □ □ □ □ □ ■ □ ■ □ ■ □ □ □ □ □ □ ■ ■ □ ■ □ □ ■ ■ □ □ □ □ □ ■ □ □ ■ ■ □ ■ □ □ □ ■ □ ■ □ ■ □ ■ □ □ □ □ □ ■ ■ □ □ □ □ ■ □ □ ■ ■ ■ □ □ □ ■ □ □



            string[,] map = InitializeMap(70, 40);
            map[6, 0] = "W";
            map[8, 0] = "W";
            map[10, 0] = "W";
            map[17, 0] = "W";
            map[18, 0] = "W";
            map[20, 0] = "W";
            map[23, 0] = "W";
            map[24, 0] = "W";
            map[30, 0] = "W";
            map[33, 0] = "W";
            map[34, 0] = "W";
            map[36, 0] = "W";
            map[40, 0] = "W";
            map[42, 0] = "W";
            map[44, 0] = "W";
            map[46, 0] = "W";
            map[52, 0] = "W";
            map[53, 0] = "W";
            map[58, 0] = "W";
            map[61, 0] = "W";
            map[62, 0] = "W";
            map[63, 0] = "W";
            map[67, 0] = "W";
            map[0, 1] = "W";
            map[2, 1] = "W";
            map[8, 1] = "W";
            map[9, 1] = "W";
            map[12, 1] = "W";
            map[13, 1] = "W";
            map[14, 1] = "W";
            map[15, 1] = "W";
            map[16, 1] = "W";
            map[17, 1] = "W";
            map[23, 1] = "W";
            map[36, 1] = "W";
            map[37, 1] = "W";
            map[41, 1] = "W";
            map[43, 1] = "W";
            map[45, 1] = "W";
            map[47, 1] = "W";
            map[49, 1] = "W";
            map[51, 1] = "W";
            map[52, 1] = "W";
            map[56, 1] = "W";
            map[58, 1] = "W";
            map[63, 1] = "W";
            map[66, 1] = "W";
            map[3, 2] = "W";
            map[5, 2] = "W";
            map[7, 2] = "W";
            map[8, 2] = "W";
            map[10, 2] = "W";
            map[14, 2] = "W";
            map[15, 2] = "W";
            map[18, 2] = "W";
            map[22, 2] = "W";
            map[24, 2] = "W";
            map[29, 2] = "W";
            map[30, 2] = "W";
            map[32, 2] = "W";
            map[33, 2] = "W";
            map[34, 2] = "W";
            map[37, 2] = "W";
            map[39, 2] = "W";
            map[47, 2] = "W";
            map[48, 2] = "W";
            map[51, 2] = "W";
            map[52, 2] = "W";
            map[54, 2] = "W";
            map[57, 2] = "W";
            map[59, 2] = "W";
            map[61, 2] = "W";
            map[63, 2] = "W";
            map[65, 2] = "W";
            map[4, 3] = "W";
            map[11, 3] = "W";
            map[13, 3] = "W";
            map[21, 3] = "W";
            map[22, 3] = "W";
            map[26, 3] = "W";
            map[30, 3] = "W";
            map[31, 3] = "W";
            map[34, 3] = "W";
            map[41, 3] = "W";
            map[42, 3] = "W";
            map[44, 3] = "W";
            map[51, 3] = "W";
            map[55, 3] = "W";
            map[57, 3] = "W";
            map[63, 3] = "W";
            map[0, 4] = "W";
            map[1, 4] = "W";
            map[4, 4] = "W";
            map[9, 4] = "W";
            map[12, 4] = "W";
            map[13, 4] = "W";
            map[18, 4] = "W";
            map[19, 4] = "W";
            map[20, 4] = "W";
            map[24, 4] = "W";
            map[25, 4] = "W";
            map[26, 4] = "W";
            map[27, 4] = "W";
            map[28, 4] = "W";
            map[36, 4] = "W";
            map[37, 4] = "W";
            map[40, 4] = "W";
            map[41, 4] = "W";
            map[44, 4] = "W";
            map[50, 4] = "W";
            map[56, 4] = "W";
            map[57, 4] = "W";
            map[59, 4] = "W";
            map[62, 4] = "W";
            map[64, 4] = "W";
            map[65, 4] = "W";
            map[66, 4] = "W";
            map[67, 4] = "W";
            map[69, 4] = "W";
            map[1, 5] = "W";
            map[2, 5] = "W";
            map[5, 5] = "W";
            map[9, 5] = "W";
            map[15, 5] = "W";
            map[18, 5] = "W";
            map[25, 5] = "W";
            map[28, 5] = "W";
            map[31, 5] = "W";
            map[32, 5] = "W";
            map[35, 5] = "W";
            map[36, 5] = "W";
            map[37, 5] = "W";
            map[43, 5] = "W";
            map[48, 5] = "W";
            map[49, 5] = "W";
            map[50, 5] = "W";
            map[51, 5] = "W";
            map[52, 5] = "W";
            map[60, 5] = "W";
            map[62, 5] = "W";
            map[69, 5] = "W";
            map[2, 6] = "W";
            map[5, 6] = "W";
            map[6, 6] = "W";
            map[9, 6] = "W";
            map[11, 6] = "W";
            map[13, 6] = "W";
            map[15, 6] = "W";
            map[16, 6] = "W";
            map[18, 6] = "W";
            map[23, 6] = "W";
            map[24, 6] = "W";
            map[26, 6] = "W";
            map[27, 6] = "W";
            map[28, 6] = "W";
            map[29, 6] = "W";
            map[33, 6] = "W";
            map[34, 6] = "W";
            map[36, 6] = "W";
            map[37, 6] = "W";
            map[44, 6] = "W";
            map[45, 6] = "W";
            map[47, 6] = "W";
            map[50, 6] = "W";
            map[55, 6] = "W";
            map[59, 6] = "W";
            map[61, 6] = "W";
            map[62, 6] = "W";
            map[65, 6] = "W";
            map[66, 6] = "W";
            map[68, 6] = "W";
            map[69, 6] = "W";
            map[0, 7] = "W";
            map[4, 7] = "W";
            map[7, 7] = "W";
            map[9, 7] = "W";
            map[11, 7] = "W";
            map[12, 7] = "W";
            map[21, 7] = "W";
            map[34, 7] = "W";
            map[35, 7] = "W";
            map[37, 7] = "W";
            map[47, 7] = "W";
            map[48, 7] = "W";
            map[50, 7] = "W";
            map[51, 7] = "W";
            map[53, 7] = "W";
            map[57, 7] = "W";
            map[62, 7] = "W";
            map[63, 7] = "W";
            map[64, 7] = "W";
            map[66, 7] = "W";
            map[69, 7] = "W";
            map[2, 8] = "W";
            map[4, 8] = "W";
            map[5, 8] = "W";
            map[6, 8] = "W";
            map[8, 8] = "W";
            map[9, 8] = "W";
            map[11, 8] = "W";
            map[13, 8] = "W";
            map[16, 8] = "W";
            map[19, 8] = "W";
            map[24, 8] = "W";
            map[25, 8] = "W";
            map[26, 8] = "W";
            map[28, 8] = "W";
            map[29, 8] = "W";
            map[30, 8] = "W";
            map[31, 8] = "W";
            map[34, 8] = "W";
            map[36, 8] = "W";
            map[38, 8] = "W";
            map[42, 8] = "W";
            map[44, 8] = "W";
            map[45, 8] = "W";
            map[46, 8] = "W";
            map[47, 8] = "W";
            map[53, 8] = "W";
            map[55, 8] = "W";
            map[59, 8] = "W";
            map[61, 8] = "W";
            map[63, 8] = "W";
            map[1, 9] = "W";
            map[5, 9] = "W";
            map[6, 9] = "W";
            map[10, 9] = "W";
            map[11, 9] = "W";
            map[16, 9] = "W";
            map[17, 9] = "W";
            map[19, 9] = "W";
            map[22, 9] = "W";
            map[25, 9] = "W";
            map[26, 9] = "W";
            map[27, 9] = "W";
            map[28, 9] = "W";
            map[31, 9] = "W";
            map[35, 9] = "W";
            map[39, 9] = "W";
            map[42, 9] = "W";
            map[43, 9] = "W";
            map[44, 9] = "W";
            map[47, 9] = "W";
            map[50, 9] = "W";
            map[51, 9] = "W";
            map[55, 9] = "W";
            map[56, 9] = "W";
            map[60, 9] = "W";
            map[61, 9] = "W";
            map[64, 9] = "W";
            map[68, 9] = "W";
            map[69, 9] = "W";
            map[0, 10] = "W";
            map[3, 10] = "W";
            map[6, 10] = "W";
            map[7, 10] = "W";
            map[9, 10] = "W";
            map[11, 10] = "W";
            map[13, 10] = "W";
            map[15, 10] = "W";
            map[17, 10] = "W";
            map[18, 10] = "W";
            map[19, 10] = "W";
            map[20, 10] = "W";
            map[23, 10] = "W";
            map[29, 10] = "W";
            map[30, 10] = "W";
            map[31, 10] = "W";
            map[33, 10] = "W";
            map[35, 10] = "W";
            map[37, 10] = "W";
            map[39, 10] = "W";
            map[41, 10] = "W";
            map[42, 10] = "W";
            map[45, 10] = "W";
            map[46, 10] = "W";
            map[47, 10] = "W";
            map[50, 10] = "W";
            map[55, 10] = "W";
            map[56, 10] = "W";
            map[58, 10] = "W";
            map[62, 10] = "W";
            map[66, 10] = "W";
            map[2, 11] = "W";
            map[3, 11] = "W";
            map[5, 11] = "W";
            map[7, 11] = "W";
            map[8, 11] = "W";
            map[14, 11] = "W";
            map[17, 11] = "W";
            map[19, 11] = "W";
            map[23, 11] = "W";
            map[25, 11] = "W";
            map[30, 11] = "W";
            map[33, 11] = "W";
            map[34, 11] = "W";
            map[36, 11] = "W";
            map[38, 11] = "W";
            map[41, 11] = "W";
            map[50, 11] = "W";
            map[52, 11] = "W";
            map[59, 11] = "W";
            map[60, 11] = "W";
            map[61, 11] = "W";
            map[62, 11] = "W";
            map[66, 11] = "W";
            map[2, 12] = "W";
            map[11, 12] = "W";
            map[12, 12] = "W";
            map[15, 12] = "W";
            map[17, 12] = "W";
            map[21, 12] = "W";
            map[24, 12] = "W";
            map[25, 12] = "W";
            map[26, 12] = "W";
            map[35, 12] = "W";
            map[37, 12] = "W";
            map[40, 12] = "W";
            map[41, 12] = "W";
            map[42, 12] = "W";
            map[43, 12] = "W";
            map[45, 12] = "W";
            map[46, 12] = "W";
            map[47, 12] = "W";
            map[48, 12] = "W";
            map[50, 12] = "W";
            map[51, 12] = "W";
            map[55, 12] = "W";
            map[56, 12] = "W";
            map[57, 12] = "W";
            map[58, 12] = "W";
            map[63, 12] = "W";
            map[64, 12] = "W";
            map[66, 12] = "W";
            map[67, 12] = "W";
            map[68, 12] = "W";
            map[0, 13] = "W";
            map[1, 13] = "W";
            map[2, 13] = "W";
            map[4, 13] = "W";
            map[6, 13] = "W";
            map[8, 13] = "W";
            map[9, 13] = "W";
            map[10, 13] = "W";
            map[13, 13] = "W";
            map[15, 13] = "W";
            map[16, 13] = "W";
            map[17, 13] = "W";
            map[18, 13] = "W";
            map[22, 13] = "W";
            map[23, 13] = "W";
            map[24, 13] = "W";
            map[26, 13] = "W";
            map[27, 13] = "W";
            map[28, 13] = "W";
            map[44, 13] = "W";
            map[46, 13] = "W";
            map[48, 13] = "W";
            map[51, 13] = "W";
            map[52, 13] = "W";
            map[55, 13] = "W";
            map[56, 13] = "W";
            map[57, 13] = "W";
            map[65, 13] = "W";
            map[66, 13] = "W";
            map[0, 14] = "W";
            map[1, 14] = "W";
            map[2, 14] = "W";
            map[5, 14] = "W";
            map[7, 14] = "W";
            map[8, 14] = "W";
            map[9, 14] = "W";
            map[10, 14] = "W";
            map[13, 14] = "W";
            map[14, 14] = "W";
            map[15, 14] = "W";
            map[18, 14] = "W";
            map[19, 14] = "W";
            map[25, 14] = "W";
            map[27, 14] = "W";
            map[28, 14] = "W";
            map[32, 14] = "W";
            map[33, 14] = "W";
            map[35, 14] = "W";
            map[38, 14] = "W";
            map[39, 14] = "W";
            map[41, 14] = "W";
            map[42, 14] = "W";
            map[43, 14] = "W";
            map[45, 14] = "W";
            map[48, 14] = "W";
            map[51, 14] = "W";
            map[59, 14] = "W";
            map[60, 14] = "W";
            map[63, 14] = "W";
            map[64, 14] = "W";
            map[68, 14] = "W";
            map[0, 15] = "W";
            map[1, 15] = "W";
            map[3, 15] = "W";
            map[5, 15] = "W";
            map[7, 15] = "W";
            map[8, 15] = "W";
            map[10, 15] = "W";
            map[14, 15] = "W";
            map[16, 15] = "W";
            map[19, 15] = "W";
            map[21, 15] = "W";
            map[22, 15] = "W";
            map[23, 15] = "W";
            map[26, 15] = "W";
            map[28, 15] = "W";
            map[31, 15] = "W";
            map[32, 15] = "W";
            map[34, 15] = "W";
            map[37, 15] = "W";
            map[39, 15] = "W";
            map[47, 15] = "W";
            map[49, 15] = "W";
            map[51, 15] = "W";
            map[53, 15] = "W";
            map[56, 15] = "W";
            map[58, 15] = "W";
            map[60, 15] = "W";
            map[62, 15] = "W";
            map[64, 15] = "W";
            map[65, 15] = "W";
            map[66, 15] = "W";
            map[7, 16] = "W";
            map[8, 16] = "W";
            map[10, 16] = "W";
            map[11, 16] = "W";
            map[12, 16] = "W";
            map[16, 16] = "W";
            map[17, 16] = "W";
            map[19, 16] = "W";
            map[22, 16] = "W";
            map[23, 16] = "W";
            map[26, 16] = "W";
            map[27, 16] = "W";
            map[31, 16] = "W";
            map[33, 16] = "W";
            map[35, 16] = "W";
            map[37, 16] = "W";
            map[40, 16] = "W";
            map[45, 16] = "W";
            map[51, 16] = "W";
            map[54, 16] = "W";
            map[63, 16] = "W";
            map[64, 16] = "W";
            map[65, 16] = "W";
            map[67, 16] = "W";
            map[0, 17] = "W";
            map[1, 17] = "W";
            map[3, 17] = "W";
            map[4, 17] = "W";
            map[6, 17] = "W";
            map[10, 17] = "W";
            map[19, 17] = "W";
            map[20, 17] = "W";
            map[25, 17] = "W";
            map[27, 17] = "W";
            map[28, 17] = "W";
            map[29, 17] = "W";
            map[37, 17] = "W";
            map[41, 17] = "W";
            map[45, 17] = "W";
            map[46, 17] = "W";
            map[47, 17] = "W";
            map[52, 17] = "W";
            map[55, 17] = "W";
            map[61, 17] = "W";
            map[62, 17] = "W";
            map[63, 17] = "W";
            map[64, 17] = "W";
            map[67, 17] = "W";
            map[68, 17] = "W";
            map[69, 17] = "W";
            map[0, 18] = "W";
            map[1, 18] = "W";
            map[2, 18] = "W";
            map[3, 18] = "W";
            map[5, 18] = "W";
            map[8, 18] = "W";
            map[16, 18] = "W";
            map[17, 18] = "W";
            map[19, 18] = "W";
            map[20, 18] = "W";
            map[31, 18] = "W";
            map[32, 18] = "W";
            map[33, 18] = "W";
            map[34, 18] = "W";
            map[35, 18] = "W";
            map[37, 18] = "W";
            map[40, 18] = "W";
            map[42, 18] = "W";
            map[45, 18] = "W";
            map[46, 18] = "W";
            map[47, 18] = "W";
            map[51, 18] = "W";
            map[52, 18] = "W";
            map[55, 18] = "W";
            map[58, 18] = "W";
            map[59, 18] = "W";
            map[60, 18] = "W";
            map[61, 18] = "W";
            map[62, 18] = "W";
            map[64, 18] = "W";
            map[67, 18] = "W";
            map[0, 19] = "W";
            map[3, 19] = "W";
            map[4, 19] = "W";
            map[5, 19] = "W";
            map[7, 19] = "W";
            map[10, 19] = "W";
            map[12, 19] = "W";
            map[15, 19] = "W";
            map[17, 19] = "W";
            map[23, 19] = "W";
            map[24, 19] = "W";
            map[25, 19] = "W";
            map[29, 19] = "W";
            map[33, 19] = "W";
            map[34, 19] = "W";
            map[37, 19] = "W";
            map[44, 19] = "W";
            map[47, 19] = "W";
            map[48, 19] = "W";
            map[51, 19] = "W";
            map[57, 19] = "W";
            map[59, 19] = "W";
            map[64, 19] = "W";
            map[65, 19] = "W";
            map[67, 19] = "W";
            map[7, 20] = "W";
            map[8, 20] = "W";
            map[10, 20] = "W";
            map[14, 20] = "W";
            map[15, 20] = "W";
            map[16, 20] = "W";
            map[21, 20] = "W";
            map[22, 20] = "W";
            map[24, 20] = "W";
            map[25, 20] = "W";
            map[26, 20] = "W";
            map[28, 20] = "W";
            map[29, 20] = "W";
            map[30, 20] = "W";
            map[33, 20] = "W";
            map[34, 20] = "W";
            map[35, 20] = "W";
            map[37, 20] = "W";
            map[39, 20] = "W";
            map[42, 20] = "W";
            map[45, 20] = "W";
            map[49, 20] = "W";
            map[50, 20] = "W";
            map[54, 20] = "W";
            map[55, 20] = "W";
            map[56, 20] = "W";
            map[57, 20] = "W";
            map[60, 20] = "W";
            map[61, 20] = "W";
            map[62, 20] = "W";
            map[64, 20] = "W";
            map[6, 21] = "W";
            map[9, 21] = "W";
            map[13, 21] = "W";
            map[14, 21] = "W";
            map[15, 21] = "W";
            map[20, 21] = "W";
            map[23, 21] = "W";
            map[27, 21] = "W";
            map[28, 21] = "W";
            map[29, 21] = "W";
            map[30, 21] = "W";
            map[31, 21] = "W";
            map[34, 21] = "W";
            map[35, 21] = "W";
            map[36, 21] = "W";
            map[37, 21] = "W";
            map[38, 21] = "W";
            map[39, 21] = "W";
            map[40, 21] = "W";
            map[42, 21] = "W";
            map[43, 21] = "W";
            map[44, 21] = "W";
            map[46, 21] = "W";
            map[49, 21] = "W";
            map[51, 21] = "W";
            map[55, 21] = "W";
            map[59, 21] = "W";
            map[65, 21] = "W";
            map[67, 21] = "W";
            map[0, 22] = "W";
            map[5, 22] = "W";
            map[6, 22] = "W";
            map[8, 22] = "W";
            map[10, 22] = "W";
            map[15, 22] = "W";
            map[19, 22] = "W";
            map[20, 22] = "W";
            map[25, 22] = "W";
            map[26, 22] = "W";
            map[30, 22] = "W";
            map[31, 22] = "W";
            map[34, 22] = "W";
            map[35, 22] = "W";
            map[37, 22] = "W";
            map[38, 22] = "W";
            map[40, 22] = "W";
            map[41, 22] = "W";
            map[42, 22] = "W";
            map[44, 22] = "W";
            map[46, 22] = "W";
            map[59, 22] = "W";
            map[60, 22] = "W";
            map[61, 22] = "W";
            map[62, 22] = "W";
            map[65, 22] = "W";
            map[66, 22] = "W";
            map[69, 22] = "W";
            map[4, 23] = "W";
            map[9, 23] = "W";
            map[10, 23] = "W";
            map[12, 23] = "W";
            map[13, 23] = "W";
            map[14, 23] = "W";
            map[17, 23] = "W";
            map[18, 23] = "W";
            map[24, 23] = "W";
            map[26, 23] = "W";
            map[28, 23] = "W";
            map[31, 23] = "W";
            map[35, 23] = "W";
            map[37, 23] = "W";
            map[40, 23] = "W";
            map[42, 23] = "W";
            map[43, 23] = "W";
            map[44, 23] = "W";
            map[46, 23] = "W";
            map[47, 23] = "W";
            map[49, 23] = "W";
            map[51, 23] = "W";
            map[55, 23] = "W";
            map[58, 23] = "W";
            map[59, 23] = "W";
            map[2, 24] = "W";
            map[11, 24] = "W";
            map[12, 24] = "W";
            map[13, 24] = "W";
            map[14, 24] = "W";
            map[17, 24] = "W";
            map[19, 24] = "W";
            map[21, 24] = "W";
            map[25, 24] = "W";
            map[26, 24] = "W";
            map[27, 24] = "W";
            map[28, 24] = "W";
            map[29, 24] = "W";
            map[30, 24] = "W";
            map[35, 24] = "W";
            map[37, 24] = "W";
            map[38, 24] = "W";
            map[39, 24] = "W";
            map[41, 24] = "W";
            map[44, 24] = "W";
            map[47, 24] = "W";
            map[49, 24] = "W";
            map[51, 24] = "W";
            map[53, 24] = "W";
            map[55, 24] = "W";
            map[62, 24] = "W";
            map[64, 24] = "W";
            map[65, 24] = "W";
            map[69, 24] = "W";
            map[1, 25] = "W";
            map[5, 25] = "W";
            map[6, 25] = "W";
            map[7, 25] = "W";
            map[13, 25] = "W";
            map[16, 25] = "W";
            map[17, 25] = "W";
            map[18, 25] = "W";
            map[19, 25] = "W";
            map[20, 25] = "W";
            map[21, 25] = "W";
            map[25, 25] = "W";
            map[27, 25] = "W";
            map[28, 25] = "W";
            map[29, 25] = "W";
            map[31, 25] = "W";
            map[33, 25] = "W";
            map[35, 25] = "W";
            map[39, 25] = "W";
            map[40, 25] = "W";
            map[43, 25] = "W";
            map[44, 25] = "W";
            map[47, 25] = "W";
            map[50, 25] = "W";
            map[54, 25] = "W";
            map[56, 25] = "W";
            map[58, 25] = "W";
            map[60, 25] = "W";
            map[61, 25] = "W";
            map[65, 25] = "W";
            map[69, 25] = "W";
            map[1, 26] = "W";
            map[4, 26] = "W";
            map[6, 26] = "W";
            map[8, 26] = "W";
            map[9, 26] = "W";
            map[12, 26] = "W";
            map[15, 26] = "W";
            map[18, 26] = "W";
            map[19, 26] = "W";
            map[23, 26] = "W";
            map[25, 26] = "W";
            map[28, 26] = "W";
            map[34, 26] = "W";
            map[35, 26] = "W";
            map[36, 26] = "W";
            map[40, 26] = "W";
            map[42, 26] = "W";
            map[53, 26] = "W";
            map[56, 26] = "W";
            map[57, 26] = "W";
            map[59, 26] = "W";
            map[62, 26] = "W";
            map[63, 26] = "W";
            map[64, 26] = "W";
            map[66, 26] = "W";
            map[4, 27] = "W";
            map[5, 27] = "W";
            map[8, 27] = "W";
            map[10, 27] = "W";
            map[14, 27] = "W";
            map[15, 27] = "W";
            map[16, 27] = "W";
            map[17, 27] = "W";
            map[18, 27] = "W";
            map[21, 27] = "W";
            map[25, 27] = "W";
            map[29, 27] = "W";
            map[30, 27] = "W";
            map[37, 27] = "W";
            map[38, 27] = "W";
            map[39, 27] = "W";
            map[43, 27] = "W";
            map[48, 27] = "W";
            map[49, 27] = "W";
            map[50, 27] = "W";
            map[54, 27] = "W";
            map[57, 27] = "W";
            map[58, 27] = "W";
            map[62, 27] = "W";
            map[63, 27] = "W";
            map[64, 27] = "W";
            map[69, 27] = "W";
            map[0, 28] = "W";
            map[1, 28] = "W";
            map[5, 28] = "W";
            map[6, 28] = "W";
            map[8, 28] = "W";
            map[10, 28] = "W";
            map[14, 28] = "W";
            map[15, 28] = "W";
            map[16, 28] = "W";
            map[19, 28] = "W";
            map[20, 28] = "W";
            map[24, 28] = "W";
            map[28, 28] = "W";
            map[29, 28] = "W";
            map[31, 28] = "W";
            map[32, 28] = "W";
            map[35, 28] = "W";
            map[40, 28] = "W";
            map[41, 28] = "W";
            map[42, 28] = "W";
            map[43, 28] = "W";
            map[45, 28] = "W";
            map[46, 28] = "W";
            map[47, 28] = "W";
            map[50, 28] = "W";
            map[52, 28] = "W";
            map[53, 28] = "W";
            map[54, 28] = "W";
            map[56, 28] = "W";
            map[57, 28] = "W";
            map[65, 28] = "W";
            map[69, 28] = "W";
            map[0, 29] = "W";
            map[8, 29] = "W";
            map[9, 29] = "W";
            map[11, 29] = "W";
            map[13, 29] = "W";
            map[14, 29] = "W";
            map[19, 29] = "W";
            map[22, 29] = "W";
            map[29, 29] = "W";
            map[30, 29] = "W";
            map[31, 29] = "W";
            map[32, 29] = "W";
            map[38, 29] = "W";
            map[39, 29] = "W";
            map[40, 29] = "W";
            map[42, 29] = "W";
            map[43, 29] = "W";
            map[44, 29] = "W";
            map[45, 29] = "W";
            map[49, 29] = "W";
            map[52, 29] = "W";
            map[54, 29] = "W";
            map[57, 29] = "W";
            map[58, 29] = "W";
            map[59, 29] = "W";
            map[60, 29] = "W";
            map[61, 29] = "W";
            map[62, 29] = "W";
            map[64, 29] = "W";
            map[65, 29] = "W";
            map[1, 30] = "W";
            map[4, 30] = "W";
            map[5, 30] = "W";
            map[9, 30] = "W";
            map[10, 30] = "W";
            map[11, 30] = "W";
            map[13, 30] = "W";
            map[15, 30] = "W";
            map[16, 30] = "W";
            map[20, 30] = "W";
            map[22, 30] = "W";
            map[23, 30] = "W";
            map[25, 30] = "W";
            map[27, 30] = "W";
            map[28, 30] = "W";
            map[29, 30] = "W";
            map[33, 30] = "W";
            map[34, 30] = "W";
            map[35, 30] = "W";
            map[39, 30] = "W";
            map[40, 30] = "W";
            map[43, 30] = "W";
            map[45, 30] = "W";
            map[46, 30] = "W";
            map[48, 30] = "W";
            map[50, 30] = "W";
            map[51, 30] = "W";
            map[52, 30] = "W";
            map[54, 30] = "W";
            map[56, 30] = "W";
            map[57, 30] = "W";
            map[59, 30] = "W";
            map[61, 30] = "W";
            map[66, 30] = "W";
            map[1, 31] = "W";
            map[8, 31] = "W";
            map[12, 31] = "W";
            map[13, 31] = "W";
            map[15, 31] = "W";
            map[17, 31] = "W";
            map[21, 31] = "W";
            map[22, 31] = "W";
            map[27, 31] = "W";
            map[30, 31] = "W";
            map[38, 31] = "W";
            map[41, 31] = "W";
            map[46, 31] = "W";
            map[47, 31] = "W";
            map[51, 31] = "W";
            map[53, 31] = "W";
            map[57, 31] = "W";
            map[58, 31] = "W";
            map[62, 31] = "W";
            map[66, 31] = "W";
            map[69, 31] = "W";
            map[1, 32] = "W";
            map[3, 32] = "W";
            map[4, 32] = "W";
            map[6, 32] = "W";
            map[8, 32] = "W";
            map[13, 32] = "W";
            map[16, 32] = "W";
            map[19, 32] = "W";
            map[21, 32] = "W";
            map[24, 32] = "W";
            map[26, 32] = "W";
            map[27, 32] = "W";
            map[28, 32] = "W";
            map[31, 32] = "W";
            map[36, 32] = "W";
            map[39, 32] = "W";
            map[40, 32] = "W";
            map[42, 32] = "W";
            map[45, 32] = "W";
            map[46, 32] = "W";
            map[47, 32] = "W";
            map[48, 32] = "W";
            map[51, 32] = "W";
            map[53, 32] = "W";
            map[54, 32] = "W";
            map[57, 32] = "W";
            map[61, 32] = "W";
            map[62, 32] = "W";
            map[64, 32] = "W";
            map[67, 32] = "W";
            map[69, 32] = "W";
            map[2, 33] = "W";
            map[4, 33] = "W";
            map[5, 33] = "W";
            map[6, 33] = "W";
            map[7, 33] = "W";
            map[9, 33] = "W";
            map[10, 33] = "W";
            map[11, 33] = "W";
            map[14, 33] = "W";
            map[18, 33] = "W";
            map[19, 33] = "W";
            map[23, 33] = "W";
            map[26, 33] = "W";
            map[28, 33] = "W";
            map[31, 33] = "W";
            map[33, 33] = "W";
            map[34, 33] = "W";
            map[36, 33] = "W";
            map[37, 33] = "W";
            map[43, 33] = "W";
            map[44, 33] = "W";
            map[48, 33] = "W";
            map[50, 33] = "W";
            map[53, 33] = "W";
            map[54, 33] = "W";
            map[58, 33] = "W";
            map[59, 33] = "W";
            map[65, 33] = "W";
            map[68, 33] = "W";
            map[0, 34] = "W";
            map[1, 34] = "W";
            map[3, 34] = "W";
            map[5, 34] = "W";
            map[9, 34] = "W";
            map[10, 34] = "W";
            map[12, 34] = "W";
            map[17, 34] = "W";
            map[18, 34] = "W";
            map[21, 34] = "W";
            map[26, 34] = "W";
            map[29, 34] = "W";
            map[30, 34] = "W";
            map[32, 34] = "W";
            map[34, 34] = "W";
            map[40, 34] = "W";
            map[42, 34] = "W";
            map[43, 34] = "W";
            map[44, 34] = "W";
            map[45, 34] = "W";
            map[48, 34] = "W";
            map[49, 34] = "W";
            map[51, 34] = "W";
            map[55, 34] = "W";
            map[58, 34] = "W";
            map[60, 34] = "W";
            map[62, 34] = "W";
            map[64, 34] = "W";
            map[65, 34] = "W";
            map[67, 34] = "W";
            map[1, 35] = "W";
            map[5, 35] = "W";
            map[11, 35] = "W";
            map[12, 35] = "W";
            map[13, 35] = "W";
            map[18, 35] = "W";
            map[19, 35] = "W";
            map[20, 35] = "W";
            map[21, 35] = "W";
            map[22, 35] = "W";
            map[25, 35] = "W";
            map[28, 35] = "W";
            map[31, 35] = "W";
            map[32, 35] = "W";
            map[33, 35] = "W";
            map[34, 35] = "W";
            map[37, 35] = "W";
            map[38, 35] = "W";
            map[40, 35] = "W";
            map[41, 35] = "W";
            map[42, 35] = "W";
            map[44, 35] = "W";
            map[49, 35] = "W";
            map[52, 35] = "W";
            map[53, 35] = "W";
            map[59, 35] = "W";
            map[61, 35] = "W";
            map[62, 35] = "W";
            map[64, 35] = "W";
            map[65, 35] = "W";
            map[67, 35] = "W";
            map[69, 35] = "W";
            map[0, 36] = "W";
            map[2, 36] = "W";
            map[4, 36] = "W";
            map[5, 36] = "W";
            map[6, 36] = "W";
            map[12, 36] = "W";
            map[13, 36] = "W";
            map[14, 36] = "W";
            map[17, 36] = "W";
            map[22, 36] = "W";
            map[29, 36] = "W";
            map[35, 36] = "W";
            map[41, 36] = "W";
            map[42, 36] = "W";
            map[43, 36] = "W";
            map[45, 36] = "W";
            map[46, 36] = "W";
            map[47, 36] = "W";
            map[49, 36] = "W";
            map[53, 36] = "W";
            map[57, 36] = "W";
            map[60, 36] = "W";
            map[62, 36] = "W";
            map[68, 36] = "W";
            map[0, 37] = "W";
            map[2, 37] = "W";
            map[4, 37] = "W";
            map[5, 37] = "W";
            map[6, 37] = "W";
            map[8, 37] = "W";
            map[12, 37] = "W";
            map[13, 37] = "W";
            map[19, 37] = "W";
            map[21, 37] = "W";
            map[24, 37] = "W";
            map[26, 37] = "W";
            map[40, 37] = "W";
            map[42, 37] = "W";
            map[50, 37] = "W";
            map[52, 37] = "W";
            map[53, 37] = "W";
            map[54, 37] = "W";
            map[62, 37] = "W";
            map[63, 37] = "W";
            map[65, 37] = "W";
            map[66, 37] = "W";
            map[68, 37] = "W";
            map[0, 38] = "W";
            map[1, 38] = "W";
            map[3, 38] = "W";
            map[5, 38] = "W";
            map[6, 38] = "W";
            map[10, 38] = "W";
            map[11, 38] = "W";
            map[13, 38] = "W";
            map[19, 38] = "W";
            map[27, 38] = "W";
            map[31, 38] = "W";
            map[33, 38] = "W";
            map[37, 38] = "W";
            map[44, 38] = "W";
            map[47, 38] = "W";
            map[48, 38] = "W";
            map[52, 38] = "W";
            map[53, 38] = "W";
            map[54, 38] = "W";
            map[58, 38] = "W";
            map[61, 38] = "W";
            map[65, 38] = "W";
            map[69, 38] = "W";
            map[1, 39] = "W";
            map[5, 39] = "W";
            map[6, 39] = "W";
            map[8, 39] = "W";
            map[9, 39] = "W";
            map[10, 39] = "W";
            map[11, 39] = "W";
            map[15, 39] = "W";
            map[17, 39] = "W";
            map[21, 39] = "W";
            map[22, 39] = "W";
            map[25, 39] = "W";
            map[30, 39] = "W";
            map[34, 39] = "W";
            map[36, 39] = "W";
            map[38, 39] = "W";
            map[40, 39] = "W";
            map[41, 39] = "W";
            map[42, 39] = "W";
            map[43, 39] = "W";
            map[44, 39] = "W";
            map[46, 39] = "W";
            map[47, 39] = "W";
            map[48, 39] = "W";
            map[51, 39] = "W";
            map[52, 39] = "W";
            map[53, 39] = "W";
            map[58, 39] = "W";
            map[64, 39] = "W";
            map[65, 39] = "W";
            return map;
        }
        #endregion

        #endregion

    }
}
