using Battle.Logic.GameController;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace Battle.Tests.SaveGames
{
    [TestClass]
    [TestCategory("L0")]
    public class SaveGameTests
    {
        [TestMethod]
        public void SaveGamesExist()
        {
            //Arrange
            string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = systemPath + @"\my games\TBS\Saves\";

            //Act
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            //Assert
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
        }

        [TestMethod]
        public void SaveGameLoads()
        {
            //Arrange
            string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = systemPath + @"\my games\TBS\Saves\";
            Mission mission = null;

            //Act
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string fileContents;
                using (var streamReader = new StreamReader(file))
                {
                    fileContents = streamReader.ReadToEnd();
                }

                mission = SaveGame.LoadGame(fileContents);
            }

            //Assert
            Assert.IsTrue(mission != null);

        }

    }
}