using Battle.Logic.GameController;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Battle.Tests.SaveGames
{
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
        public void SaveGameLoads()
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
        public void SaveNewGameExist()
        {
            //Arrange
            Mission mission = new Mission();
            string path = _rootPath + @"\SaveGames\Saves\";

            //Act

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
        }

    }
}