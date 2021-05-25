using Battle.Logic.Characters;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class PathFindingTests
    {

        [TestMethod]
        public void Test_WithoutWalls_CanFindPath()
        {
            //Arrange
            Vector3 startLocation = new(1, 0, 2);
            Vector3 endLocation = new(5, 0, 2);
            string[,] map = MapUtility.InitializeMap(7, 5);

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.AreEqual(4, PathFindingResult.Path.Count);
            Assert.AreEqual("<2, 0, 2>", PathFindingResult.Path[0].ToString());
            Assert.AreEqual("<5, 0, 2>", PathFindingResult.Path[3].ToString());
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
            Vector3 startLocation = new(1, 0, 2);
            Vector3 endLocation = new(5, 0, 2);
            string[,] map = MapUtility.InitializeMap(7, 5);
            map[3, 4] = CoverType.FullCover;
            map[3, 3] = CoverType.FullCover;
            map[3, 2] = CoverType.FullCover;
            map[3, 1] = CoverType.FullCover;
            map[4, 1] = CoverType.FullCover;

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.IsTrue(PathFindingResult.GetLastTile() != null);
            Assert.AreEqual(7, PathFindingResult.GetLastTile().TraversalCost);
            Assert.AreEqual(5, PathFindingResult.Path.Count);
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
            map[3, 4] = CoverType.FullCover;
            map[3, 3] = CoverType.FullCover;
            map[3, 2] = CoverType.FullCover;
            map[3, 1] = CoverType.FullCover;
            map[3, 0] = CoverType.FullCover;

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsFalse(PathFindingResult.Path.Any());
            Assert.IsFalse(PathFindingResult.Tiles.Any());
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
            map[0, 0] = CoverType.FullCover;
            map[1, 4] = CoverType.FullCover;
            map[1, 3] = CoverType.FullCover;
            map[1, 2] = CoverType.FullCover;
            map[1, 1] = CoverType.FullCover;
            map[2, 4] = CoverType.FullCover;
            map[2, 0] = CoverType.FullCover;
            map[3, 3] = CoverType.FullCover;
            map[3, 2] = CoverType.FullCover;
            map[3, 1] = CoverType.FullCover;
            map[3, 0] = CoverType.FullCover;
            map[4, 4] = CoverType.FullCover;
            map[4, 0] = CoverType.FullCover;
            map[5, 4] = CoverType.FullCover;
            map[5, 3] = CoverType.FullCover;
            map[5, 2] = CoverType.FullCover;
            map[5, 1] = CoverType.FullCover;
            map[6, 0] = CoverType.FullCover;

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.IsTrue(PathFindingResult.GetLastTile() != null);
            Assert.AreEqual(19, PathFindingResult.GetLastTile().TraversalCost);
            Assert.AreEqual(16, PathFindingResult.Path.Count);
        }


        [TestMethod]
        public void Test_GiantRandomMap_WithInefficentPath()
        {
            //Arrange
            Vector3 startLocation = new(0, 0, 0);
            Vector3 endLocation = new(69, 0, 39);
            string[,] map = CreateGiantMap();

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.AreEqual(97, PathFindingResult.Path.Count);
            CreateDebugPictureOfMapAndRoute(70, 40, PathFindingResult.Path, map);
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
            map[1, 1] = CoverType.FullCover;
            map[1, 2] = CoverType.FullCover;
            map[1, 3] = CoverType.FullCover;
            map[2, 1] = CoverType.FullCover;
            map[2, 3] = CoverType.FullCover;
            map[3, 1] = CoverType.FullCover;
            map[3, 2] = CoverType.FullCover;
            map[3, 3] = CoverType.FullCover;
            Vector3 startLocation = new(2, 0, 2);
            Vector3 endLocation = new(2, 0, 4);

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Count == 0);
            Assert.IsTrue(PathFindingResult.Tiles.Count == 0);
            Assert.IsTrue(PathFindingResult.GetLastTile() == null);
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
            map[1, 1] = CoverType.FullCover;
            map[1, 2] = CoverType.FullCover;
            map[1, 3] = CoverType.FullCover;
            map[2, 1] = CoverType.FullCover;
            map[2, 3] = CoverType.FullCover;
            map[3, 1] = CoverType.FullCover;
            map[3, 2] = CoverType.FullCover;
            map[3, 3] = CoverType.FullCover;

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(startLocation, endLocation, map);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Count == 0);
            Assert.IsTrue(PathFindingResult.Tiles.Count == 0);
            Assert.IsTrue(PathFindingResult.GetLastTile() == null);
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
            MapTile tile = new(3, 3, "", new Vector3(6, 0, 6));

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
            map[6, 0] = CoverType.FullCover;
            map[8, 0] = CoverType.FullCover;
            map[10, 0] = CoverType.FullCover;
            map[17, 0] = CoverType.FullCover;
            map[18, 0] = CoverType.FullCover;
            map[20, 0] = CoverType.FullCover;
            map[23, 0] = CoverType.FullCover;
            map[24, 0] = CoverType.FullCover;
            map[30, 0] = CoverType.FullCover;
            map[33, 0] = CoverType.FullCover;
            map[34, 0] = CoverType.FullCover;
            map[36, 0] = CoverType.FullCover;
            map[40, 0] = CoverType.FullCover;
            map[42, 0] = CoverType.FullCover;
            map[44, 0] = CoverType.FullCover;
            map[46, 0] = CoverType.FullCover;
            map[52, 0] = CoverType.FullCover;
            map[53, 0] = CoverType.FullCover;
            map[58, 0] = CoverType.FullCover;
            map[61, 0] = CoverType.FullCover;
            map[62, 0] = CoverType.FullCover;
            map[63, 0] = CoverType.FullCover;
            map[67, 0] = CoverType.FullCover;
            map[0, 1] = CoverType.FullCover;
            map[2, 1] = CoverType.FullCover;
            map[8, 1] = CoverType.FullCover;
            map[9, 1] = CoverType.FullCover;
            map[12, 1] = CoverType.FullCover;
            map[13, 1] = CoverType.FullCover;
            map[14, 1] = CoverType.FullCover;
            map[15, 1] = CoverType.FullCover;
            map[16, 1] = CoverType.FullCover;
            map[17, 1] = CoverType.FullCover;
            map[23, 1] = CoverType.FullCover;
            map[36, 1] = CoverType.FullCover;
            map[37, 1] = CoverType.FullCover;
            map[41, 1] = CoverType.FullCover;
            map[43, 1] = CoverType.FullCover;
            map[45, 1] = CoverType.FullCover;
            map[47, 1] = CoverType.FullCover;
            map[49, 1] = CoverType.FullCover;
            map[51, 1] = CoverType.FullCover;
            map[52, 1] = CoverType.FullCover;
            map[56, 1] = CoverType.FullCover;
            map[58, 1] = CoverType.FullCover;
            map[63, 1] = CoverType.FullCover;
            map[66, 1] = CoverType.FullCover;
            map[3, 2] = CoverType.FullCover;
            map[5, 2] = CoverType.FullCover;
            map[7, 2] = CoverType.FullCover;
            map[8, 2] = CoverType.FullCover;
            map[10, 2] = CoverType.FullCover;
            map[14, 2] = CoverType.FullCover;
            map[15, 2] = CoverType.FullCover;
            map[18, 2] = CoverType.FullCover;
            map[22, 2] = CoverType.FullCover;
            map[24, 2] = CoverType.FullCover;
            map[29, 2] = CoverType.FullCover;
            map[30, 2] = CoverType.FullCover;
            map[32, 2] = CoverType.FullCover;
            map[33, 2] = CoverType.FullCover;
            map[34, 2] = CoverType.FullCover;
            map[37, 2] = CoverType.FullCover;
            map[39, 2] = CoverType.FullCover;
            map[47, 2] = CoverType.FullCover;
            map[48, 2] = CoverType.FullCover;
            map[51, 2] = CoverType.FullCover;
            map[52, 2] = CoverType.FullCover;
            map[54, 2] = CoverType.FullCover;
            map[57, 2] = CoverType.FullCover;
            map[59, 2] = CoverType.FullCover;
            map[61, 2] = CoverType.FullCover;
            map[63, 2] = CoverType.FullCover;
            map[65, 2] = CoverType.FullCover;
            map[4, 3] = CoverType.FullCover;
            map[11, 3] = CoverType.FullCover;
            map[13, 3] = CoverType.FullCover;
            map[21, 3] = CoverType.FullCover;
            map[22, 3] = CoverType.FullCover;
            map[26, 3] = CoverType.FullCover;
            map[30, 3] = CoverType.FullCover;
            map[31, 3] = CoverType.FullCover;
            map[34, 3] = CoverType.FullCover;
            map[41, 3] = CoverType.FullCover;
            map[42, 3] = CoverType.FullCover;
            map[44, 3] = CoverType.FullCover;
            map[51, 3] = CoverType.FullCover;
            map[55, 3] = CoverType.FullCover;
            map[57, 3] = CoverType.FullCover;
            map[63, 3] = CoverType.FullCover;
            map[0, 4] = CoverType.FullCover;
            map[1, 4] = CoverType.FullCover;
            map[4, 4] = CoverType.FullCover;
            map[9, 4] = CoverType.FullCover;
            map[12, 4] = CoverType.FullCover;
            map[13, 4] = CoverType.FullCover;
            map[18, 4] = CoverType.FullCover;
            map[19, 4] = CoverType.FullCover;
            map[20, 4] = CoverType.FullCover;
            map[24, 4] = CoverType.FullCover;
            map[25, 4] = CoverType.FullCover;
            map[26, 4] = CoverType.FullCover;
            map[27, 4] = CoverType.FullCover;
            map[28, 4] = CoverType.FullCover;
            map[36, 4] = CoverType.FullCover;
            map[37, 4] = CoverType.FullCover;
            map[40, 4] = CoverType.FullCover;
            map[41, 4] = CoverType.FullCover;
            map[44, 4] = CoverType.FullCover;
            map[50, 4] = CoverType.FullCover;
            map[56, 4] = CoverType.FullCover;
            map[57, 4] = CoverType.FullCover;
            map[59, 4] = CoverType.FullCover;
            map[62, 4] = CoverType.FullCover;
            map[64, 4] = CoverType.FullCover;
            map[65, 4] = CoverType.FullCover;
            map[66, 4] = CoverType.FullCover;
            map[67, 4] = CoverType.FullCover;
            map[69, 4] = CoverType.FullCover;
            map[1, 5] = CoverType.FullCover;
            map[2, 5] = CoverType.FullCover;
            map[5, 5] = CoverType.FullCover;
            map[9, 5] = CoverType.FullCover;
            map[15, 5] = CoverType.FullCover;
            map[18, 5] = CoverType.FullCover;
            map[25, 5] = CoverType.FullCover;
            map[28, 5] = CoverType.FullCover;
            map[31, 5] = CoverType.FullCover;
            map[32, 5] = CoverType.FullCover;
            map[35, 5] = CoverType.FullCover;
            map[36, 5] = CoverType.FullCover;
            map[37, 5] = CoverType.FullCover;
            map[43, 5] = CoverType.FullCover;
            map[48, 5] = CoverType.FullCover;
            map[49, 5] = CoverType.FullCover;
            map[50, 5] = CoverType.FullCover;
            map[51, 5] = CoverType.FullCover;
            map[52, 5] = CoverType.FullCover;
            map[60, 5] = CoverType.FullCover;
            map[62, 5] = CoverType.FullCover;
            map[69, 5] = CoverType.FullCover;
            map[2, 6] = CoverType.FullCover;
            map[5, 6] = CoverType.FullCover;
            map[6, 6] = CoverType.FullCover;
            map[9, 6] = CoverType.FullCover;
            map[11, 6] = CoverType.FullCover;
            map[13, 6] = CoverType.FullCover;
            map[15, 6] = CoverType.FullCover;
            map[16, 6] = CoverType.FullCover;
            map[18, 6] = CoverType.FullCover;
            map[23, 6] = CoverType.FullCover;
            map[24, 6] = CoverType.FullCover;
            map[26, 6] = CoverType.FullCover;
            map[27, 6] = CoverType.FullCover;
            map[28, 6] = CoverType.FullCover;
            map[29, 6] = CoverType.FullCover;
            map[33, 6] = CoverType.FullCover;
            map[34, 6] = CoverType.FullCover;
            map[36, 6] = CoverType.FullCover;
            map[37, 6] = CoverType.FullCover;
            map[44, 6] = CoverType.FullCover;
            map[45, 6] = CoverType.FullCover;
            map[47, 6] = CoverType.FullCover;
            map[50, 6] = CoverType.FullCover;
            map[55, 6] = CoverType.FullCover;
            map[59, 6] = CoverType.FullCover;
            map[61, 6] = CoverType.FullCover;
            map[62, 6] = CoverType.FullCover;
            map[65, 6] = CoverType.FullCover;
            map[66, 6] = CoverType.FullCover;
            map[68, 6] = CoverType.FullCover;
            map[69, 6] = CoverType.FullCover;
            map[0, 7] = CoverType.FullCover;
            map[4, 7] = CoverType.FullCover;
            map[7, 7] = CoverType.FullCover;
            map[9, 7] = CoverType.FullCover;
            map[11, 7] = CoverType.FullCover;
            map[12, 7] = CoverType.FullCover;
            map[21, 7] = CoverType.FullCover;
            map[34, 7] = CoverType.FullCover;
            map[35, 7] = CoverType.FullCover;
            map[37, 7] = CoverType.FullCover;
            map[47, 7] = CoverType.FullCover;
            map[48, 7] = CoverType.FullCover;
            map[50, 7] = CoverType.FullCover;
            map[51, 7] = CoverType.FullCover;
            map[53, 7] = CoverType.FullCover;
            map[57, 7] = CoverType.FullCover;
            map[62, 7] = CoverType.FullCover;
            map[63, 7] = CoverType.FullCover;
            map[64, 7] = CoverType.FullCover;
            map[66, 7] = CoverType.FullCover;
            map[69, 7] = CoverType.FullCover;
            map[2, 8] = CoverType.FullCover;
            map[4, 8] = CoverType.FullCover;
            map[5, 8] = CoverType.FullCover;
            map[6, 8] = CoverType.FullCover;
            map[8, 8] = CoverType.FullCover;
            map[9, 8] = CoverType.FullCover;
            map[11, 8] = CoverType.FullCover;
            map[13, 8] = CoverType.FullCover;
            map[16, 8] = CoverType.FullCover;
            map[19, 8] = CoverType.FullCover;
            map[24, 8] = CoverType.FullCover;
            map[25, 8] = CoverType.FullCover;
            map[26, 8] = CoverType.FullCover;
            map[28, 8] = CoverType.FullCover;
            map[29, 8] = CoverType.FullCover;
            map[30, 8] = CoverType.FullCover;
            map[31, 8] = CoverType.FullCover;
            map[34, 8] = CoverType.FullCover;
            map[36, 8] = CoverType.FullCover;
            map[38, 8] = CoverType.FullCover;
            map[42, 8] = CoverType.FullCover;
            map[44, 8] = CoverType.FullCover;
            map[45, 8] = CoverType.FullCover;
            map[46, 8] = CoverType.FullCover;
            map[47, 8] = CoverType.FullCover;
            map[53, 8] = CoverType.FullCover;
            map[55, 8] = CoverType.FullCover;
            map[59, 8] = CoverType.FullCover;
            map[61, 8] = CoverType.FullCover;
            map[63, 8] = CoverType.FullCover;
            map[1, 9] = CoverType.FullCover;
            map[5, 9] = CoverType.FullCover;
            map[6, 9] = CoverType.FullCover;
            map[10, 9] = CoverType.FullCover;
            map[11, 9] = CoverType.FullCover;
            map[16, 9] = CoverType.FullCover;
            map[17, 9] = CoverType.FullCover;
            map[19, 9] = CoverType.FullCover;
            map[22, 9] = CoverType.FullCover;
            map[25, 9] = CoverType.FullCover;
            map[26, 9] = CoverType.FullCover;
            map[27, 9] = CoverType.FullCover;
            map[28, 9] = CoverType.FullCover;
            map[31, 9] = CoverType.FullCover;
            map[35, 9] = CoverType.FullCover;
            map[39, 9] = CoverType.FullCover;
            map[42, 9] = CoverType.FullCover;
            map[43, 9] = CoverType.FullCover;
            map[44, 9] = CoverType.FullCover;
            map[47, 9] = CoverType.FullCover;
            map[50, 9] = CoverType.FullCover;
            map[51, 9] = CoverType.FullCover;
            map[55, 9] = CoverType.FullCover;
            map[56, 9] = CoverType.FullCover;
            map[60, 9] = CoverType.FullCover;
            map[61, 9] = CoverType.FullCover;
            map[64, 9] = CoverType.FullCover;
            map[68, 9] = CoverType.FullCover;
            map[69, 9] = CoverType.FullCover;
            map[0, 10] = CoverType.FullCover;
            map[3, 10] = CoverType.FullCover;
            map[6, 10] = CoverType.FullCover;
            map[7, 10] = CoverType.FullCover;
            map[9, 10] = CoverType.FullCover;
            map[11, 10] = CoverType.FullCover;
            map[13, 10] = CoverType.FullCover;
            map[15, 10] = CoverType.FullCover;
            map[17, 10] = CoverType.FullCover;
            map[18, 10] = CoverType.FullCover;
            map[19, 10] = CoverType.FullCover;
            map[20, 10] = CoverType.FullCover;
            map[23, 10] = CoverType.FullCover;
            map[29, 10] = CoverType.FullCover;
            map[30, 10] = CoverType.FullCover;
            map[31, 10] = CoverType.FullCover;
            map[33, 10] = CoverType.FullCover;
            map[35, 10] = CoverType.FullCover;
            map[37, 10] = CoverType.FullCover;
            map[39, 10] = CoverType.FullCover;
            map[41, 10] = CoverType.FullCover;
            map[42, 10] = CoverType.FullCover;
            map[45, 10] = CoverType.FullCover;
            map[46, 10] = CoverType.FullCover;
            map[47, 10] = CoverType.FullCover;
            map[50, 10] = CoverType.FullCover;
            map[55, 10] = CoverType.FullCover;
            map[56, 10] = CoverType.FullCover;
            map[58, 10] = CoverType.FullCover;
            map[62, 10] = CoverType.FullCover;
            map[66, 10] = CoverType.FullCover;
            map[2, 11] = CoverType.FullCover;
            map[3, 11] = CoverType.FullCover;
            map[5, 11] = CoverType.FullCover;
            map[7, 11] = CoverType.FullCover;
            map[8, 11] = CoverType.FullCover;
            map[14, 11] = CoverType.FullCover;
            map[17, 11] = CoverType.FullCover;
            map[19, 11] = CoverType.FullCover;
            map[23, 11] = CoverType.FullCover;
            map[25, 11] = CoverType.FullCover;
            map[30, 11] = CoverType.FullCover;
            map[33, 11] = CoverType.FullCover;
            map[34, 11] = CoverType.FullCover;
            map[36, 11] = CoverType.FullCover;
            map[38, 11] = CoverType.FullCover;
            map[41, 11] = CoverType.FullCover;
            map[50, 11] = CoverType.FullCover;
            map[52, 11] = CoverType.FullCover;
            map[59, 11] = CoverType.FullCover;
            map[60, 11] = CoverType.FullCover;
            map[61, 11] = CoverType.FullCover;
            map[62, 11] = CoverType.FullCover;
            map[66, 11] = CoverType.FullCover;
            map[2, 12] = CoverType.FullCover;
            map[11, 12] = CoverType.FullCover;
            map[12, 12] = CoverType.FullCover;
            map[15, 12] = CoverType.FullCover;
            map[17, 12] = CoverType.FullCover;
            map[21, 12] = CoverType.FullCover;
            map[24, 12] = CoverType.FullCover;
            map[25, 12] = CoverType.FullCover;
            map[26, 12] = CoverType.FullCover;
            map[35, 12] = CoverType.FullCover;
            map[37, 12] = CoverType.FullCover;
            map[40, 12] = CoverType.FullCover;
            map[41, 12] = CoverType.FullCover;
            map[42, 12] = CoverType.FullCover;
            map[43, 12] = CoverType.FullCover;
            map[45, 12] = CoverType.FullCover;
            map[46, 12] = CoverType.FullCover;
            map[47, 12] = CoverType.FullCover;
            map[48, 12] = CoverType.FullCover;
            map[50, 12] = CoverType.FullCover;
            map[51, 12] = CoverType.FullCover;
            map[55, 12] = CoverType.FullCover;
            map[56, 12] = CoverType.FullCover;
            map[57, 12] = CoverType.FullCover;
            map[58, 12] = CoverType.FullCover;
            map[63, 12] = CoverType.FullCover;
            map[64, 12] = CoverType.FullCover;
            map[66, 12] = CoverType.FullCover;
            map[67, 12] = CoverType.FullCover;
            map[68, 12] = CoverType.FullCover;
            map[0, 13] = CoverType.FullCover;
            map[1, 13] = CoverType.FullCover;
            map[2, 13] = CoverType.FullCover;
            map[4, 13] = CoverType.FullCover;
            map[6, 13] = CoverType.FullCover;
            map[8, 13] = CoverType.FullCover;
            map[9, 13] = CoverType.FullCover;
            map[10, 13] = CoverType.FullCover;
            map[13, 13] = CoverType.FullCover;
            map[15, 13] = CoverType.FullCover;
            map[16, 13] = CoverType.FullCover;
            map[17, 13] = CoverType.FullCover;
            map[18, 13] = CoverType.FullCover;
            map[22, 13] = CoverType.FullCover;
            map[23, 13] = CoverType.FullCover;
            map[24, 13] = CoverType.FullCover;
            map[26, 13] = CoverType.FullCover;
            map[27, 13] = CoverType.FullCover;
            map[28, 13] = CoverType.FullCover;
            map[44, 13] = CoverType.FullCover;
            map[46, 13] = CoverType.FullCover;
            map[48, 13] = CoverType.FullCover;
            map[51, 13] = CoverType.FullCover;
            map[52, 13] = CoverType.FullCover;
            map[55, 13] = CoverType.FullCover;
            map[56, 13] = CoverType.FullCover;
            map[57, 13] = CoverType.FullCover;
            map[65, 13] = CoverType.FullCover;
            map[66, 13] = CoverType.FullCover;
            map[0, 14] = CoverType.FullCover;
            map[1, 14] = CoverType.FullCover;
            map[2, 14] = CoverType.FullCover;
            map[5, 14] = CoverType.FullCover;
            map[7, 14] = CoverType.FullCover;
            map[8, 14] = CoverType.FullCover;
            map[9, 14] = CoverType.FullCover;
            map[10, 14] = CoverType.FullCover;
            map[13, 14] = CoverType.FullCover;
            map[14, 14] = CoverType.FullCover;
            map[15, 14] = CoverType.FullCover;
            map[18, 14] = CoverType.FullCover;
            map[19, 14] = CoverType.FullCover;
            map[25, 14] = CoverType.FullCover;
            map[27, 14] = CoverType.FullCover;
            map[28, 14] = CoverType.FullCover;
            map[32, 14] = CoverType.FullCover;
            map[33, 14] = CoverType.FullCover;
            map[35, 14] = CoverType.FullCover;
            map[38, 14] = CoverType.FullCover;
            map[39, 14] = CoverType.FullCover;
            map[41, 14] = CoverType.FullCover;
            map[42, 14] = CoverType.FullCover;
            map[43, 14] = CoverType.FullCover;
            map[45, 14] = CoverType.FullCover;
            map[48, 14] = CoverType.FullCover;
            map[51, 14] = CoverType.FullCover;
            map[59, 14] = CoverType.FullCover;
            map[60, 14] = CoverType.FullCover;
            map[63, 14] = CoverType.FullCover;
            map[64, 14] = CoverType.FullCover;
            map[68, 14] = CoverType.FullCover;
            map[0, 15] = CoverType.FullCover;
            map[1, 15] = CoverType.FullCover;
            map[3, 15] = CoverType.FullCover;
            map[5, 15] = CoverType.FullCover;
            map[7, 15] = CoverType.FullCover;
            map[8, 15] = CoverType.FullCover;
            map[10, 15] = CoverType.FullCover;
            map[14, 15] = CoverType.FullCover;
            map[16, 15] = CoverType.FullCover;
            map[19, 15] = CoverType.FullCover;
            map[21, 15] = CoverType.FullCover;
            map[22, 15] = CoverType.FullCover;
            map[23, 15] = CoverType.FullCover;
            map[26, 15] = CoverType.FullCover;
            map[28, 15] = CoverType.FullCover;
            map[31, 15] = CoverType.FullCover;
            map[32, 15] = CoverType.FullCover;
            map[34, 15] = CoverType.FullCover;
            map[37, 15] = CoverType.FullCover;
            map[39, 15] = CoverType.FullCover;
            map[47, 15] = CoverType.FullCover;
            map[49, 15] = CoverType.FullCover;
            map[51, 15] = CoverType.FullCover;
            map[53, 15] = CoverType.FullCover;
            map[56, 15] = CoverType.FullCover;
            map[58, 15] = CoverType.FullCover;
            map[60, 15] = CoverType.FullCover;
            map[62, 15] = CoverType.FullCover;
            map[64, 15] = CoverType.FullCover;
            map[65, 15] = CoverType.FullCover;
            map[66, 15] = CoverType.FullCover;
            map[7, 16] = CoverType.FullCover;
            map[8, 16] = CoverType.FullCover;
            map[10, 16] = CoverType.FullCover;
            map[11, 16] = CoverType.FullCover;
            map[12, 16] = CoverType.FullCover;
            map[16, 16] = CoverType.FullCover;
            map[17, 16] = CoverType.FullCover;
            map[19, 16] = CoverType.FullCover;
            map[22, 16] = CoverType.FullCover;
            map[23, 16] = CoverType.FullCover;
            map[26, 16] = CoverType.FullCover;
            map[27, 16] = CoverType.FullCover;
            map[31, 16] = CoverType.FullCover;
            map[33, 16] = CoverType.FullCover;
            map[35, 16] = CoverType.FullCover;
            map[37, 16] = CoverType.FullCover;
            map[40, 16] = CoverType.FullCover;
            map[45, 16] = CoverType.FullCover;
            map[51, 16] = CoverType.FullCover;
            map[54, 16] = CoverType.FullCover;
            map[63, 16] = CoverType.FullCover;
            map[64, 16] = CoverType.FullCover;
            map[65, 16] = CoverType.FullCover;
            map[67, 16] = CoverType.FullCover;
            map[0, 17] = CoverType.FullCover;
            map[1, 17] = CoverType.FullCover;
            map[3, 17] = CoverType.FullCover;
            map[4, 17] = CoverType.FullCover;
            map[6, 17] = CoverType.FullCover;
            map[10, 17] = CoverType.FullCover;
            map[19, 17] = CoverType.FullCover;
            map[20, 17] = CoverType.FullCover;
            map[25, 17] = CoverType.FullCover;
            map[27, 17] = CoverType.FullCover;
            map[28, 17] = CoverType.FullCover;
            map[29, 17] = CoverType.FullCover;
            map[37, 17] = CoverType.FullCover;
            map[41, 17] = CoverType.FullCover;
            map[45, 17] = CoverType.FullCover;
            map[46, 17] = CoverType.FullCover;
            map[47, 17] = CoverType.FullCover;
            map[52, 17] = CoverType.FullCover;
            map[55, 17] = CoverType.FullCover;
            map[61, 17] = CoverType.FullCover;
            map[62, 17] = CoverType.FullCover;
            map[63, 17] = CoverType.FullCover;
            map[64, 17] = CoverType.FullCover;
            map[67, 17] = CoverType.FullCover;
            map[68, 17] = CoverType.FullCover;
            map[69, 17] = CoverType.FullCover;
            map[0, 18] = CoverType.FullCover;
            map[1, 18] = CoverType.FullCover;
            map[2, 18] = CoverType.FullCover;
            map[3, 18] = CoverType.FullCover;
            map[5, 18] = CoverType.FullCover;
            map[8, 18] = CoverType.FullCover;
            map[16, 18] = CoverType.FullCover;
            map[17, 18] = CoverType.FullCover;
            map[19, 18] = CoverType.FullCover;
            map[20, 18] = CoverType.FullCover;
            map[31, 18] = CoverType.FullCover;
            map[32, 18] = CoverType.FullCover;
            map[33, 18] = CoverType.FullCover;
            map[34, 18] = CoverType.FullCover;
            map[35, 18] = CoverType.FullCover;
            map[37, 18] = CoverType.FullCover;
            map[40, 18] = CoverType.FullCover;
            map[42, 18] = CoverType.FullCover;
            map[45, 18] = CoverType.FullCover;
            map[46, 18] = CoverType.FullCover;
            map[47, 18] = CoverType.FullCover;
            map[51, 18] = CoverType.FullCover;
            map[52, 18] = CoverType.FullCover;
            map[55, 18] = CoverType.FullCover;
            map[58, 18] = CoverType.FullCover;
            map[59, 18] = CoverType.FullCover;
            map[60, 18] = CoverType.FullCover;
            map[61, 18] = CoverType.FullCover;
            map[62, 18] = CoverType.FullCover;
            map[64, 18] = CoverType.FullCover;
            map[67, 18] = CoverType.FullCover;
            map[0, 19] = CoverType.FullCover;
            map[3, 19] = CoverType.FullCover;
            map[4, 19] = CoverType.FullCover;
            map[5, 19] = CoverType.FullCover;
            map[7, 19] = CoverType.FullCover;
            map[10, 19] = CoverType.FullCover;
            map[12, 19] = CoverType.FullCover;
            map[15, 19] = CoverType.FullCover;
            map[17, 19] = CoverType.FullCover;
            map[23, 19] = CoverType.FullCover;
            map[24, 19] = CoverType.FullCover;
            map[25, 19] = CoverType.FullCover;
            map[29, 19] = CoverType.FullCover;
            map[33, 19] = CoverType.FullCover;
            map[34, 19] = CoverType.FullCover;
            map[37, 19] = CoverType.FullCover;
            map[44, 19] = CoverType.FullCover;
            map[47, 19] = CoverType.FullCover;
            map[48, 19] = CoverType.FullCover;
            map[51, 19] = CoverType.FullCover;
            map[57, 19] = CoverType.FullCover;
            map[59, 19] = CoverType.FullCover;
            map[64, 19] = CoverType.FullCover;
            map[65, 19] = CoverType.FullCover;
            map[67, 19] = CoverType.FullCover;
            map[7, 20] = CoverType.FullCover;
            map[8, 20] = CoverType.FullCover;
            map[10, 20] = CoverType.FullCover;
            map[14, 20] = CoverType.FullCover;
            map[15, 20] = CoverType.FullCover;
            map[16, 20] = CoverType.FullCover;
            map[21, 20] = CoverType.FullCover;
            map[22, 20] = CoverType.FullCover;
            map[24, 20] = CoverType.FullCover;
            map[25, 20] = CoverType.FullCover;
            map[26, 20] = CoverType.FullCover;
            map[28, 20] = CoverType.FullCover;
            map[29, 20] = CoverType.FullCover;
            map[30, 20] = CoverType.FullCover;
            map[33, 20] = CoverType.FullCover;
            map[34, 20] = CoverType.FullCover;
            map[35, 20] = CoverType.FullCover;
            map[37, 20] = CoverType.FullCover;
            map[39, 20] = CoverType.FullCover;
            map[42, 20] = CoverType.FullCover;
            map[45, 20] = CoverType.FullCover;
            map[49, 20] = CoverType.FullCover;
            map[50, 20] = CoverType.FullCover;
            map[54, 20] = CoverType.FullCover;
            map[55, 20] = CoverType.FullCover;
            map[56, 20] = CoverType.FullCover;
            map[57, 20] = CoverType.FullCover;
            map[60, 20] = CoverType.FullCover;
            map[61, 20] = CoverType.FullCover;
            map[62, 20] = CoverType.FullCover;
            map[64, 20] = CoverType.FullCover;
            map[6, 21] = CoverType.FullCover;
            map[9, 21] = CoverType.FullCover;
            map[13, 21] = CoverType.FullCover;
            map[14, 21] = CoverType.FullCover;
            map[15, 21] = CoverType.FullCover;
            map[20, 21] = CoverType.FullCover;
            map[23, 21] = CoverType.FullCover;
            map[27, 21] = CoverType.FullCover;
            map[28, 21] = CoverType.FullCover;
            map[29, 21] = CoverType.FullCover;
            map[30, 21] = CoverType.FullCover;
            map[31, 21] = CoverType.FullCover;
            map[34, 21] = CoverType.FullCover;
            map[35, 21] = CoverType.FullCover;
            map[36, 21] = CoverType.FullCover;
            map[37, 21] = CoverType.FullCover;
            map[38, 21] = CoverType.FullCover;
            map[39, 21] = CoverType.FullCover;
            map[40, 21] = CoverType.FullCover;
            map[42, 21] = CoverType.FullCover;
            map[43, 21] = CoverType.FullCover;
            map[44, 21] = CoverType.FullCover;
            map[46, 21] = CoverType.FullCover;
            map[49, 21] = CoverType.FullCover;
            map[51, 21] = CoverType.FullCover;
            map[55, 21] = CoverType.FullCover;
            map[59, 21] = CoverType.FullCover;
            map[65, 21] = CoverType.FullCover;
            map[67, 21] = CoverType.FullCover;
            map[0, 22] = CoverType.FullCover;
            map[5, 22] = CoverType.FullCover;
            map[6, 22] = CoverType.FullCover;
            map[8, 22] = CoverType.FullCover;
            map[10, 22] = CoverType.FullCover;
            map[15, 22] = CoverType.FullCover;
            map[19, 22] = CoverType.FullCover;
            map[20, 22] = CoverType.FullCover;
            map[25, 22] = CoverType.FullCover;
            map[26, 22] = CoverType.FullCover;
            map[30, 22] = CoverType.FullCover;
            map[31, 22] = CoverType.FullCover;
            map[34, 22] = CoverType.FullCover;
            map[35, 22] = CoverType.FullCover;
            map[37, 22] = CoverType.FullCover;
            map[38, 22] = CoverType.FullCover;
            map[40, 22] = CoverType.FullCover;
            map[41, 22] = CoverType.FullCover;
            map[42, 22] = CoverType.FullCover;
            map[44, 22] = CoverType.FullCover;
            map[46, 22] = CoverType.FullCover;
            map[59, 22] = CoverType.FullCover;
            map[60, 22] = CoverType.FullCover;
            map[61, 22] = CoverType.FullCover;
            map[62, 22] = CoverType.FullCover;
            map[65, 22] = CoverType.FullCover;
            map[66, 22] = CoverType.FullCover;
            map[69, 22] = CoverType.FullCover;
            map[4, 23] = CoverType.FullCover;
            map[9, 23] = CoverType.FullCover;
            map[10, 23] = CoverType.FullCover;
            map[12, 23] = CoverType.FullCover;
            map[13, 23] = CoverType.FullCover;
            map[14, 23] = CoverType.FullCover;
            map[17, 23] = CoverType.FullCover;
            map[18, 23] = CoverType.FullCover;
            map[24, 23] = CoverType.FullCover;
            map[26, 23] = CoverType.FullCover;
            map[28, 23] = CoverType.FullCover;
            map[31, 23] = CoverType.FullCover;
            map[35, 23] = CoverType.FullCover;
            map[37, 23] = CoverType.FullCover;
            map[40, 23] = CoverType.FullCover;
            map[42, 23] = CoverType.FullCover;
            map[43, 23] = CoverType.FullCover;
            map[44, 23] = CoverType.FullCover;
            map[46, 23] = CoverType.FullCover;
            map[47, 23] = CoverType.FullCover;
            map[49, 23] = CoverType.FullCover;
            map[51, 23] = CoverType.FullCover;
            map[55, 23] = CoverType.FullCover;
            map[58, 23] = CoverType.FullCover;
            map[59, 23] = CoverType.FullCover;
            map[2, 24] = CoverType.FullCover;
            map[11, 24] = CoverType.FullCover;
            map[12, 24] = CoverType.FullCover;
            map[13, 24] = CoverType.FullCover;
            map[14, 24] = CoverType.FullCover;
            map[17, 24] = CoverType.FullCover;
            map[19, 24] = CoverType.FullCover;
            map[21, 24] = CoverType.FullCover;
            map[25, 24] = CoverType.FullCover;
            map[26, 24] = CoverType.FullCover;
            map[27, 24] = CoverType.FullCover;
            map[28, 24] = CoverType.FullCover;
            map[29, 24] = CoverType.FullCover;
            map[30, 24] = CoverType.FullCover;
            map[35, 24] = CoverType.FullCover;
            map[37, 24] = CoverType.FullCover;
            map[38, 24] = CoverType.FullCover;
            map[39, 24] = CoverType.FullCover;
            map[41, 24] = CoverType.FullCover;
            map[44, 24] = CoverType.FullCover;
            map[47, 24] = CoverType.FullCover;
            map[49, 24] = CoverType.FullCover;
            map[51, 24] = CoverType.FullCover;
            map[53, 24] = CoverType.FullCover;
            map[55, 24] = CoverType.FullCover;
            map[62, 24] = CoverType.FullCover;
            map[64, 24] = CoverType.FullCover;
            map[65, 24] = CoverType.FullCover;
            map[69, 24] = CoverType.FullCover;
            map[1, 25] = CoverType.FullCover;
            map[5, 25] = CoverType.FullCover;
            map[6, 25] = CoverType.FullCover;
            map[7, 25] = CoverType.FullCover;
            map[13, 25] = CoverType.FullCover;
            map[16, 25] = CoverType.FullCover;
            map[17, 25] = CoverType.FullCover;
            map[18, 25] = CoverType.FullCover;
            map[19, 25] = CoverType.FullCover;
            map[20, 25] = CoverType.FullCover;
            map[21, 25] = CoverType.FullCover;
            map[25, 25] = CoverType.FullCover;
            map[27, 25] = CoverType.FullCover;
            map[28, 25] = CoverType.FullCover;
            map[29, 25] = CoverType.FullCover;
            map[31, 25] = CoverType.FullCover;
            map[33, 25] = CoverType.FullCover;
            map[35, 25] = CoverType.FullCover;
            map[39, 25] = CoverType.FullCover;
            map[40, 25] = CoverType.FullCover;
            map[43, 25] = CoverType.FullCover;
            map[44, 25] = CoverType.FullCover;
            map[47, 25] = CoverType.FullCover;
            map[50, 25] = CoverType.FullCover;
            map[54, 25] = CoverType.FullCover;
            map[56, 25] = CoverType.FullCover;
            map[58, 25] = CoverType.FullCover;
            map[60, 25] = CoverType.FullCover;
            map[61, 25] = CoverType.FullCover;
            map[65, 25] = CoverType.FullCover;
            map[69, 25] = CoverType.FullCover;
            map[1, 26] = CoverType.FullCover;
            map[4, 26] = CoverType.FullCover;
            map[6, 26] = CoverType.FullCover;
            map[8, 26] = CoverType.FullCover;
            map[9, 26] = CoverType.FullCover;
            map[12, 26] = CoverType.FullCover;
            map[15, 26] = CoverType.FullCover;
            map[18, 26] = CoverType.FullCover;
            map[19, 26] = CoverType.FullCover;
            map[23, 26] = CoverType.FullCover;
            map[25, 26] = CoverType.FullCover;
            map[28, 26] = CoverType.FullCover;
            map[34, 26] = CoverType.FullCover;
            map[35, 26] = CoverType.FullCover;
            map[36, 26] = CoverType.FullCover;
            map[40, 26] = CoverType.FullCover;
            map[42, 26] = CoverType.FullCover;
            map[53, 26] = CoverType.FullCover;
            map[56, 26] = CoverType.FullCover;
            map[57, 26] = CoverType.FullCover;
            map[59, 26] = CoverType.FullCover;
            map[62, 26] = CoverType.FullCover;
            map[63, 26] = CoverType.FullCover;
            map[64, 26] = CoverType.FullCover;
            map[66, 26] = CoverType.FullCover;
            map[4, 27] = CoverType.FullCover;
            map[5, 27] = CoverType.FullCover;
            map[8, 27] = CoverType.FullCover;
            map[10, 27] = CoverType.FullCover;
            map[14, 27] = CoverType.FullCover;
            map[15, 27] = CoverType.FullCover;
            map[16, 27] = CoverType.FullCover;
            map[17, 27] = CoverType.FullCover;
            map[18, 27] = CoverType.FullCover;
            map[21, 27] = CoverType.FullCover;
            map[25, 27] = CoverType.FullCover;
            map[29, 27] = CoverType.FullCover;
            map[30, 27] = CoverType.FullCover;
            map[37, 27] = CoverType.FullCover;
            map[38, 27] = CoverType.FullCover;
            map[39, 27] = CoverType.FullCover;
            map[43, 27] = CoverType.FullCover;
            map[48, 27] = CoverType.FullCover;
            map[49, 27] = CoverType.FullCover;
            map[50, 27] = CoverType.FullCover;
            map[54, 27] = CoverType.FullCover;
            map[57, 27] = CoverType.FullCover;
            map[58, 27] = CoverType.FullCover;
            map[62, 27] = CoverType.FullCover;
            map[63, 27] = CoverType.FullCover;
            map[64, 27] = CoverType.FullCover;
            map[69, 27] = CoverType.FullCover;
            map[0, 28] = CoverType.FullCover;
            map[1, 28] = CoverType.FullCover;
            map[5, 28] = CoverType.FullCover;
            map[6, 28] = CoverType.FullCover;
            map[8, 28] = CoverType.FullCover;
            map[10, 28] = CoverType.FullCover;
            map[14, 28] = CoverType.FullCover;
            map[15, 28] = CoverType.FullCover;
            map[16, 28] = CoverType.FullCover;
            map[19, 28] = CoverType.FullCover;
            map[20, 28] = CoverType.FullCover;
            map[24, 28] = CoverType.FullCover;
            map[28, 28] = CoverType.FullCover;
            map[29, 28] = CoverType.FullCover;
            map[31, 28] = CoverType.FullCover;
            map[32, 28] = CoverType.FullCover;
            map[35, 28] = CoverType.FullCover;
            map[40, 28] = CoverType.FullCover;
            map[41, 28] = CoverType.FullCover;
            map[42, 28] = CoverType.FullCover;
            map[43, 28] = CoverType.FullCover;
            map[45, 28] = CoverType.FullCover;
            map[46, 28] = CoverType.FullCover;
            map[47, 28] = CoverType.FullCover;
            map[50, 28] = CoverType.FullCover;
            map[52, 28] = CoverType.FullCover;
            map[53, 28] = CoverType.FullCover;
            map[54, 28] = CoverType.FullCover;
            map[56, 28] = CoverType.FullCover;
            map[57, 28] = CoverType.FullCover;
            map[65, 28] = CoverType.FullCover;
            map[69, 28] = CoverType.FullCover;
            map[0, 29] = CoverType.FullCover;
            map[8, 29] = CoverType.FullCover;
            map[9, 29] = CoverType.FullCover;
            map[11, 29] = CoverType.FullCover;
            map[13, 29] = CoverType.FullCover;
            map[14, 29] = CoverType.FullCover;
            map[19, 29] = CoverType.FullCover;
            map[22, 29] = CoverType.FullCover;
            map[29, 29] = CoverType.FullCover;
            map[30, 29] = CoverType.FullCover;
            map[31, 29] = CoverType.FullCover;
            map[32, 29] = CoverType.FullCover;
            map[38, 29] = CoverType.FullCover;
            map[39, 29] = CoverType.FullCover;
            map[40, 29] = CoverType.FullCover;
            map[42, 29] = CoverType.FullCover;
            map[43, 29] = CoverType.FullCover;
            map[44, 29] = CoverType.FullCover;
            map[45, 29] = CoverType.FullCover;
            map[49, 29] = CoverType.FullCover;
            map[52, 29] = CoverType.FullCover;
            map[54, 29] = CoverType.FullCover;
            map[57, 29] = CoverType.FullCover;
            map[58, 29] = CoverType.FullCover;
            map[59, 29] = CoverType.FullCover;
            map[60, 29] = CoverType.FullCover;
            map[61, 29] = CoverType.FullCover;
            map[62, 29] = CoverType.FullCover;
            map[64, 29] = CoverType.FullCover;
            map[65, 29] = CoverType.FullCover;
            map[1, 30] = CoverType.FullCover;
            map[4, 30] = CoverType.FullCover;
            map[5, 30] = CoverType.FullCover;
            map[9, 30] = CoverType.FullCover;
            map[10, 30] = CoverType.FullCover;
            map[11, 30] = CoverType.FullCover;
            map[13, 30] = CoverType.FullCover;
            map[15, 30] = CoverType.FullCover;
            map[16, 30] = CoverType.FullCover;
            map[20, 30] = CoverType.FullCover;
            map[22, 30] = CoverType.FullCover;
            map[23, 30] = CoverType.FullCover;
            map[25, 30] = CoverType.FullCover;
            map[27, 30] = CoverType.FullCover;
            map[28, 30] = CoverType.FullCover;
            map[29, 30] = CoverType.FullCover;
            map[33, 30] = CoverType.FullCover;
            map[34, 30] = CoverType.FullCover;
            map[35, 30] = CoverType.FullCover;
            map[39, 30] = CoverType.FullCover;
            map[40, 30] = CoverType.FullCover;
            map[43, 30] = CoverType.FullCover;
            map[45, 30] = CoverType.FullCover;
            map[46, 30] = CoverType.FullCover;
            map[48, 30] = CoverType.FullCover;
            map[50, 30] = CoverType.FullCover;
            map[51, 30] = CoverType.FullCover;
            map[52, 30] = CoverType.FullCover;
            map[54, 30] = CoverType.FullCover;
            map[56, 30] = CoverType.FullCover;
            map[57, 30] = CoverType.FullCover;
            map[59, 30] = CoverType.FullCover;
            map[61, 30] = CoverType.FullCover;
            map[66, 30] = CoverType.FullCover;
            map[1, 31] = CoverType.FullCover;
            map[8, 31] = CoverType.FullCover;
            map[12, 31] = CoverType.FullCover;
            map[13, 31] = CoverType.FullCover;
            map[15, 31] = CoverType.FullCover;
            map[17, 31] = CoverType.FullCover;
            map[21, 31] = CoverType.FullCover;
            map[22, 31] = CoverType.FullCover;
            map[27, 31] = CoverType.FullCover;
            map[30, 31] = CoverType.FullCover;
            map[38, 31] = CoverType.FullCover;
            map[41, 31] = CoverType.FullCover;
            map[46, 31] = CoverType.FullCover;
            map[47, 31] = CoverType.FullCover;
            map[51, 31] = CoverType.FullCover;
            map[53, 31] = CoverType.FullCover;
            map[57, 31] = CoverType.FullCover;
            map[58, 31] = CoverType.FullCover;
            map[62, 31] = CoverType.FullCover;
            map[66, 31] = CoverType.FullCover;
            map[69, 31] = CoverType.FullCover;
            map[1, 32] = CoverType.FullCover;
            map[3, 32] = CoverType.FullCover;
            map[4, 32] = CoverType.FullCover;
            map[6, 32] = CoverType.FullCover;
            map[8, 32] = CoverType.FullCover;
            map[13, 32] = CoverType.FullCover;
            map[16, 32] = CoverType.FullCover;
            map[19, 32] = CoverType.FullCover;
            map[21, 32] = CoverType.FullCover;
            map[24, 32] = CoverType.FullCover;
            map[26, 32] = CoverType.FullCover;
            map[27, 32] = CoverType.FullCover;
            map[28, 32] = CoverType.FullCover;
            map[31, 32] = CoverType.FullCover;
            map[36, 32] = CoverType.FullCover;
            map[39, 32] = CoverType.FullCover;
            map[40, 32] = CoverType.FullCover;
            map[42, 32] = CoverType.FullCover;
            map[45, 32] = CoverType.FullCover;
            map[46, 32] = CoverType.FullCover;
            map[47, 32] = CoverType.FullCover;
            map[48, 32] = CoverType.FullCover;
            map[51, 32] = CoverType.FullCover;
            map[53, 32] = CoverType.FullCover;
            map[54, 32] = CoverType.FullCover;
            map[57, 32] = CoverType.FullCover;
            map[61, 32] = CoverType.FullCover;
            map[62, 32] = CoverType.FullCover;
            map[64, 32] = CoverType.FullCover;
            map[67, 32] = CoverType.FullCover;
            map[69, 32] = CoverType.FullCover;
            map[2, 33] = CoverType.FullCover;
            map[4, 33] = CoverType.FullCover;
            map[5, 33] = CoverType.FullCover;
            map[6, 33] = CoverType.FullCover;
            map[7, 33] = CoverType.FullCover;
            map[9, 33] = CoverType.FullCover;
            map[10, 33] = CoverType.FullCover;
            map[11, 33] = CoverType.FullCover;
            map[14, 33] = CoverType.FullCover;
            map[18, 33] = CoverType.FullCover;
            map[19, 33] = CoverType.FullCover;
            map[23, 33] = CoverType.FullCover;
            map[26, 33] = CoverType.FullCover;
            map[28, 33] = CoverType.FullCover;
            map[31, 33] = CoverType.FullCover;
            map[33, 33] = CoverType.FullCover;
            map[34, 33] = CoverType.FullCover;
            map[36, 33] = CoverType.FullCover;
            map[37, 33] = CoverType.FullCover;
            map[43, 33] = CoverType.FullCover;
            map[44, 33] = CoverType.FullCover;
            map[48, 33] = CoverType.FullCover;
            map[50, 33] = CoverType.FullCover;
            map[53, 33] = CoverType.FullCover;
            map[54, 33] = CoverType.FullCover;
            map[58, 33] = CoverType.FullCover;
            map[59, 33] = CoverType.FullCover;
            map[65, 33] = CoverType.FullCover;
            map[68, 33] = CoverType.FullCover;
            map[0, 34] = CoverType.FullCover;
            map[1, 34] = CoverType.FullCover;
            map[3, 34] = CoverType.FullCover;
            map[5, 34] = CoverType.FullCover;
            map[9, 34] = CoverType.FullCover;
            map[10, 34] = CoverType.FullCover;
            map[12, 34] = CoverType.FullCover;
            map[17, 34] = CoverType.FullCover;
            map[18, 34] = CoverType.FullCover;
            map[21, 34] = CoverType.FullCover;
            map[26, 34] = CoverType.FullCover;
            map[29, 34] = CoverType.FullCover;
            map[30, 34] = CoverType.FullCover;
            map[32, 34] = CoverType.FullCover;
            map[34, 34] = CoverType.FullCover;
            map[40, 34] = CoverType.FullCover;
            map[42, 34] = CoverType.FullCover;
            map[43, 34] = CoverType.FullCover;
            map[44, 34] = CoverType.FullCover;
            map[45, 34] = CoverType.FullCover;
            map[48, 34] = CoverType.FullCover;
            map[49, 34] = CoverType.FullCover;
            map[51, 34] = CoverType.FullCover;
            map[55, 34] = CoverType.FullCover;
            map[58, 34] = CoverType.FullCover;
            map[60, 34] = CoverType.FullCover;
            map[62, 34] = CoverType.FullCover;
            map[64, 34] = CoverType.FullCover;
            map[65, 34] = CoverType.FullCover;
            map[67, 34] = CoverType.FullCover;
            map[1, 35] = CoverType.FullCover;
            map[5, 35] = CoverType.FullCover;
            map[11, 35] = CoverType.FullCover;
            map[12, 35] = CoverType.FullCover;
            map[13, 35] = CoverType.FullCover;
            map[18, 35] = CoverType.FullCover;
            map[19, 35] = CoverType.FullCover;
            map[20, 35] = CoverType.FullCover;
            map[21, 35] = CoverType.FullCover;
            map[22, 35] = CoverType.FullCover;
            map[25, 35] = CoverType.FullCover;
            map[28, 35] = CoverType.FullCover;
            map[31, 35] = CoverType.FullCover;
            map[32, 35] = CoverType.FullCover;
            map[33, 35] = CoverType.FullCover;
            map[34, 35] = CoverType.FullCover;
            map[37, 35] = CoverType.FullCover;
            map[38, 35] = CoverType.FullCover;
            map[40, 35] = CoverType.FullCover;
            map[41, 35] = CoverType.FullCover;
            map[42, 35] = CoverType.FullCover;
            map[44, 35] = CoverType.FullCover;
            map[49, 35] = CoverType.FullCover;
            map[52, 35] = CoverType.FullCover;
            map[53, 35] = CoverType.FullCover;
            map[59, 35] = CoverType.FullCover;
            map[61, 35] = CoverType.FullCover;
            map[62, 35] = CoverType.FullCover;
            map[64, 35] = CoverType.FullCover;
            map[65, 35] = CoverType.FullCover;
            map[67, 35] = CoverType.FullCover;
            map[69, 35] = CoverType.FullCover;
            map[0, 36] = CoverType.FullCover;
            map[2, 36] = CoverType.FullCover;
            map[4, 36] = CoverType.FullCover;
            map[5, 36] = CoverType.FullCover;
            map[6, 36] = CoverType.FullCover;
            map[12, 36] = CoverType.FullCover;
            map[13, 36] = CoverType.FullCover;
            map[14, 36] = CoverType.FullCover;
            map[17, 36] = CoverType.FullCover;
            map[22, 36] = CoverType.FullCover;
            map[29, 36] = CoverType.FullCover;
            map[35, 36] = CoverType.FullCover;
            map[41, 36] = CoverType.FullCover;
            map[42, 36] = CoverType.FullCover;
            map[43, 36] = CoverType.FullCover;
            map[45, 36] = CoverType.FullCover;
            map[46, 36] = CoverType.FullCover;
            map[47, 36] = CoverType.FullCover;
            map[49, 36] = CoverType.FullCover;
            map[53, 36] = CoverType.FullCover;
            map[57, 36] = CoverType.FullCover;
            map[60, 36] = CoverType.FullCover;
            map[62, 36] = CoverType.FullCover;
            map[68, 36] = CoverType.FullCover;
            map[0, 37] = CoverType.FullCover;
            map[2, 37] = CoverType.FullCover;
            map[4, 37] = CoverType.FullCover;
            map[5, 37] = CoverType.FullCover;
            map[6, 37] = CoverType.FullCover;
            map[8, 37] = CoverType.FullCover;
            map[12, 37] = CoverType.FullCover;
            map[13, 37] = CoverType.FullCover;
            map[19, 37] = CoverType.FullCover;
            map[21, 37] = CoverType.FullCover;
            map[24, 37] = CoverType.FullCover;
            map[26, 37] = CoverType.FullCover;
            map[40, 37] = CoverType.FullCover;
            map[42, 37] = CoverType.FullCover;
            map[50, 37] = CoverType.FullCover;
            map[52, 37] = CoverType.FullCover;
            map[53, 37] = CoverType.FullCover;
            map[54, 37] = CoverType.FullCover;
            map[62, 37] = CoverType.FullCover;
            map[63, 37] = CoverType.FullCover;
            map[65, 37] = CoverType.FullCover;
            map[66, 37] = CoverType.FullCover;
            map[68, 37] = CoverType.FullCover;
            map[0, 38] = CoverType.FullCover;
            map[1, 38] = CoverType.FullCover;
            map[3, 38] = CoverType.FullCover;
            map[5, 38] = CoverType.FullCover;
            map[6, 38] = CoverType.FullCover;
            map[10, 38] = CoverType.FullCover;
            map[11, 38] = CoverType.FullCover;
            map[13, 38] = CoverType.FullCover;
            map[19, 38] = CoverType.FullCover;
            map[27, 38] = CoverType.FullCover;
            map[31, 38] = CoverType.FullCover;
            map[33, 38] = CoverType.FullCover;
            map[37, 38] = CoverType.FullCover;
            map[44, 38] = CoverType.FullCover;
            map[47, 38] = CoverType.FullCover;
            map[48, 38] = CoverType.FullCover;
            map[52, 38] = CoverType.FullCover;
            map[53, 38] = CoverType.FullCover;
            map[54, 38] = CoverType.FullCover;
            map[58, 38] = CoverType.FullCover;
            map[61, 38] = CoverType.FullCover;
            map[65, 38] = CoverType.FullCover;
            map[69, 38] = CoverType.FullCover;
            map[1, 39] = CoverType.FullCover;
            map[5, 39] = CoverType.FullCover;
            map[6, 39] = CoverType.FullCover;
            map[8, 39] = CoverType.FullCover;
            map[9, 39] = CoverType.FullCover;
            map[10, 39] = CoverType.FullCover;
            map[11, 39] = CoverType.FullCover;
            map[15, 39] = CoverType.FullCover;
            map[17, 39] = CoverType.FullCover;
            map[21, 39] = CoverType.FullCover;
            map[22, 39] = CoverType.FullCover;
            map[25, 39] = CoverType.FullCover;
            map[30, 39] = CoverType.FullCover;
            map[34, 39] = CoverType.FullCover;
            map[36, 39] = CoverType.FullCover;
            map[38, 39] = CoverType.FullCover;
            map[40, 39] = CoverType.FullCover;
            map[41, 39] = CoverType.FullCover;
            map[42, 39] = CoverType.FullCover;
            map[43, 39] = CoverType.FullCover;
            map[44, 39] = CoverType.FullCover;
            map[46, 39] = CoverType.FullCover;
            map[47, 39] = CoverType.FullCover;
            map[48, 39] = CoverType.FullCover;
            map[51, 39] = CoverType.FullCover;
            map[52, 39] = CoverType.FullCover;
            map[53, 39] = CoverType.FullCover;
            map[58, 39] = CoverType.FullCover;
            map[64, 39] = CoverType.FullCover;
            map[65, 39] = CoverType.FullCover;
            return map;
        }
        #endregion

        #endregion

    }
}
