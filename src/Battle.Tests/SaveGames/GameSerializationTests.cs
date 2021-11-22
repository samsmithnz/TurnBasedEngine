using Battle.Logic.Game;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace Battle.Tests.SaveGames
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class GameSerializationTests
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/SaveGames/Saves/";
        }

        [TestMethod]
        public void SaveGamesExist()
        {
            //Arrange
            string path = _rootPath;

            //Act

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
        }

        [TestMethod]
        public void LoadGameTest()
        {
            //Arrange
            string path = _rootPath;
            Mission mission = null;

            //Act
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                mission = GameSerialization.LoadGameFile(file);
            }

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(mission != null);
        }

        [TestMethod]
        public void SaveNewGameTest()
        {
            //Arrange
            Mission mission = new();
            string path = _rootPath;

            //Act
            string json = GameSerialization.SaveGame(mission);
        string savedFile=    GameSerialization.CreateSaveGameFile(path, json);

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);

            //Clean up
            File.Delete(savedFile);
        }

    }
}