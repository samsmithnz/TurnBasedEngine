using Battle.Logic.GameController;
using Battle.Logic.Map;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L1")]
    public class TeamFieldOfViewTests
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        [TestMethod]
        public void TeamTestMultipleCharactersFOVTest()
        {
            //Arrange
            string path = _rootPath + @"\SaveGames\Saves\SaveFOV002.json";

            //Act
            string fileContents;
            using (var streamReader = new StreamReader(path))
            {
                fileContents = streamReader.ReadToEnd();
            }
            Mission mission = GameSerialization.LoadGame(fileContents);

            //Assert
            Assert.IsNotNull(mission);
        }

    }
}
