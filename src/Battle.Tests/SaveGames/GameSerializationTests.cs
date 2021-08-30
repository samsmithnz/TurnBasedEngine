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

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            int number = new DirectoryInfo(path).GetFiles().Length;
            string fileName = $"Save{number:000}.json";
            while (File.Exists(path + fileName) == true)
            {
                number++;
                fileName = $"Save{number:000}.json";
                if (number > 1000)
                {
                    //we don't want an infinite loop
                    break;
                }
            }
            File.WriteAllText(path + fileName, json);

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
        }

    }
}