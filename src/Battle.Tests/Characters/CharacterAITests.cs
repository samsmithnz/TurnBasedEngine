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
            jethro.ActionPointsCurrent = 1;
            Team team2 = new Team()
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jethro },
                Color = "Red"
            };
            mission.Teams.Add(team2);

            //Act            
            CharacterAI ai = new CharacterAI();
            ActionResult actionResult1 = ai.CalculateAIAction(mission.Map, mission.Teams, jethro, mission.RandomNumbers);
            string mapString = ai.CreateAIMap(mission.Map);
            ActionResult actionResult2 = ai.CalculateAIAction(mission.Map, mission.Teams, jethro, mission.RandomNumbers);

            //Assert
            string mapResult = @"
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . 0 0 0 0 0 0 0 0 0 2 ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . 0 0 0 0 0 0 0 0 0 1 0 2 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . 0 0 0 0 0 0 0 0 1 □ 1 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . 0 0 0 0 0 0 0 0 0 0 1 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . 0 0 0 0 0 0 0 0 P 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . 0 0 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . 0 0 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapResult, mapString);
            string log1 = @"
Jethro is processing AI, with intelligence 25
Failed intelligence check: 25, (dice roll: 8)
";
            Assert.AreEqual(log1, actionResult1.LogString);
            Assert.AreEqual(new Vector3(15, 0, 15), actionResult1.StartLocation);
            Assert.AreEqual(new Vector3(10, 0, 19), actionResult1.EndLocation);
            //Assert.IsTrue(jethro.InFullCover);
            string log2 = @"
Jethro is processing AI, with intelligence 25
Successful intelligence check: 25, (dice roll: 81)
";
            Assert.AreEqual(log2, actionResult2.LogString);
            Assert.AreEqual(new Vector3(15, 0, 15), actionResult2.StartLocation);
            Assert.AreEqual(new Vector3(19, 0, 20), actionResult2.EndLocation);
        }

    }
}
