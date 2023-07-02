using TBE.Logic.Game;
using TBE.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace TBE.Tests.SaveGames
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
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\SaveGames\Saves\";
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
        public void SaveNewGameWithNumberTest()
        {
            //Arrange
            Mission mission = new();
            string path = _rootPath;

            //Act
            string json = GameSerialization.SaveGame(mission);
            string savedFile = GameSerialization.CreateSaveGameFile(path, json, 1);

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);

            //Clean up
            File.Delete(savedFile);
        }

        [TestMethod]
        public void SaveNewGameTest()
        {
            //Arrange
            Mission mission = new();
            string path = _rootPath;

            //Act
            string json = GameSerialization.SaveGame(mission);
            string savedFile = GameSerialization.CreateSaveGameFile(path, json);

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);

            //Clean up
            File.Delete(savedFile);
        }

        [TestMethod]
        public void SaveNewGameTwiceTest()
        {
            //Arrange
            Mission mission = new();
            string path = _rootPath;

            //Act
            string json = GameSerialization.SaveGame(mission);
            string savedFile = GameSerialization.CreateSaveGameFile(path, json, 9);
            string savedFile2 = GameSerialization.CreateSaveGameFile(path, json, 9);

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
            Assert.AreEqual(path + "Save009.json", savedFile);
            Assert.AreEqual(path + "Save010.json", savedFile2);

            //Clean up
            File.Delete(savedFile);
            File.Delete(savedFile2);
        }

        [TestMethod]
        public void SaveNewGameTooLargeTest()
        {
            //Arrange
            Mission mission = new();
            string path = _rootPath;

            //Act
            string json = GameSerialization.SaveGame(mission);
            string savedFile = GameSerialization.CreateSaveGameFile(path, json, 1001);
            string savedFile2 = GameSerialization.CreateSaveGameFile(path, json, 1001);

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
            Assert.AreEqual(path + "Save1001.json", savedFile);
            Assert.AreEqual(path + "Save1002.json", savedFile2);

            //Clean up
            File.Delete(savedFile);
            File.Delete(savedFile2);
        }

        [TestMethod]
        public void SaveNewGameNewDirectoryTest()
        {
            //Arrange
            Mission mission = new();
            string path = _rootPath + "\\NewSaveDir";

            //Act
            string json = GameSerialization.SaveGame(mission);
            string savedFile = GameSerialization.CreateSaveGameFile(path, json, 12);

            //Assert
            Assert.IsTrue(Directory.Exists(path));
            Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
            Assert.AreEqual(path + "Save012.json", savedFile);

            //Clean up
            File.Delete(savedFile);
            Directory.Delete(path);
        }

        [TestMethod]
        public void LoadGameWithNoContentTest()
        {
            //Arrange
            Mission mission = new();
            string path = _rootPath;

            //Act
            string json = "";
            string savedFile = GameSerialization.CreateSaveGameFile(path, json, 15);
            //This should throw an exception
            try
            {
                GameSerialization.LoadGameFile(savedFile);
            }
            catch (System.IO.FileNotFoundException)
            {
                //Assert
                Assert.IsTrue(Directory.Exists(path));
                Assert.IsTrue(Directory.GetFiles(path).Length >= 0);
                Assert.AreEqual(path + "Save015.json", savedFile);
            }
            catch (System.Exception)
            {
                //Should never be here
                Assert.IsTrue(false);
            }
            finally
            {
                //Clean up
                File.Delete(savedFile);
            }
        }

    }
}