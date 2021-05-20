using Battle.Logic.PathFinding;
using Battle.Tests.Map;
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
            string[,] map = MapUtility.InitializeMap(7, 5);

            //Act
            PathResult pathResult = Path.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(pathResult);
            Assert.IsNotNull(pathResult.Path);
            Assert.IsTrue(pathResult.Path.Any());
            Assert.AreEqual(4, pathResult.Path.Count);
            Assert.AreEqual("<2, 0, 2>", pathResult.Path[0].ToString());
            Assert.AreEqual("<5, 0, 2>", pathResult.Path[3].ToString());
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
            string[,] map = MapUtility.InitializeMap(7, 5);
            map[3, 4] = "■";
            map[3, 3] = "■";
            map[3, 2] = "■";
            map[3, 1] = "■";
            map[4, 1] = "■";

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
            string[,] map = MapUtility.InitializeMap(7, 5);
            map[3, 4] = "■";
            map[3, 3] = "■";
            map[3, 2] = "■";
            map[3, 1] = "■";
            map[3, 0] = "■";

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
            string[,] map = MapUtility.InitializeMap(7, 5);
            map[0, 0] = "■";
            map[1, 4] = "■";
            map[1, 3] = "■";
            map[1, 2] = "■";
            map[1, 1] = "■";
            map[2, 4] = "■";
            map[2, 0] = "■";
            map[3, 3] = "■";
            map[3, 2] = "■";
            map[3, 1] = "■";
            map[3, 0] = "■";
            map[4, 4] = "■";
            map[4, 0] = "■";
            map[5, 4] = "■";
            map[5, 3] = "■";
            map[5, 2] = "■";
            map[5, 1] = "■";
            map[6, 0] = "■";

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
            string[,] map = MapUtility.InitializeMap(width, height);
            map[1, 1] = "■";
            map[1, 2] = "■";
            map[1, 3] = "■";
            map[2, 1] = "■";
            map[2, 3] = "■";
            map[3, 1] = "■";
            map[3, 2] = "■";
            map[3, 3] = "■";
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
            string[,] map = MapUtility.InitializeMap(width, height);
            map[1, 1] = "■";
            map[1, 2] = "■";
            map[1, 3] = "■";
            map[2, 1] = "■";
            map[2, 3] = "■";
            map[3, 1] = "■";
            map[3, 2] = "■";
            map[3, 3] = "■";

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



        [TestMethod]
        public void TileTest()
        {
            //Arrange
            Tile tile = new(3,3,"", new Vector3(6, 0, 6));

            //Act
            string result = tile.ToString();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("3, 0: Untested", result);
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



            string[,] map = MapUtility.InitializeMap(70, 40);
            map[6, 0] = "■";
            map[8, 0] = "■";
            map[10, 0] = "■";
            map[17, 0] = "■";
            map[18, 0] = "■";
            map[20, 0] = "■";
            map[23, 0] = "■";
            map[24, 0] = "■";
            map[30, 0] = "■";
            map[33, 0] = "■";
            map[34, 0] = "■";
            map[36, 0] = "■";
            map[40, 0] = "■";
            map[42, 0] = "■";
            map[44, 0] = "■";
            map[46, 0] = "■";
            map[52, 0] = "■";
            map[53, 0] = "■";
            map[58, 0] = "■";
            map[61, 0] = "■";
            map[62, 0] = "■";
            map[63, 0] = "■";
            map[67, 0] = "■";
            map[0, 1] = "■";
            map[2, 1] = "■";
            map[8, 1] = "■";
            map[9, 1] = "■";
            map[12, 1] = "■";
            map[13, 1] = "■";
            map[14, 1] = "■";
            map[15, 1] = "■";
            map[16, 1] = "■";
            map[17, 1] = "■";
            map[23, 1] = "■";
            map[36, 1] = "■";
            map[37, 1] = "■";
            map[41, 1] = "■";
            map[43, 1] = "■";
            map[45, 1] = "■";
            map[47, 1] = "■";
            map[49, 1] = "■";
            map[51, 1] = "■";
            map[52, 1] = "■";
            map[56, 1] = "■";
            map[58, 1] = "■";
            map[63, 1] = "■";
            map[66, 1] = "■";
            map[3, 2] = "■";
            map[5, 2] = "■";
            map[7, 2] = "■";
            map[8, 2] = "■";
            map[10, 2] = "■";
            map[14, 2] = "■";
            map[15, 2] = "■";
            map[18, 2] = "■";
            map[22, 2] = "■";
            map[24, 2] = "■";
            map[29, 2] = "■";
            map[30, 2] = "■";
            map[32, 2] = "■";
            map[33, 2] = "■";
            map[34, 2] = "■";
            map[37, 2] = "■";
            map[39, 2] = "■";
            map[47, 2] = "■";
            map[48, 2] = "■";
            map[51, 2] = "■";
            map[52, 2] = "■";
            map[54, 2] = "■";
            map[57, 2] = "■";
            map[59, 2] = "■";
            map[61, 2] = "■";
            map[63, 2] = "■";
            map[65, 2] = "■";
            map[4, 3] = "■";
            map[11, 3] = "■";
            map[13, 3] = "■";
            map[21, 3] = "■";
            map[22, 3] = "■";
            map[26, 3] = "■";
            map[30, 3] = "■";
            map[31, 3] = "■";
            map[34, 3] = "■";
            map[41, 3] = "■";
            map[42, 3] = "■";
            map[44, 3] = "■";
            map[51, 3] = "■";
            map[55, 3] = "■";
            map[57, 3] = "■";
            map[63, 3] = "■";
            map[0, 4] = "■";
            map[1, 4] = "■";
            map[4, 4] = "■";
            map[9, 4] = "■";
            map[12, 4] = "■";
            map[13, 4] = "■";
            map[18, 4] = "■";
            map[19, 4] = "■";
            map[20, 4] = "■";
            map[24, 4] = "■";
            map[25, 4] = "■";
            map[26, 4] = "■";
            map[27, 4] = "■";
            map[28, 4] = "■";
            map[36, 4] = "■";
            map[37, 4] = "■";
            map[40, 4] = "■";
            map[41, 4] = "■";
            map[44, 4] = "■";
            map[50, 4] = "■";
            map[56, 4] = "■";
            map[57, 4] = "■";
            map[59, 4] = "■";
            map[62, 4] = "■";
            map[64, 4] = "■";
            map[65, 4] = "■";
            map[66, 4] = "■";
            map[67, 4] = "■";
            map[69, 4] = "■";
            map[1, 5] = "■";
            map[2, 5] = "■";
            map[5, 5] = "■";
            map[9, 5] = "■";
            map[15, 5] = "■";
            map[18, 5] = "■";
            map[25, 5] = "■";
            map[28, 5] = "■";
            map[31, 5] = "■";
            map[32, 5] = "■";
            map[35, 5] = "■";
            map[36, 5] = "■";
            map[37, 5] = "■";
            map[43, 5] = "■";
            map[48, 5] = "■";
            map[49, 5] = "■";
            map[50, 5] = "■";
            map[51, 5] = "■";
            map[52, 5] = "■";
            map[60, 5] = "■";
            map[62, 5] = "■";
            map[69, 5] = "■";
            map[2, 6] = "■";
            map[5, 6] = "■";
            map[6, 6] = "■";
            map[9, 6] = "■";
            map[11, 6] = "■";
            map[13, 6] = "■";
            map[15, 6] = "■";
            map[16, 6] = "■";
            map[18, 6] = "■";
            map[23, 6] = "■";
            map[24, 6] = "■";
            map[26, 6] = "■";
            map[27, 6] = "■";
            map[28, 6] = "■";
            map[29, 6] = "■";
            map[33, 6] = "■";
            map[34, 6] = "■";
            map[36, 6] = "■";
            map[37, 6] = "■";
            map[44, 6] = "■";
            map[45, 6] = "■";
            map[47, 6] = "■";
            map[50, 6] = "■";
            map[55, 6] = "■";
            map[59, 6] = "■";
            map[61, 6] = "■";
            map[62, 6] = "■";
            map[65, 6] = "■";
            map[66, 6] = "■";
            map[68, 6] = "■";
            map[69, 6] = "■";
            map[0, 7] = "■";
            map[4, 7] = "■";
            map[7, 7] = "■";
            map[9, 7] = "■";
            map[11, 7] = "■";
            map[12, 7] = "■";
            map[21, 7] = "■";
            map[34, 7] = "■";
            map[35, 7] = "■";
            map[37, 7] = "■";
            map[47, 7] = "■";
            map[48, 7] = "■";
            map[50, 7] = "■";
            map[51, 7] = "■";
            map[53, 7] = "■";
            map[57, 7] = "■";
            map[62, 7] = "■";
            map[63, 7] = "■";
            map[64, 7] = "■";
            map[66, 7] = "■";
            map[69, 7] = "■";
            map[2, 8] = "■";
            map[4, 8] = "■";
            map[5, 8] = "■";
            map[6, 8] = "■";
            map[8, 8] = "■";
            map[9, 8] = "■";
            map[11, 8] = "■";
            map[13, 8] = "■";
            map[16, 8] = "■";
            map[19, 8] = "■";
            map[24, 8] = "■";
            map[25, 8] = "■";
            map[26, 8] = "■";
            map[28, 8] = "■";
            map[29, 8] = "■";
            map[30, 8] = "■";
            map[31, 8] = "■";
            map[34, 8] = "■";
            map[36, 8] = "■";
            map[38, 8] = "■";
            map[42, 8] = "■";
            map[44, 8] = "■";
            map[45, 8] = "■";
            map[46, 8] = "■";
            map[47, 8] = "■";
            map[53, 8] = "■";
            map[55, 8] = "■";
            map[59, 8] = "■";
            map[61, 8] = "■";
            map[63, 8] = "■";
            map[1, 9] = "■";
            map[5, 9] = "■";
            map[6, 9] = "■";
            map[10, 9] = "■";
            map[11, 9] = "■";
            map[16, 9] = "■";
            map[17, 9] = "■";
            map[19, 9] = "■";
            map[22, 9] = "■";
            map[25, 9] = "■";
            map[26, 9] = "■";
            map[27, 9] = "■";
            map[28, 9] = "■";
            map[31, 9] = "■";
            map[35, 9] = "■";
            map[39, 9] = "■";
            map[42, 9] = "■";
            map[43, 9] = "■";
            map[44, 9] = "■";
            map[47, 9] = "■";
            map[50, 9] = "■";
            map[51, 9] = "■";
            map[55, 9] = "■";
            map[56, 9] = "■";
            map[60, 9] = "■";
            map[61, 9] = "■";
            map[64, 9] = "■";
            map[68, 9] = "■";
            map[69, 9] = "■";
            map[0, 10] = "■";
            map[3, 10] = "■";
            map[6, 10] = "■";
            map[7, 10] = "■";
            map[9, 10] = "■";
            map[11, 10] = "■";
            map[13, 10] = "■";
            map[15, 10] = "■";
            map[17, 10] = "■";
            map[18, 10] = "■";
            map[19, 10] = "■";
            map[20, 10] = "■";
            map[23, 10] = "■";
            map[29, 10] = "■";
            map[30, 10] = "■";
            map[31, 10] = "■";
            map[33, 10] = "■";
            map[35, 10] = "■";
            map[37, 10] = "■";
            map[39, 10] = "■";
            map[41, 10] = "■";
            map[42, 10] = "■";
            map[45, 10] = "■";
            map[46, 10] = "■";
            map[47, 10] = "■";
            map[50, 10] = "■";
            map[55, 10] = "■";
            map[56, 10] = "■";
            map[58, 10] = "■";
            map[62, 10] = "■";
            map[66, 10] = "■";
            map[2, 11] = "■";
            map[3, 11] = "■";
            map[5, 11] = "■";
            map[7, 11] = "■";
            map[8, 11] = "■";
            map[14, 11] = "■";
            map[17, 11] = "■";
            map[19, 11] = "■";
            map[23, 11] = "■";
            map[25, 11] = "■";
            map[30, 11] = "■";
            map[33, 11] = "■";
            map[34, 11] = "■";
            map[36, 11] = "■";
            map[38, 11] = "■";
            map[41, 11] = "■";
            map[50, 11] = "■";
            map[52, 11] = "■";
            map[59, 11] = "■";
            map[60, 11] = "■";
            map[61, 11] = "■";
            map[62, 11] = "■";
            map[66, 11] = "■";
            map[2, 12] = "■";
            map[11, 12] = "■";
            map[12, 12] = "■";
            map[15, 12] = "■";
            map[17, 12] = "■";
            map[21, 12] = "■";
            map[24, 12] = "■";
            map[25, 12] = "■";
            map[26, 12] = "■";
            map[35, 12] = "■";
            map[37, 12] = "■";
            map[40, 12] = "■";
            map[41, 12] = "■";
            map[42, 12] = "■";
            map[43, 12] = "■";
            map[45, 12] = "■";
            map[46, 12] = "■";
            map[47, 12] = "■";
            map[48, 12] = "■";
            map[50, 12] = "■";
            map[51, 12] = "■";
            map[55, 12] = "■";
            map[56, 12] = "■";
            map[57, 12] = "■";
            map[58, 12] = "■";
            map[63, 12] = "■";
            map[64, 12] = "■";
            map[66, 12] = "■";
            map[67, 12] = "■";
            map[68, 12] = "■";
            map[0, 13] = "■";
            map[1, 13] = "■";
            map[2, 13] = "■";
            map[4, 13] = "■";
            map[6, 13] = "■";
            map[8, 13] = "■";
            map[9, 13] = "■";
            map[10, 13] = "■";
            map[13, 13] = "■";
            map[15, 13] = "■";
            map[16, 13] = "■";
            map[17, 13] = "■";
            map[18, 13] = "■";
            map[22, 13] = "■";
            map[23, 13] = "■";
            map[24, 13] = "■";
            map[26, 13] = "■";
            map[27, 13] = "■";
            map[28, 13] = "■";
            map[44, 13] = "■";
            map[46, 13] = "■";
            map[48, 13] = "■";
            map[51, 13] = "■";
            map[52, 13] = "■";
            map[55, 13] = "■";
            map[56, 13] = "■";
            map[57, 13] = "■";
            map[65, 13] = "■";
            map[66, 13] = "■";
            map[0, 14] = "■";
            map[1, 14] = "■";
            map[2, 14] = "■";
            map[5, 14] = "■";
            map[7, 14] = "■";
            map[8, 14] = "■";
            map[9, 14] = "■";
            map[10, 14] = "■";
            map[13, 14] = "■";
            map[14, 14] = "■";
            map[15, 14] = "■";
            map[18, 14] = "■";
            map[19, 14] = "■";
            map[25, 14] = "■";
            map[27, 14] = "■";
            map[28, 14] = "■";
            map[32, 14] = "■";
            map[33, 14] = "■";
            map[35, 14] = "■";
            map[38, 14] = "■";
            map[39, 14] = "■";
            map[41, 14] = "■";
            map[42, 14] = "■";
            map[43, 14] = "■";
            map[45, 14] = "■";
            map[48, 14] = "■";
            map[51, 14] = "■";
            map[59, 14] = "■";
            map[60, 14] = "■";
            map[63, 14] = "■";
            map[64, 14] = "■";
            map[68, 14] = "■";
            map[0, 15] = "■";
            map[1, 15] = "■";
            map[3, 15] = "■";
            map[5, 15] = "■";
            map[7, 15] = "■";
            map[8, 15] = "■";
            map[10, 15] = "■";
            map[14, 15] = "■";
            map[16, 15] = "■";
            map[19, 15] = "■";
            map[21, 15] = "■";
            map[22, 15] = "■";
            map[23, 15] = "■";
            map[26, 15] = "■";
            map[28, 15] = "■";
            map[31, 15] = "■";
            map[32, 15] = "■";
            map[34, 15] = "■";
            map[37, 15] = "■";
            map[39, 15] = "■";
            map[47, 15] = "■";
            map[49, 15] = "■";
            map[51, 15] = "■";
            map[53, 15] = "■";
            map[56, 15] = "■";
            map[58, 15] = "■";
            map[60, 15] = "■";
            map[62, 15] = "■";
            map[64, 15] = "■";
            map[65, 15] = "■";
            map[66, 15] = "■";
            map[7, 16] = "■";
            map[8, 16] = "■";
            map[10, 16] = "■";
            map[11, 16] = "■";
            map[12, 16] = "■";
            map[16, 16] = "■";
            map[17, 16] = "■";
            map[19, 16] = "■";
            map[22, 16] = "■";
            map[23, 16] = "■";
            map[26, 16] = "■";
            map[27, 16] = "■";
            map[31, 16] = "■";
            map[33, 16] = "■";
            map[35, 16] = "■";
            map[37, 16] = "■";
            map[40, 16] = "■";
            map[45, 16] = "■";
            map[51, 16] = "■";
            map[54, 16] = "■";
            map[63, 16] = "■";
            map[64, 16] = "■";
            map[65, 16] = "■";
            map[67, 16] = "■";
            map[0, 17] = "■";
            map[1, 17] = "■";
            map[3, 17] = "■";
            map[4, 17] = "■";
            map[6, 17] = "■";
            map[10, 17] = "■";
            map[19, 17] = "■";
            map[20, 17] = "■";
            map[25, 17] = "■";
            map[27, 17] = "■";
            map[28, 17] = "■";
            map[29, 17] = "■";
            map[37, 17] = "■";
            map[41, 17] = "■";
            map[45, 17] = "■";
            map[46, 17] = "■";
            map[47, 17] = "■";
            map[52, 17] = "■";
            map[55, 17] = "■";
            map[61, 17] = "■";
            map[62, 17] = "■";
            map[63, 17] = "■";
            map[64, 17] = "■";
            map[67, 17] = "■";
            map[68, 17] = "■";
            map[69, 17] = "■";
            map[0, 18] = "■";
            map[1, 18] = "■";
            map[2, 18] = "■";
            map[3, 18] = "■";
            map[5, 18] = "■";
            map[8, 18] = "■";
            map[16, 18] = "■";
            map[17, 18] = "■";
            map[19, 18] = "■";
            map[20, 18] = "■";
            map[31, 18] = "■";
            map[32, 18] = "■";
            map[33, 18] = "■";
            map[34, 18] = "■";
            map[35, 18] = "■";
            map[37, 18] = "■";
            map[40, 18] = "■";
            map[42, 18] = "■";
            map[45, 18] = "■";
            map[46, 18] = "■";
            map[47, 18] = "■";
            map[51, 18] = "■";
            map[52, 18] = "■";
            map[55, 18] = "■";
            map[58, 18] = "■";
            map[59, 18] = "■";
            map[60, 18] = "■";
            map[61, 18] = "■";
            map[62, 18] = "■";
            map[64, 18] = "■";
            map[67, 18] = "■";
            map[0, 19] = "■";
            map[3, 19] = "■";
            map[4, 19] = "■";
            map[5, 19] = "■";
            map[7, 19] = "■";
            map[10, 19] = "■";
            map[12, 19] = "■";
            map[15, 19] = "■";
            map[17, 19] = "■";
            map[23, 19] = "■";
            map[24, 19] = "■";
            map[25, 19] = "■";
            map[29, 19] = "■";
            map[33, 19] = "■";
            map[34, 19] = "■";
            map[37, 19] = "■";
            map[44, 19] = "■";
            map[47, 19] = "■";
            map[48, 19] = "■";
            map[51, 19] = "■";
            map[57, 19] = "■";
            map[59, 19] = "■";
            map[64, 19] = "■";
            map[65, 19] = "■";
            map[67, 19] = "■";
            map[7, 20] = "■";
            map[8, 20] = "■";
            map[10, 20] = "■";
            map[14, 20] = "■";
            map[15, 20] = "■";
            map[16, 20] = "■";
            map[21, 20] = "■";
            map[22, 20] = "■";
            map[24, 20] = "■";
            map[25, 20] = "■";
            map[26, 20] = "■";
            map[28, 20] = "■";
            map[29, 20] = "■";
            map[30, 20] = "■";
            map[33, 20] = "■";
            map[34, 20] = "■";
            map[35, 20] = "■";
            map[37, 20] = "■";
            map[39, 20] = "■";
            map[42, 20] = "■";
            map[45, 20] = "■";
            map[49, 20] = "■";
            map[50, 20] = "■";
            map[54, 20] = "■";
            map[55, 20] = "■";
            map[56, 20] = "■";
            map[57, 20] = "■";
            map[60, 20] = "■";
            map[61, 20] = "■";
            map[62, 20] = "■";
            map[64, 20] = "■";
            map[6, 21] = "■";
            map[9, 21] = "■";
            map[13, 21] = "■";
            map[14, 21] = "■";
            map[15, 21] = "■";
            map[20, 21] = "■";
            map[23, 21] = "■";
            map[27, 21] = "■";
            map[28, 21] = "■";
            map[29, 21] = "■";
            map[30, 21] = "■";
            map[31, 21] = "■";
            map[34, 21] = "■";
            map[35, 21] = "■";
            map[36, 21] = "■";
            map[37, 21] = "■";
            map[38, 21] = "■";
            map[39, 21] = "■";
            map[40, 21] = "■";
            map[42, 21] = "■";
            map[43, 21] = "■";
            map[44, 21] = "■";
            map[46, 21] = "■";
            map[49, 21] = "■";
            map[51, 21] = "■";
            map[55, 21] = "■";
            map[59, 21] = "■";
            map[65, 21] = "■";
            map[67, 21] = "■";
            map[0, 22] = "■";
            map[5, 22] = "■";
            map[6, 22] = "■";
            map[8, 22] = "■";
            map[10, 22] = "■";
            map[15, 22] = "■";
            map[19, 22] = "■";
            map[20, 22] = "■";
            map[25, 22] = "■";
            map[26, 22] = "■";
            map[30, 22] = "■";
            map[31, 22] = "■";
            map[34, 22] = "■";
            map[35, 22] = "■";
            map[37, 22] = "■";
            map[38, 22] = "■";
            map[40, 22] = "■";
            map[41, 22] = "■";
            map[42, 22] = "■";
            map[44, 22] = "■";
            map[46, 22] = "■";
            map[59, 22] = "■";
            map[60, 22] = "■";
            map[61, 22] = "■";
            map[62, 22] = "■";
            map[65, 22] = "■";
            map[66, 22] = "■";
            map[69, 22] = "■";
            map[4, 23] = "■";
            map[9, 23] = "■";
            map[10, 23] = "■";
            map[12, 23] = "■";
            map[13, 23] = "■";
            map[14, 23] = "■";
            map[17, 23] = "■";
            map[18, 23] = "■";
            map[24, 23] = "■";
            map[26, 23] = "■";
            map[28, 23] = "■";
            map[31, 23] = "■";
            map[35, 23] = "■";
            map[37, 23] = "■";
            map[40, 23] = "■";
            map[42, 23] = "■";
            map[43, 23] = "■";
            map[44, 23] = "■";
            map[46, 23] = "■";
            map[47, 23] = "■";
            map[49, 23] = "■";
            map[51, 23] = "■";
            map[55, 23] = "■";
            map[58, 23] = "■";
            map[59, 23] = "■";
            map[2, 24] = "■";
            map[11, 24] = "■";
            map[12, 24] = "■";
            map[13, 24] = "■";
            map[14, 24] = "■";
            map[17, 24] = "■";
            map[19, 24] = "■";
            map[21, 24] = "■";
            map[25, 24] = "■";
            map[26, 24] = "■";
            map[27, 24] = "■";
            map[28, 24] = "■";
            map[29, 24] = "■";
            map[30, 24] = "■";
            map[35, 24] = "■";
            map[37, 24] = "■";
            map[38, 24] = "■";
            map[39, 24] = "■";
            map[41, 24] = "■";
            map[44, 24] = "■";
            map[47, 24] = "■";
            map[49, 24] = "■";
            map[51, 24] = "■";
            map[53, 24] = "■";
            map[55, 24] = "■";
            map[62, 24] = "■";
            map[64, 24] = "■";
            map[65, 24] = "■";
            map[69, 24] = "■";
            map[1, 25] = "■";
            map[5, 25] = "■";
            map[6, 25] = "■";
            map[7, 25] = "■";
            map[13, 25] = "■";
            map[16, 25] = "■";
            map[17, 25] = "■";
            map[18, 25] = "■";
            map[19, 25] = "■";
            map[20, 25] = "■";
            map[21, 25] = "■";
            map[25, 25] = "■";
            map[27, 25] = "■";
            map[28, 25] = "■";
            map[29, 25] = "■";
            map[31, 25] = "■";
            map[33, 25] = "■";
            map[35, 25] = "■";
            map[39, 25] = "■";
            map[40, 25] = "■";
            map[43, 25] = "■";
            map[44, 25] = "■";
            map[47, 25] = "■";
            map[50, 25] = "■";
            map[54, 25] = "■";
            map[56, 25] = "■";
            map[58, 25] = "■";
            map[60, 25] = "■";
            map[61, 25] = "■";
            map[65, 25] = "■";
            map[69, 25] = "■";
            map[1, 26] = "■";
            map[4, 26] = "■";
            map[6, 26] = "■";
            map[8, 26] = "■";
            map[9, 26] = "■";
            map[12, 26] = "■";
            map[15, 26] = "■";
            map[18, 26] = "■";
            map[19, 26] = "■";
            map[23, 26] = "■";
            map[25, 26] = "■";
            map[28, 26] = "■";
            map[34, 26] = "■";
            map[35, 26] = "■";
            map[36, 26] = "■";
            map[40, 26] = "■";
            map[42, 26] = "■";
            map[53, 26] = "■";
            map[56, 26] = "■";
            map[57, 26] = "■";
            map[59, 26] = "■";
            map[62, 26] = "■";
            map[63, 26] = "■";
            map[64, 26] = "■";
            map[66, 26] = "■";
            map[4, 27] = "■";
            map[5, 27] = "■";
            map[8, 27] = "■";
            map[10, 27] = "■";
            map[14, 27] = "■";
            map[15, 27] = "■";
            map[16, 27] = "■";
            map[17, 27] = "■";
            map[18, 27] = "■";
            map[21, 27] = "■";
            map[25, 27] = "■";
            map[29, 27] = "■";
            map[30, 27] = "■";
            map[37, 27] = "■";
            map[38, 27] = "■";
            map[39, 27] = "■";
            map[43, 27] = "■";
            map[48, 27] = "■";
            map[49, 27] = "■";
            map[50, 27] = "■";
            map[54, 27] = "■";
            map[57, 27] = "■";
            map[58, 27] = "■";
            map[62, 27] = "■";
            map[63, 27] = "■";
            map[64, 27] = "■";
            map[69, 27] = "■";
            map[0, 28] = "■";
            map[1, 28] = "■";
            map[5, 28] = "■";
            map[6, 28] = "■";
            map[8, 28] = "■";
            map[10, 28] = "■";
            map[14, 28] = "■";
            map[15, 28] = "■";
            map[16, 28] = "■";
            map[19, 28] = "■";
            map[20, 28] = "■";
            map[24, 28] = "■";
            map[28, 28] = "■";
            map[29, 28] = "■";
            map[31, 28] = "■";
            map[32, 28] = "■";
            map[35, 28] = "■";
            map[40, 28] = "■";
            map[41, 28] = "■";
            map[42, 28] = "■";
            map[43, 28] = "■";
            map[45, 28] = "■";
            map[46, 28] = "■";
            map[47, 28] = "■";
            map[50, 28] = "■";
            map[52, 28] = "■";
            map[53, 28] = "■";
            map[54, 28] = "■";
            map[56, 28] = "■";
            map[57, 28] = "■";
            map[65, 28] = "■";
            map[69, 28] = "■";
            map[0, 29] = "■";
            map[8, 29] = "■";
            map[9, 29] = "■";
            map[11, 29] = "■";
            map[13, 29] = "■";
            map[14, 29] = "■";
            map[19, 29] = "■";
            map[22, 29] = "■";
            map[29, 29] = "■";
            map[30, 29] = "■";
            map[31, 29] = "■";
            map[32, 29] = "■";
            map[38, 29] = "■";
            map[39, 29] = "■";
            map[40, 29] = "■";
            map[42, 29] = "■";
            map[43, 29] = "■";
            map[44, 29] = "■";
            map[45, 29] = "■";
            map[49, 29] = "■";
            map[52, 29] = "■";
            map[54, 29] = "■";
            map[57, 29] = "■";
            map[58, 29] = "■";
            map[59, 29] = "■";
            map[60, 29] = "■";
            map[61, 29] = "■";
            map[62, 29] = "■";
            map[64, 29] = "■";
            map[65, 29] = "■";
            map[1, 30] = "■";
            map[4, 30] = "■";
            map[5, 30] = "■";
            map[9, 30] = "■";
            map[10, 30] = "■";
            map[11, 30] = "■";
            map[13, 30] = "■";
            map[15, 30] = "■";
            map[16, 30] = "■";
            map[20, 30] = "■";
            map[22, 30] = "■";
            map[23, 30] = "■";
            map[25, 30] = "■";
            map[27, 30] = "■";
            map[28, 30] = "■";
            map[29, 30] = "■";
            map[33, 30] = "■";
            map[34, 30] = "■";
            map[35, 30] = "■";
            map[39, 30] = "■";
            map[40, 30] = "■";
            map[43, 30] = "■";
            map[45, 30] = "■";
            map[46, 30] = "■";
            map[48, 30] = "■";
            map[50, 30] = "■";
            map[51, 30] = "■";
            map[52, 30] = "■";
            map[54, 30] = "■";
            map[56, 30] = "■";
            map[57, 30] = "■";
            map[59, 30] = "■";
            map[61, 30] = "■";
            map[66, 30] = "■";
            map[1, 31] = "■";
            map[8, 31] = "■";
            map[12, 31] = "■";
            map[13, 31] = "■";
            map[15, 31] = "■";
            map[17, 31] = "■";
            map[21, 31] = "■";
            map[22, 31] = "■";
            map[27, 31] = "■";
            map[30, 31] = "■";
            map[38, 31] = "■";
            map[41, 31] = "■";
            map[46, 31] = "■";
            map[47, 31] = "■";
            map[51, 31] = "■";
            map[53, 31] = "■";
            map[57, 31] = "■";
            map[58, 31] = "■";
            map[62, 31] = "■";
            map[66, 31] = "■";
            map[69, 31] = "■";
            map[1, 32] = "■";
            map[3, 32] = "■";
            map[4, 32] = "■";
            map[6, 32] = "■";
            map[8, 32] = "■";
            map[13, 32] = "■";
            map[16, 32] = "■";
            map[19, 32] = "■";
            map[21, 32] = "■";
            map[24, 32] = "■";
            map[26, 32] = "■";
            map[27, 32] = "■";
            map[28, 32] = "■";
            map[31, 32] = "■";
            map[36, 32] = "■";
            map[39, 32] = "■";
            map[40, 32] = "■";
            map[42, 32] = "■";
            map[45, 32] = "■";
            map[46, 32] = "■";
            map[47, 32] = "■";
            map[48, 32] = "■";
            map[51, 32] = "■";
            map[53, 32] = "■";
            map[54, 32] = "■";
            map[57, 32] = "■";
            map[61, 32] = "■";
            map[62, 32] = "■";
            map[64, 32] = "■";
            map[67, 32] = "■";
            map[69, 32] = "■";
            map[2, 33] = "■";
            map[4, 33] = "■";
            map[5, 33] = "■";
            map[6, 33] = "■";
            map[7, 33] = "■";
            map[9, 33] = "■";
            map[10, 33] = "■";
            map[11, 33] = "■";
            map[14, 33] = "■";
            map[18, 33] = "■";
            map[19, 33] = "■";
            map[23, 33] = "■";
            map[26, 33] = "■";
            map[28, 33] = "■";
            map[31, 33] = "■";
            map[33, 33] = "■";
            map[34, 33] = "■";
            map[36, 33] = "■";
            map[37, 33] = "■";
            map[43, 33] = "■";
            map[44, 33] = "■";
            map[48, 33] = "■";
            map[50, 33] = "■";
            map[53, 33] = "■";
            map[54, 33] = "■";
            map[58, 33] = "■";
            map[59, 33] = "■";
            map[65, 33] = "■";
            map[68, 33] = "■";
            map[0, 34] = "■";
            map[1, 34] = "■";
            map[3, 34] = "■";
            map[5, 34] = "■";
            map[9, 34] = "■";
            map[10, 34] = "■";
            map[12, 34] = "■";
            map[17, 34] = "■";
            map[18, 34] = "■";
            map[21, 34] = "■";
            map[26, 34] = "■";
            map[29, 34] = "■";
            map[30, 34] = "■";
            map[32, 34] = "■";
            map[34, 34] = "■";
            map[40, 34] = "■";
            map[42, 34] = "■";
            map[43, 34] = "■";
            map[44, 34] = "■";
            map[45, 34] = "■";
            map[48, 34] = "■";
            map[49, 34] = "■";
            map[51, 34] = "■";
            map[55, 34] = "■";
            map[58, 34] = "■";
            map[60, 34] = "■";
            map[62, 34] = "■";
            map[64, 34] = "■";
            map[65, 34] = "■";
            map[67, 34] = "■";
            map[1, 35] = "■";
            map[5, 35] = "■";
            map[11, 35] = "■";
            map[12, 35] = "■";
            map[13, 35] = "■";
            map[18, 35] = "■";
            map[19, 35] = "■";
            map[20, 35] = "■";
            map[21, 35] = "■";
            map[22, 35] = "■";
            map[25, 35] = "■";
            map[28, 35] = "■";
            map[31, 35] = "■";
            map[32, 35] = "■";
            map[33, 35] = "■";
            map[34, 35] = "■";
            map[37, 35] = "■";
            map[38, 35] = "■";
            map[40, 35] = "■";
            map[41, 35] = "■";
            map[42, 35] = "■";
            map[44, 35] = "■";
            map[49, 35] = "■";
            map[52, 35] = "■";
            map[53, 35] = "■";
            map[59, 35] = "■";
            map[61, 35] = "■";
            map[62, 35] = "■";
            map[64, 35] = "■";
            map[65, 35] = "■";
            map[67, 35] = "■";
            map[69, 35] = "■";
            map[0, 36] = "■";
            map[2, 36] = "■";
            map[4, 36] = "■";
            map[5, 36] = "■";
            map[6, 36] = "■";
            map[12, 36] = "■";
            map[13, 36] = "■";
            map[14, 36] = "■";
            map[17, 36] = "■";
            map[22, 36] = "■";
            map[29, 36] = "■";
            map[35, 36] = "■";
            map[41, 36] = "■";
            map[42, 36] = "■";
            map[43, 36] = "■";
            map[45, 36] = "■";
            map[46, 36] = "■";
            map[47, 36] = "■";
            map[49, 36] = "■";
            map[53, 36] = "■";
            map[57, 36] = "■";
            map[60, 36] = "■";
            map[62, 36] = "■";
            map[68, 36] = "■";
            map[0, 37] = "■";
            map[2, 37] = "■";
            map[4, 37] = "■";
            map[5, 37] = "■";
            map[6, 37] = "■";
            map[8, 37] = "■";
            map[12, 37] = "■";
            map[13, 37] = "■";
            map[19, 37] = "■";
            map[21, 37] = "■";
            map[24, 37] = "■";
            map[26, 37] = "■";
            map[40, 37] = "■";
            map[42, 37] = "■";
            map[50, 37] = "■";
            map[52, 37] = "■";
            map[53, 37] = "■";
            map[54, 37] = "■";
            map[62, 37] = "■";
            map[63, 37] = "■";
            map[65, 37] = "■";
            map[66, 37] = "■";
            map[68, 37] = "■";
            map[0, 38] = "■";
            map[1, 38] = "■";
            map[3, 38] = "■";
            map[5, 38] = "■";
            map[6, 38] = "■";
            map[10, 38] = "■";
            map[11, 38] = "■";
            map[13, 38] = "■";
            map[19, 38] = "■";
            map[27, 38] = "■";
            map[31, 38] = "■";
            map[33, 38] = "■";
            map[37, 38] = "■";
            map[44, 38] = "■";
            map[47, 38] = "■";
            map[48, 38] = "■";
            map[52, 38] = "■";
            map[53, 38] = "■";
            map[54, 38] = "■";
            map[58, 38] = "■";
            map[61, 38] = "■";
            map[65, 38] = "■";
            map[69, 38] = "■";
            map[1, 39] = "■";
            map[5, 39] = "■";
            map[6, 39] = "■";
            map[8, 39] = "■";
            map[9, 39] = "■";
            map[10, 39] = "■";
            map[11, 39] = "■";
            map[15, 39] = "■";
            map[17, 39] = "■";
            map[21, 39] = "■";
            map[22, 39] = "■";
            map[25, 39] = "■";
            map[30, 39] = "■";
            map[34, 39] = "■";
            map[36, 39] = "■";
            map[38, 39] = "■";
            map[40, 39] = "■";
            map[41, 39] = "■";
            map[42, 39] = "■";
            map[43, 39] = "■";
            map[44, 39] = "■";
            map[46, 39] = "■";
            map[47, 39] = "■";
            map[48, 39] = "■";
            map[51, 39] = "■";
            map[52, 39] = "■";
            map[53, 39] = "■";
            map[58, 39] = "■";
            map[64, 39] = "■";
            map[65, 39] = "■";
            return map;
        }
        #endregion

        #endregion

    }
}
