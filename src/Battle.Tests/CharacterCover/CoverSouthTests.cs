using Battle.Logic.CharacterCover;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.CharacterCover
{
    [TestClass]
    [TestCategory("L0")]
    public class CoverSouthTests
    {
        // In the diagrams
        // "□" is open space
        // CoverType.FullCover is cover/an object
        // "S" is the player
        // "E" is the enemy/opponent
        // "E" will look at "S", and the cover system will determine if "S" is in cover or not
        // If a player is not in cover, they are 'flanked', and vulnerable

        [TestMethod]
        public void Test_WithSouthCover_1_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  □ □ E □
            //  □ S □ □ 
            //  □ ■ □ □ 
            //  □ □ □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 3));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithSouthCover_3_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □
            //  □ S E □ 
            //  □ ■ □ □ 
            //  □ □ □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithSouthCover_4_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □
            //  □ S □ □ 
            //  □ ■ E □ 
            //  □ □ □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 1));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithSouthCover_5_O_Clock_Enemy()
        {
            // Arrange
            //  In Cover
            //  □ □ □ □
            //  □ S □ □ 
            //  □ ■ □ □ 
            //  □ □ E □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 0));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
        }

        [TestMethod]
        public void Test_WithSouthCover_6_O_Clock_Enemy()
        {
            // Arrange
            //  In Cover
            //  □ □ □ □
            //  □ S □ □ 
            //  □ ■ □ □ 
            //  □ E □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(1, 0, 0));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
        }

        [TestMethod]
        public void Test_WithSouthCover_7_O_Clock_Enemy()
        {
            // Arrange
            //  In Cover
            //  □ □ □ □
            //  □ S □ □ 
            //  □ ■ □ □ 
            //  E □ □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 0));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
        }

        [TestMethod]
        public void Test_WithSouthCover_8_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □
            //  □ S □ □ 
            //  E ■ □ □ 
            //  □ □ □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 1));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }


        [TestMethod]
        public void Test_WithSouthCover_9_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □
            //  E S □ □ 
            //  □ ■ □ □ 
            //  □ □ □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithSouthCover_10_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  E □ □ □
            //  □ S □ □ 
            //  □ ■ □ □ 
            //  □ □ □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 3));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithSouthCover_12_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  □ E □ □
            //  □ S □ □ 
            //  □ ■ □ □ 
            //  □ □ □ □
            Vector3 startingLocation = new(1, 0, 2);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(1, 0, 3));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

    }
}