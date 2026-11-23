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
    public class AICrashTest
    {
        [TestMethod]
        public void AICrashesOnFirstTurnTest()
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/SaveGames/Saves/";
            Mission mission = GameSerialization.LoadGameFile(rootPath + "Save019.json");
            mission.StartMission();

            Character firstEnemy = mission.Teams[1].Characters[0];
            Character activeEnemy = mission.Teams[1].Characters[1];
            Assert.IsTrue(firstEnemy.HitpointsCurrent <= 0);

            mission.MoveToNextTurn();
            AIAction action = mission.CalculateAIAction(activeEnemy, mission.Teams[1], mission.Teams[0]);

            Assert.IsNotNull(action);
            Assert.IsFalse(string.IsNullOrEmpty(action.MapString));
            Assert.IsTrue(action.ActionPointsCost > 0);
            Assert.IsTrue(action.ActionPointsCost <= activeEnemy.ActionPointsCurrent);
        }
    }
}
