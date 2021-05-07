﻿using Battle.Logic.CharacterCover;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.CharacterCover
{
    [TestClass]
    [TestCategory("L0")]
    public class CoverEastTests
    {
        // In the diagrams
        // "□" is open space
        // "■" is cover/an object
        // "S" is the player
        // "E" is the enemy/opponent
        // "E" will look at "S", and the cover system will determine if "S" is in cover or not
        // If a player is not in cover, they are 'flanked', and vulnerable

        [TestMethod]
        public void Test_WithEastCover_2_OClock_Enemy()
        {
            // Arrange
            //  In Cover
            //  □ □ □ E 
            //  □ S ■ □ 
            //  □ □ □ □
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(3, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
        }

        [TestMethod]
        public void Test_WithEastCover_3_OClock_Enemy()
        {
            // Arrange
            //  In Cover
            // 2 □ □ □ □ 
            // 1 □ S ■ E 
            // 0 □ □ □ □
            //   0 1 2 3
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(3, 0, 1));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
        }

        [TestMethod]
        public void Test_WithEastCover_4_OClock_Enemy()
        {
            // Arrange
            //  In Cover
            //  □ □ □ □ 
            //  □ S ■ □ 
            //  □ □ □ E
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(3, 0, 0));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
        }

        [TestMethod]
        public void Test_WithEastCover_5_OClock_Enemy_Flanking()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □ 
            //  □ S ■ □ 
            //  □ □ E □
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 0));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithEastCover_6_OClock_Enemy2()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □ 
            //  □ S ■ □ 
            //  □ E □ □
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(1, 0, 0));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithEastCover_7_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □ 
            //  □ S ■ □ 
            //  E □ □ □
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(3, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 0));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithEastCover_9_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □ 
            //  E S ■ □ 
            //  □ □ □ □            
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 1));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithEastCover_10_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            // 2 E □ □ □ 
            // 1 □ S ■ □ 
            // 0 □ □ □ □     
            //   0 1 2 3          
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithEastCover_12_OClock_Enemy2()
        {
            // Arrange
            //  Flanked
            //  □ E □ □ 
            //  □ S ■ □ 
            //  □ □ □ □             
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(1, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

        [TestMethod]
        public void Test_WithEastCover_1_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            // 2 □ □ E □ 
            // 1 □ S ■ □ 
            // 0 □ □ □ □   
            //   0 1 2 3          
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
        }

     }
}