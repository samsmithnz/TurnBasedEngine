﻿using Battle.Logic.CharacterCover;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.CharacterCover
{
    [TestClass]
    [TestCategory("L0")]
    public class CoverBaseTests
    {
        // In the diagrams
        // "□" is open space
        // "■" is cover/an object
        // "S" is the player
        // "E" is the enemy/opponent
        // "E" will look at "S", and the cover system will determine if "S" is in cover or not
        // If a player is not in cover, they are 'flanked', and vulnerable

        [TestMethod]
        public void Test_WithoutCover_NoEnemy()
        {
            // Arrange
            //  □ □ □ 
            //  □ S □ 
            //  □ □ □ 
            Vector3 startingLocation = new(1,0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> enemyLocations = null;

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  null);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
            Assert.IsTrue(coverResult.InNorthCover == false);
            Assert.IsTrue(coverResult.InEastCover == false);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == false);
        }

        [TestMethod]
        public void Test_WithoutCover_NoEnemy_TinyMap()
        {
            // Arrange
            //  S 
            Vector3 startingLocation = new (0,0, 0);
            int width = 1;
            int height = 1;
            List<Vector3> enemyLocations = null;

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  null);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
            Assert.IsTrue(coverResult.InNorthCover == false);
            Assert.IsTrue(coverResult.InEastCover == false);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == false);
        }


        [TestMethod]
        public void Test_WithNorthCover_NoEnemy()
        {
            // Arrange
            //  □ ■ □ 
            //  □ S □
            //  □ □ □ 
            Vector3 startingLocation = new (1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> coverLocations = new ();
            coverLocations.Add(new (1, 0, 2));
            List<Vector3> enemyLocations = null;

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == false);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == false);
        }

        [TestMethod]
        public void Test_WithEastCover_NoEnemy()
        {
            // Arrange
            //  □ □ □ 
            //  □ S ■ 
            //  □ □ □ 
            Vector3 startingLocation = new (1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new (2, 0, 1));
            List<Vector3> enemyLocations = null;

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == false);
            Assert.IsTrue(coverResult.InEastCover == true);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == false);
        }

        [TestMethod]
        public void Test_WithSouthCover_NoEnemy()
        {
            // Arrange
            //  □ □ □ 
            //  □ S □ 
            //  □ ■ □ 
            Vector3 startingLocation = new (1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new (1, 0, 0));
            List<Vector3> enemyLocations = null;

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == false);
            Assert.IsTrue(coverResult.InEastCover == false);
            Assert.IsTrue(coverResult.InSouthCover == true);
            Assert.IsTrue(coverResult.InWestCover == false);
        }

        [TestMethod]
        public void Test_WithWestCover_NoEnemy()
        {
            // Arrange
            //  □ □ □ 
            //  ■ S □ 
            //  □ □ □ 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(0, 0, 1));
            List<Vector3> enemyLocations = null;

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height,  coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == false);
            Assert.IsTrue(coverResult.InEastCover == false);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == true);
        }

    }
}