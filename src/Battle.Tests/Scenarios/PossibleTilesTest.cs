using Battle.Logic.Characters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class PossibleTilesTest
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        [TestMethod]
        public void PossibleMovementTilesTest()
        {
            //Arrange
            string path = _rootPath + @"\SaveGames\Saves\Save004.json";

            //Act
            string fileContents;
            using (var streamReader = new StreamReader(path))
            {
                fileContents = streamReader.ReadToEnd();
            }
            Mission mission = GameSerialization.LoadGame(fileContents);
            Character fred = mission.Teams[0].Characters[0];
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(mission.Map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);

            //Assert
            Assert.AreEqual(339, movementPossibileTiles.Count);
        }
    }
}
