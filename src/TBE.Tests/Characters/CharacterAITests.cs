using TBE.Logic;
using TBE.Logic.Characters;
using TBE.Logic.Encounters;
using TBE.Logic.Game;
using TBE.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace TBE.Tests.Characters
{
    [TestClass]
    [TestCategory("L1")]
    public class CharacterAITests
    {
        [TestMethod]
        public void CharacterAIChoosesAnAffordableSingleMove()
        {
            Mission mission = CreateMission(1, false, out Character character, out Team opponentTeam);

            AIAction action = mission.CalculateAIAction(character, mission.Teams[0], opponentTeam);

            Assert.AreEqual(ActionTypeEnum.Move, action.ActionType);
            Assert.AreEqual(GameConstants.SINGLE_MOVE_ACTION_POINT_COST, action.ActionPointsCost);
            Assert.AreNotEqual(character.Location, action.EndLocation);
        }

        [TestMethod]
        public void CharacterAIAttacksFromCurrentPositionAndEndsTurn()
        {
            Mission mission = CreateMission(2, true, out Character character, out Team opponentTeam);

            AIAction action = mission.CalculateAIAction(character, mission.Teams[0], opponentTeam);

            Assert.AreEqual(ActionTypeEnum.StayInPositionThenAttack, action.ActionType);
            Assert.AreEqual(character.Location, action.StartLocation);
            Assert.AreEqual(character.Location, action.EndLocation);
            Assert.AreEqual(character.ActionPointsCurrent, action.ActionPointsCost);
            Assert.AreEqual(opponentTeam.Characters[0].Name, action.TargetName);
        }

        [TestMethod]
        public void CharacterAIWithEmptyWeaponChoosesMovementInsteadOfAnAttack()
        {
            Mission mission = CreateMission(1, true, out Character character, out Team opponentTeam);
            character.WeaponEquipped.AmmoCurrent = 0;

            AIAction action = mission.CalculateAIAction(character, mission.Teams[0], opponentTeam);

            Assert.AreEqual(ActionTypeEnum.Move, action.ActionType);
            Assert.AreEqual(GameConstants.SINGLE_MOVE_ACTION_POINT_COST, action.ActionPointsCost);
        }

        private static Mission CreateMission(int actionPoints, bool addOpponent, out Character character, out Team opponentTeam)
        {
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            character = CharacterPool.CreateJethroBaddie(mission.Map, new Vector3(15, 0, 15));
            character.Intelligence = GameConstants.PERCENTAGE_MAX;
            character.ActionPointsCurrent = actionPoints;

            Team sourceTeam = new Team(0)
            {
                Name = "Source team",
                Characters = new() { character },
                Color = "Red"
            };
            opponentTeam = new Team(1)
            {
                Name = "Opponent team",
                Characters = new(),
                Color = "Blue"
            };

            if (addOpponent)
            {
                opponentTeam.Characters.Add(CharacterPool.CreateFredHero(mission.Map, new Vector3(16, 0, 15)));
            }

            mission.Teams.Add(sourceTeam);
            mission.Teams.Add(opponentTeam);
            return mission;
        }
    }
}
