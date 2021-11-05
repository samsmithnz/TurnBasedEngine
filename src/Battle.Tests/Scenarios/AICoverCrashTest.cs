using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class AICoverCrashTest
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/SaveGames/Saves/";
        }

        [TestMethod]
        public void AICoverStateCrashTest()
        {
            //Arrange
            string path = _rootPath + "Save021.json";

            //Act
            Mission mission = GameSerialization.LoadGameFile(path);
            mission.StartMission();
            Character fred = mission.Teams[0].Characters[0];

            //Assert
            Assert.AreEqual("Fred", fred.Name);
            Assert.AreEqual(true, fred.CoverState.InFullCover);
            Assert.AreEqual(true, fred.CoverState.InNorthFullCover);
        }

        [TestMethod]
        public void SaveFileNotFoundCrashTest()
        {
            //Arrange
            string path = _rootPath + "SaveXXX.json";

            //Act
            try
            {
                Mission mission = GameSerialization.LoadGameFile(path);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                //Assert
                Assert.AreEqual("Could not find file '" + path.Replace("/", "\\") + "'.", ex.Message);
            }
        }
    }
}
