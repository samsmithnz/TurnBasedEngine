using TBE.Logic.Characters;
using TBE.Logic.Encounters;
using TBE.Logic.Game;
using TBE.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace TBE.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class AITargetingTest
    {
        [TestMethod]
        public void AIChoosesAnAffordableIndependentAction()
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/SaveGames/Saves/";
            Mission mission = GameSerialization.LoadGameFile(rootPath + "Save016.json");
            mission.StartMission();
            mission.MoveToNextTurn();

            Character character = mission.Teams[1].Characters[0];
            AIAction action = mission.CalculateAIAction(character, mission.Teams[1], mission.Teams[0]);

            Assert.AreNotEqual(ActionTypeEnum.MoveThenAttack, action.ActionType);
            Assert.AreNotEqual(ActionTypeEnum.MoveThenOverwatch, action.ActionType);
            Assert.AreEqual(character.Location, action.StartLocation);

            if (action.ActionType == ActionTypeEnum.Move || action.ActionType == ActionTypeEnum.DoubleMove)
            {
                Assert.IsTrue(action.ActionPointsCost > 0);
                Assert.IsTrue(action.ActionPointsCost <= character.ActionPointsCurrent);
                Assert.AreNotEqual(character.Location, action.EndLocation);
            }
            else
            {
                Assert.IsTrue(action.ActionType == ActionTypeEnum.StayInPositionThenAttack || action.ActionType == ActionTypeEnum.Overwatch);
                Assert.AreEqual(character.Location, action.EndLocation);
                Assert.AreEqual(character.ActionPointsCurrent, action.ActionPointsCost);
            }
        }
    }
}
