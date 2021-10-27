using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
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
        public void CharacterJethroAIMovesIntoFullCoverNoOpponentsTest()
        {
            //Arrange
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[20, 0, 20] = CoverType.FullCover;
            mission.Map[18, 0, 18] = CoverType.HalfCover;
            Team team1 = new Team(1)
            {
                Name = "Good guys",
                Characters = new List<Character>() { },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new Vector3(15, 0, 15));
            jethro.ActionPointsCurrent = 2;
            Team team2 = new Team(0)
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jethro },
                Color = "Red"
            };
            mission.Teams.Add(team2);

            //Act            
            //Scenario 1: Failure
            AIAction actionResult1 = mission.CalculateAIAction(jethro, team2, team1);
            //Scenario 2: Success
            AIAction actionResult2 = mission.CalculateAIAction(jethro, team2, team1);
            string mapString = actionResult2.MapString;

            //Assert
            string mapStringExpected = @"
. . . . . . . . . . . . . . . 1 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . 
. . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . 
. . . 1 1 1 1 1 1 1 1 1 1 1 1 2 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . 
. . 1 1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 4 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 5 ■ 4 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 4 2 5 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . 
. 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 4 □ 4 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . 
1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 4 2 2 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . 
1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . 
1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 P 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . 
1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . 
1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . 
. 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . 
. 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . 
. . 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . 1 1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . . 1 1 1 1 1 1 1 1 1 1 1 1 2 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . 
. . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . 
. . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . 
. . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapStringExpected, mapString);
            string log1 = @"
Jethro is processing AI, with intelligence 25
Failed intelligence check: 25, (dice roll: 8)
";
            Assert.AreEqual(log1, actionResult1.LogString);
            Assert.AreEqual(4, actionResult1.Score);
            Assert.AreEqual(new Vector3(15, 0, 15), actionResult1.StartLocation);
            Assert.AreEqual(new Vector3(17, 0, 18), actionResult1.EndLocation);
            Assert.AreEqual(ActionTypeEnum.DoubleMove, actionResult1.ActionType);

            string log2 = @"
Jethro is processing AI, with intelligence 25
Successful intelligence check: 25, (dice roll: 81)
";
            Assert.AreEqual(log2, actionResult2.LogString);
            Assert.AreEqual(new Vector3(15, 0, 15), actionResult2.StartLocation);
            Assert.AreEqual(new Vector3(19, 0, 20), actionResult2.EndLocation);
            Assert.AreEqual(ActionTypeEnum.DoubleMove, actionResult2.ActionType);
        }


        [TestMethod]
        public void CharacterJethroAIMovesIntoFullCoverWithOpponentTest()
        {
            //Arrange
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[5, 0, 6] = CoverType.FullCover;
            mission.Map[14, 0, 5] = CoverType.HalfCover; //half cover here!
            mission.Map[14, 0, 6] = CoverType.HalfCover; //half cover here!
            mission.Map[14, 0, 7] = CoverType.HalfCover; //half cover here!
            mission.Map[14, 0, 8] = CoverType.FullCover;
            mission.Map[14, 0, 9] = CoverType.FullCover;
            mission.Map[14, 0, 10] = CoverType.FullCover;
            mission.Map[14, 0, 11] = CoverType.FullCover;
            mission.Map[14, 0, 12] = CoverType.FullCover;
            mission.Map[14, 0, 13] = CoverType.FullCover;
            mission.Map[14, 0, 14] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(5, 0, 5));
            Team team1 = new Team(1)
            {
                Name = "Good guys",
                Characters = new List<Character>() { fred }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new Vector3(15, 0, 10));
            jethro.HitpointsCurrent = 5;
            Team team2 = new Team(0)
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jethro }
            };
            mission.Teams.Add(team2);
            //Remove the 8, so that the 81 is primed and ready
            mission.RandomNumbers.Dequeue();

            //Act            
            AIAction actionResult = mission.CalculateAIAction(jethro, team2, team1);
            string mapString = actionResult.MapString;

            //Assert         
            string mapStringExpected = @"
. . . . . . . . . . . . . . . 1 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . 1 1 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . 1 1 1 1 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . 1 1 1 1 1 1 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . 1 1 1 1 1 1 1 0 0 0 0 0 0 0 0 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 1 1 1 1 1 0 0 0 0 0 0 0 0 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 1 1 1 1 1 0 0 0 0 0 0 0 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 1 1 1 1 0 0 0 0 0 0 0 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . 
. . . . . 1 1 1 1 1 1 1 0 0 0 0 0 0 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . 
. . . . . 1 1 1 1 1 1 0 0 0 0 0 0 0 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . . . 1 1 1 1 1 1 1 0 0 0 0 0 0 0 0 0 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . . . 1 1 1 1 1 1 0 0 0 0 2 0 0 0 0 0 0 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . . . 1 1 1 1 1 1 0 0 0 0 ■ 0 0 0 0 0 0 0 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . 
. . . . . 1 1 1 1 0 0 0 0 0 ■ 0 0 0 0 0 0 0 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . 
. . . . . 1 1 1 1 0 0 0 0 0 ■ 0 0 0 0 0 0 0 0 1 1 1 1 1 1 0 0 . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 0 0 0 0 0 . ■ 0 0 0 0 0 0 0 0 1 1 1 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 0 0 0 0 0 . ■ P 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . 
. . . . . . . 0 0 0 0 0 0 . ■ 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . 
. . . . . . . 0 0 0 0 0 0 . ■ 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . 
. . . . . . 0 0 0 0 0 0 0 0 □ 1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . . . . . . 
. . . . . ■ 1 1 1 1 1 1 1 1 □ 3 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . 
. . . . . P 1 1 1 1 1 1 1 1 □ 3 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . 
";
            Assert.AreEqual(mapStringExpected, mapString);
            string log1 = @"
Jethro is processing AI, with intelligence 25
Successful intelligence check: 25, (dice roll: 81)
";
            Assert.AreEqual(log1, actionResult.LogString);
            Assert.AreEqual(new Vector3(15, 0, 10), actionResult.StartLocation);
            Assert.AreEqual(new Vector3(15, 0, 6), actionResult.EndLocation);
            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, actionResult.ActionType);
        }

    }
}
