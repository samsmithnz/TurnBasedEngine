using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.GameController;
using Battle.Logic.Map;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace Battle.Tests.Scenarios
{
    [TestClass]
    [TestCategory("L1")]
    public class AICoverCrashTest
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        [TestMethod]
        public void AICoverStateCrashTest()
        {
            //Arrange
            string path = _rootPath + @"\SaveGames\Saves\Save005.json";

            //Act
            string fileContents;
            using (var streamReader = new StreamReader(path))
            {
                fileContents = streamReader.ReadToEnd();
            }
            Mission mission = GameSerialization.LoadGame(fileContents);
            mission.MoveToNextTurn();
            CharacterAI ai = new CharacterAI();
            AIAction aIAction = ai.CalculateAIAction(mission.Map, 
                mission.Teams, 
                mission.Teams[1].Characters[0], 
                mission.RandomNumbers);

            //Assert
            Assert.AreEqual(ActionTypeEnum.Move, aIAction.ActionType);
            Assert.AreEqual(new Vector3(19,0,19), aIAction.StartLocation);
            Assert.AreEqual(new Vector3(25,0,23), aIAction.EndLocation);            
        }
    }
}
