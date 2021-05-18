using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.MainGame;
using Battle.Logic.Weapons;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(1, characters.Count);
            Assert.AreEqual("Jeff", characters[0].Name);
        }

        [TestMethod]
        public void FredCannotSeeJeffTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,] map = MapUtility.InitializeMap(10, 10);
            map[7, 7] = "W";
            map[8, 7] = "W";
            map[9, 7] = "W";
            Team teamBaddie = new();
            teamBaddie.Characters.Add(jeff);

            //Act
            List<Character> characters = fred.GetCharactersInView(map, new List<Team> { teamBaddie });

            //Assert
            Assert.IsTrue(characters != null);
            Assert.AreEqual(0, characters.Count);
        }

    }
}