using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.GameController;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterAITests
    {

        [TestMethod]
        public void CharacterJethroAIMovesIntoFullCoverTest()
        {
            //Arrange
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[20, 0, 20] = CoverType.FullCover;
            mission.Map[18, 0, 18] = CoverType.HalfCover;
            Team team1 = new Team()
            {
                Name = "Good guys",
                Characters = new List<Character>() { },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(null, new Vector3(8, 0, 8));
            jethro.SetLocation(new Vector3(15, 0, 15), mission.Map);
            Team team2 = new Team()
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jethro },
                Color = "Red"
            };
            mission.Teams.Add(team2);

            //Act            
            ActionResult actionResult = CharacterAI.CalculateAction(mission.Map, mission.Teams, jethro, mission.RandomNumbers);

            //Assert
            Assert.AreEqual(new Vector3(15, 0, 15), actionResult.StartLocation);
            Assert.AreEqual(new Vector3(18, 0, 19), actionResult.EndLocation);
            Assert.IsTrue(jethro.InFullCover);
        }

    }
}
