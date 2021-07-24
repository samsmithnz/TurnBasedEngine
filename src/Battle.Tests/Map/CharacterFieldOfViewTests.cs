using Battle.Logic.Characters;
using Battle.Logic.GameController;
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
        public void FredCanSeeJeffTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Team teamBaddie = new Team();
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
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = CoverType.FullCover;
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredCannotSeeJeffTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = CoverType.FullCover;
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(8, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(0, characters.Count);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredCanSeeJeffInNorthCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(8, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }



        [TestMethod]
        public void FredCanSeeJeffInSouthCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 2] = CoverType.FullCover;
            map[9, 0, 2] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(8, 0, 8), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(8, 0, 1), map);
            Team teamBaddie = new Team();
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
o o o o o o o o P . 
o o o o o o o o . . 
";
            Assert.AreEqual(mapResult, mapString);
        }



        [TestMethod]
        public void FredCanSeeJeffInEastCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(1, 0, 7), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(9, 0, 7), map);
            Team teamBaddie = new Team();
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
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[1, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(9, 0, 7), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(0, 0, 7), map);
            Team teamBaddie = new Team();
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
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = CoverType.FullCover;
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Team teamGood = new Team();
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
o o o o o . . . . . 
o o . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
. . . . . . . . . . 
P . . . . . . . . . 
";
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void JeffCanSeeFredTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = CoverType.HalfCover;
            map[8, 0, 7] = CoverType.FullCover;
            map[9, 0, 7] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Team teamGood = new Team();
            teamGood.Characters.Add(fred);

            //Act
            string mapString = jeff.GetCharactersInViewMapString(map, new List<Team> { teamGood });
            List<Character> characters = jeff.GetCharactersInView(map, new List<Team> { teamGood });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredShouldSeeJeffInCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 5] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 2), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(2, 0, 6), map);
            Team teamBaddie = new Team();
            teamBaddie.Characters.Add(jeff);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team> { teamBaddie });
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }

        [TestMethod]
        public void FredSimpleRange3Test()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(11, 1, 11);
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(5, 0, 5), map); ;
            fred.ShootingRange = 3;

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new List<Team>());
            List<Character> characters = fred.GetCharactersInView(map, new List<Team>());

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(0, characters.Count);
            string mapResult = @"
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
            Assert.AreEqual(mapResult, mapString);
        }

    }
}