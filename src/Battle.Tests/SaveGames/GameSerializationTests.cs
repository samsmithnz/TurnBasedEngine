using Battle.Logic.GameController;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace Battle.Tests.SaveGames
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class GameSerializationTests
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        [TestMethod]
        public void SaveGamesExist()
        {
            //Arrange
            string path = _rootPath + @"\SaveGames\Saves\";

            //Act

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
        }

        [TestMethod]
        public void LoadGameTest()
        {
            //Arrange
            string path = _rootPath + @"\SaveGames\Saves\";
            Mission mission = null;

            //Act
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string fileContents;
                using (var streamReader = new StreamReader(file))
                {
                    fileContents = streamReader.ReadToEnd();
                }

                mission = GameSerialization.LoadGame(fileContents);
            }

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(mission != null);
        }

        [TestMethod]
        public void SaveNewGameTest()
        {
            //Arrange
            Mission mission = new Mission();
            string path = _rootPath + @"\SaveGames\Saves\";

            //Act
            string json = GameSerialization.SaveGame(mission);
            GameSerialization.CreateSaveGameFile(path, json);

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
        }

    }
}