using Battle.Logic.CharacterCover;
using Battle.Logic.Characters;
using Battle.Logic.FieldOfView;
using Battle.Logic.MainGame;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.FieldOfView
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class CharacterFieldOfViewTests
    {
        [TestMethod]
        public void FredCanSeeJeffTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            Assert.AreEqual("Jeff", characters[0].Name);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredCanSeeJeffInAngleTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[7, 7] = CoverType.FullCover;
            map[8, 7] = CoverType.FullCover;
            map[9, 7] = CoverType.FullCover;
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
           string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
o o o o o o o o o □ 
o o o o o o o o P □ 
o o o o o o o ■ ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
P o o o o o o o o o 
";
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredCannotSeeJeffTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(8, 0, 0);
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[7, 7] = CoverType.FullCover;
            map[8, 7] = CoverType.FullCover;
            map[9, 7] = CoverType.FullCover;
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(0, characters.Count);
            string mapResult = @"
o o o o o o o □ □ □ 
o o o o o o o □ P □ 
o o o o o o o ■ ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o P o 
";
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredCanSeeJeffInNorthCoverTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(8, 0, 0);
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[8, 7] = CoverType.FullCover;
            map[9, 7] = CoverType.FullCover;
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
o o o o o o o o □ □ 
o o o o o o o o P □ 
o o o o o o o o ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o P o 
";
            Assert.AreEqual(mapResult, mapString);
        }



        [TestMethod]
        public void FredCanSeeJeffInSouthCoverTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(8, 0, 8);
            Character jeff = CharacterPool.CreateJeffBaddie();
            jeff.Location = new(8, 0, 1);
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[8, 2] = CoverType.FullCover;
            map[9, 2] = CoverType.FullCover;
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o ■ ■ 
o o o o o o o o P □ 
o o o o o o o o □ □ 
";
            Assert.AreEqual(mapResult, mapString);
        }



        [TestMethod]
        public void FredCanSeeJeffInEastCoverTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(1, 0, 7);
            Character jeff = CharacterPool.CreateJeffBaddie();
            jeff.Location = new(9, 0, 7);
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[8, 7] = CoverType.FullCover;
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }


        [TestMethod]
        public void FredCanSeeJeffInWestCoverTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(9, 0, 7); 
            Character jeff = CharacterPool.CreateJeffBaddie();
            jeff.Location = new(0, 0, 7);
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[1, 7] = CoverType.FullCover;
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void JeffCannotSeeFredTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[7, 7] = CoverType.FullCover;
            map[8, 7] = CoverType.FullCover;
            map[9, 7] = CoverType.FullCover;
            Team teamGood = new();
            teamGood.Characters.Add(fred);

            //Act
            string mapString = jeff.GetCharactersInViewMapString(map, new List<Team> { teamGood });
            List<Character> characters = jeff.GetCharactersInView(map, new List<Team> { teamGood });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(0, characters.Count);
            string mapResult = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o ■ ■ ■ 
o o o o o □ □ □ □ □ 
o o □ □ □ □ □ □ □ □ 
□ □ □ □ □ □ □ □ □ □ 
□ □ □ □ □ □ □ □ □ □ 
□ □ □ □ □ □ □ □ □ □ 
□ □ □ □ □ □ □ □ □ □ 
P □ □ □ □ □ □ □ □ □ 
";
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredShouldSeeJeffInCoverTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new Vector3(2, 0, 2);
            Character jeff = CharacterPool.CreateJeffBaddie();
            jeff.Location = new Vector3(2, 0, 6);
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[2, 5] = CoverType.FullCover;
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
o □ □ □ o o o o o o 
o □ □ □ o o o o o o 
o o □ o o o o o o o 
o o P o o o o o o o 
o o ■ o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o P o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
";
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredSimpleRange3Test()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            fred.Location = new(5, 0, 5);
            fred.Range = 3;
            string[,] map = MapUtility.InitializeMap(11, 11);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team>());
            List<Character> characters = fred.GetCharactersInView(map, new List<Team>());

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(0, characters.Count);
            string mapResult = @"
□ □ □ □ □ □ □ □ □ □ □ 
□ □ □ □ □ □ □ □ □ □ □ 
□ □ □ o o o o o □ □ □ 
□ □ o o o o o o o □ □ 
□ □ o o o o o o o □ □ 
□ □ o o o P o o o □ □ 
□ □ o o o o o o o □ □ 
□ □ o o o o o o o □ □ 
□ □ □ o o o o o □ □ □ 
□ □ □ □ □ □ □ □ □ □ □ 
□ □ □ □ □ □ □ □ □ □ □ 
";
            Assert.AreEqual(mapResult, mapString);
        }

    }
}