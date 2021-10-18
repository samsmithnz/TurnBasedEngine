using Battle.Logic.Characters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class CharacterFieldOfViewTests
    {
        [TestMethod]
        public void FredCanSeeJethroTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
            //List<Character> characters = fred.GetCharactersInRangeWithCurrentWeapon(map, teamBaddie.Characters);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            Assert.AreEqual("Jethro", fred.TargetCharacters[0].Name);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
P o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void FredCanSeeJethroInAngleTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = CoverType.FullCover;
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
            string fovMapString = MapCore.GetMapStringWithMapMask(map, fred.FOVMap);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o . 
o o o o o o o o P . 
o o o o o o o ■ ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
P o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);

            string expectedFOV = @"
. . . . . . . . . ▓ 
. . . . . . . . ▓ ▓ 
. . . . . . . ■ ■ ■ 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
";
            Assert.AreEqual(expectedFOV, fovMapString);
        }

        [TestMethod]
        public void FredCannotSeeJethroTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = CoverType.FullCover;
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(8, 0, 0));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
            string fovMapString = MapCore.GetMapStringWithMapMask(map, fred.FOVMap);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(0, fred.TargetCharacters.Count);
            string expected = @"
o o o o o o o . . . 
o o o o o o o . P . 
o o o o o o o ■ ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o P o 
";
            Assert.AreEqual(expected, mapString);

            string expectedFOV = @"
. . . . . . . ▓ ▓ ▓ 
. . . . . . . ▓ ▓ ▓ 
. . . . . . . ■ ■ ■ 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . P . 
";
            Assert.AreEqual(expectedFOV, fovMapString);
        }

        [TestMethod]
        public void FredCanSeeJethroInNorthCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(8, 0, 0));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
          
            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o . . 
o o o o o o o o P . 
o o o o o o o o ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o P o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }



        [TestMethod]
        public void FredCanSeeJethroInSouthCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 2] = CoverType.FullCover;
            map[9, 0, 2] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(8, 0, 8));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 1));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
           
            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o ■ ■ 
o o o o o o o o P . 
o o o o o o o o . . 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }



        [TestMethod]
        public void FredCanSeeJethroInEastCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(1, 0, 7));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(9, 0, 7));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
           
            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o o o 
o P o o o o o o ■ P 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }


        [TestMethod]
        public void FredCanSeeJethroInWestCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[1, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(9, 0, 7));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(0, 0, 7));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
          
            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o o o 
P ■ o o o o o o o P 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void JethroCannotSeeFredTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = CoverType.FullCover;
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            Team teamGood = new Team();
            teamGood.Characters.Add(fred);

            //Act
            string mapString = jethro.GetCharactersInViewMapString(map, teamGood.Characters);
           
            //Assert
            Assert.IsTrue(jethro.TargetCharacters != null);
            Assert.AreEqual(0, jethro.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o ■ ■ ■ 
o o o o o . . . . . 
o o . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
P . . . . . . . . . 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void JethroCanSeeFredTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = CoverType.HalfCover;
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(0, 0, 0));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(8, 0, 8));
            Team teamGood = new Team();
            teamGood.Characters.Add(fred);

            //Act
            string mapString = jethro.GetCharactersInViewMapString(map, teamGood.Characters);
         
            //Assert
            Assert.IsTrue(jethro.TargetCharacters != null);
            Assert.AreEqual(1, jethro.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o □ ■ ■ 
o o o o o o o o . . 
o o o o o o o . . . 
o o o o o o o . . . 
o o o o o o . . . . 
o o o o o o . . . . 
o o o o o . . . . . 
P o o o o . . . . . 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void FredShouldSeeJethroInCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 5] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(2, 0, 2));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(2, 0, 6));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
         
            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o . . . o o o o o o 
o . . . o o o o o o 
o o . o o o o o o o 
o o P o o o o o o o 
o o ■ o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o P o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void FredSimpleRange3Test()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(11, 1, 11);
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(5, 0, 5));
            fred.ShootingRange = 3;

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Character>());
          
            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(0, fred.TargetCharacters.Count);
            string mapStringExpected = @"
. . . . . . . . . . . 
. . . . . . . . . . . 
. . . o o o o o . . . 
. . o o o o o o o . . 
. . o o o o o o o . . 
. . o o o P o o o . . 
. . o o o o o o o . . 
. . o o o o o o o . . 
. . . o o o o o . . . 
. . . . . . . . . . . 
. . . . . . . . . . . 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void FredCanFlankJethroInNorthCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 2] = CoverType.FullCover;
            map[4, 0, 2] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new Vector3(5, 0, 2));
            Character jethro = CharacterPool.CreateJethroBaddie(map, new Vector3(2, 0, 1));
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jethro);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, teamBaddie.Characters);
          
            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
. o o o o o o o o o 
. . . o o o o o o o 
. . ■ . ■ P o o o o 
. . P o o o o o o o 
. o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);

            //Now test cover for each character
            
            //Act
            CoverState coverStateResultPlayer = CharacterCover.CalculateCover(map, fred.Location, new List<Vector3>() { jethro.Location });
            CoverState coverStateResultEnemy = CharacterCover.CalculateCover(map, jethro.Location, new List<Vector3>() { fred.Location });

            //Assert
            Assert.IsTrue(coverStateResultPlayer.InFullCover);
            Assert.IsTrue(coverStateResultPlayer.InWestFullCover);
            Assert.IsTrue(!coverStateResultEnemy.InFullCover);
            Assert.IsTrue(!coverStateResultEnemy.InSouthFullCover);
        }
    }
}