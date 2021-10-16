﻿using Battle.Logic.Characters;
using Battle.Logic.Encounters;
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
    [TestClass]
    [TestCategory("L1")]
    public class AINoTargetCrashTest
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
            string path = _rootPath + @"\SaveGames\Saves\Save008.json";

            //Act
            string fileContents;
            using (var streamReader = new StreamReader(path))
            {
                fileContents = streamReader.ReadToEnd();
            }
            Mission mission = GameSerialization.LoadGame(fileContents);

            //Move to enemy turn
            mission.MoveToNextTurn();

            //process AI for character 1
            CharacterAI ai = new CharacterAI();
            AIAction aIAction = ai.CalculateAIAction(mission.Map,
                mission.Teams,
                mission.Teams[1].Characters[0],
                mission.RandomNumbers);
            string mapString = ai.CreateAIMap(mission.Map);
            string mapStringExpected = @"
. . . . . . . . . . . □ . . ■ . . □ . . . . . . . . □ . . . . . ■ . . . . . . . . . ■ . . . ■ . . . 
. . . . . . . . . . . . . . . . . . . . ■ . . . ■ . . . . . . . . . . . . . . . . . . ■ . . . . □ . 
. . . . . . . . . . . . . . . . ■ . . . . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . ■ . . . . . . . . ■ . . . . . . . . . □ ■ . . . . . . . . . . . . . . . . . . . 
. . . ■ . . ■ . . . □ . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . □ . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . □ . . . . . . . . ■ . . . . . . 
. . . . . □ . . . . . . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . □ . . . . . . □ . 
. . . . ■ . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . □ . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . ■ . . □ . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . □ . . . . □ . . □ . . . . . . . . . ■ . . . . . . . . . 
. . . . . . . . . . . □ . . . . □ . . . . . . . . . . . . . . . . . . . . □ . . . . . . ■ . . . . . 
. . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . ■ . . . . . . . . . . . 
. . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . 1 ■ . . . ■ . . . . . . ■ . . . ■ . . . . . . . . . . . . . . 
. . . . . . ■ . . . . . . ■ . . . 1 1 0 0 0 . . . . . □ . . . . . . . . . . . . . . . . . . . . . . 
. □ . . . . . . . . . . . . . 1 1 1 0 0 1 1 0 0 . . . . . . ■ . . . . . . . . . . . ■ . . ■ . . . . 
. . . . . . . . . . . . 1 1 1 1 1 0 0 0 □ 1 0 0 2 ■ P □ . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . ■ . 0 2 1 1 1 1 1 0 0 1 □ 1 0 0 □ 4 1 1 1 . □ . . . . . . . . ■ . □ . . . . . . . . 
. . . . □ . □ . . 0 0 ■ 3 1 1 1 0 0 1 0 0 0 0 1 1 0 0 1 1 1 1 . . . . . . . . . . ■ . . . . . . . . 
. . . . . . . . 0 0 1 3 3 1 1 0 0 0 □ 1 0 0 1 1 1 3 1 0 0 1 3 . . . . . . . . . . . . . . . . . □ . 
. . . . . . . 0 0 0 4 3 3 1 0 1 1 0 0 0 0 1 1 0 □ □ 1 0 0 1 □ 3 . . . . . . . . . . . . . . . ■ . . 
□ . . . . . . 0 0 0 ■ 3 1 0 0 1 0 1 0 0 1 0 0 1 0 0 0 0 ■ 2 0 □ . ■ . . . ■ . . . ■ . . . . . . ■ . 
. . . . . . 1 0 0 0 1 3 1 0 1 1 1 1 6 1 0 1 1 0 0 0 0 0 0 0 0 2 1 . . . . . . . □ . . □ . . . . . . 
. . . . . . 1 0 0 0 3 1 0 2 3 2 1 3 ■ 4 1 2 2 0 0 1 1 0 0 0 0 ■ 4 . ■ . . . □ . □ . . . . . . . . . 
. . . . . . . 1 0 2 3 0 2 ■ 2 1 3 1 3 1 3 3 6 0 0 1 □ 1 0 0 2 4 1 . . . . . . . . . . . . . . . . . 
. . . . . 1 0 □ 2 ■ 1 0 □ 2 6 1 6 0 2 2 3 6 ■ 6 1 0 0 1 0 0 ■ ■ 2 1 . . . □ . . . . . . . . . . ■ . 
. . . . . 1 0 0 0 ■ 0 0 0 2 ■ 3 ■ 4 2 3 3 ■ 4 3 5 0 0 □ 1 0 0 0 ■ 2 . . . . . □ . . . □ . . . . . . 
□ . . . □ 1 0 0 0 0 0 0 2 2 3 3 2 2 3 3 3 3 3 5 1 0 0 0 0 0 0 1 0 0 0 . . . . . . . . . ■ . . . . . 
. . . . 0 0 0 0 0 0 0 0 2 3 1 2 2 3 5 3 5 3 5 3 2 2 2 0 0 0 0 0 0 0 0 . . . . . . . . . . . . . . . 
■ . . 1 1 0 0 0 0 0 0 2 2 3 2 3 3 5 3 P □ 7 3 2 2 2 3 0 0 0 0 0 1 0 0 . . . . . . . . . . . . . . . 
. ■ . . 1 0 0 0 0 0 0 0 3 2 2 □ 5 3 3 3 5 1 3 2 3 3 3 0 0 0 0 0 □ 1 . . . . . . . . . . . . . . . . 
■ . . . 1 0 0 0 0 0 0 0 1 3 3 5 3 3 3 3 1 3 2 3 5 1 1 1 0 0 0 0 0 1 0 . . . . . . □ . . □ . . . . . 
. . . . . 0 0 0 0 0 0 3 3 3 5 3 3 3 3 3 3 2 3 3 □ 3 4 3 1 0 0 0 0 □ . . . . . . . . . . . □ . . . . 
. . . . . 0 0 0 0 1 3 1 □ 5 5 3 6 3 3 3 5 3 0 0 0 1 ■ 2 0 0 0 0 0 ■ . . . ■ . . . . □ . . . . . . . 
. . . . . . 0 1 1 1 3 1 3 3 3 3 ■ 4 1 3 3 1 1 3 3 0 0 □ 1 0 0 0 4 . . . . . . . . . . . □ ■ . . . . 
. . . . . □ 0 1 1 3 1 1 3 1 1 3 3 3 5 3 1 3 3 2 0 0 0 0 0 0 1 0 ■ . . . . . . . . . . . . . □ ■ . . 
. . P ■ . . 0 0 1 □ 1 3 □ 1 1 □ 3 5 3 1 3 2 0 0 0 0 0 0 0 ■ 2 □ 0 . . . . . . . . . . . . . . . . . 
. . . . . . . 0 1 1 3 1 1 1 1 3 3 3 3 2 0 0 0 0 0 0 0 0 □ 0 0 1 . . . . □ . . . . . □ . . . . . . . 
. . . . . . . ■ 2 3 1 1 1 1 3 3 3 1 0 0 0 0 0 0 0 0 0 0 0 0 ■ 4 □ . . . . . . . ■ . □ . . . . . . . 
. . . . □ □ . . ■ 0 1 1 1 3 3 3 1 1 0 0 0 0 0 0 0 0 0 1 3 3 1 . . . . . . . . ■ . . . . . . □ . . . 
. . . . . . . . . 0 0 3 3 3 1 1 1 0 0 0 0 0 0 0 1 1 3 1 1 1 1 . . . . . . . . . . . . □ . . . . . . 
. . . □ . □ . . . □ 2 ■ 4 1 1 1 1 0 0 0 0 1 1 1 ■ □ 3 1 1 . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . □ . . . □ 1 1 1 0 0 1 1 1 1 1 1 1 . □ . . . . . . . . . . . . . . . . . . . . . . . 
. . . □ . □ . ■ . . . . . . . 3 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . . . . . . . . . . □ . 
. . . . . . . . . . . . . . . . . 1 1 1 1 1 . . . . . . . . . . . ■ . □ . . . ■ . . . . . . . . . . 
. . . . . . . . . ■ ■ . . . ■ . . . . 1 . . . . . . ■ . . . . . □ . . . . . . . . ■ . . . . . ■ . . 
. . . . . . . . . . . . . . P . . . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . 
. P . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . □ . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . □ . □ . . . . . . . . . . . . . . . . ■ . 
";

            //Assert
            Assert.AreEqual(ActionTypeEnum.Attack, aIAction.ActionType);
            Assert.AreEqual(6, aIAction.Score);
            Assert.AreEqual(new Vector3(19, 0, 19), aIAction.StartLocation);
            Assert.AreEqual(new Vector3(16, 0, 23), aIAction.EndLocation);
            Assert.AreEqual(mapStringExpected, mapString);

            //process AI for character 1
            CharacterAI ai2 = new CharacterAI();
            AIAction aIAction2 = ai2.CalculateAIAction(mission.Map,
                mission.Teams,
                mission.Teams[1].Characters[1],
                mission.RandomNumbers);
            string mapString2 = ai.CreateAIMap(mission.Map);
            string mapStringExpected2 = @"
. . . . . . . . . . . □ . . ■ . . □ . . . . . . . . □ . . . . . ■ . . . . . . . . . ■ . . . ■ . . . 
. . . . . . . . . . . . . . . . . . . . ■ . . . ■ . . . . . . . . . . . . . . . . . . ■ . . . . □ . 
. . . . . . . . . . . . . . . . ■ . . . . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . ■ . . . . . . . . ■ . . . . . . . . . □ ■ . . . . . . . . . . . . . . . . . . . 
. . . ■ . . ■ . . . □ . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . □ . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . □ . . . . . . . . ■ . . . . . . 
. . . . . □ . . . . . . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . □ . . . . . . □ . 
. . . . ■ . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . □ . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . ■ . . □ . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . □ . . . . □ . . □ . . . . . . . . . ■ . . . . . . . . . 
. . . . . . . . . . . □ . . . . □ . . . . . . . . . . . . . . . . . . . . □ . . . . . . ■ . . . . . 
. . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . ■ . . . . . . . . . . . 
. . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . 1 ■ . . . ■ . . . . . . ■ . . . ■ . . . . . . . . . . . . . . 
. . . . . . ■ . . . . . . ■ . . . 1 1 1 1 1 . . . . . □ . . . . . . . . . . . . . . . . . . . . . . 
. □ . . . . . . . . . . . . . 1 1 1 1 1 3 1 1 1 . . . . . . ■ . . . . . . . . . . . ■ . . ■ . . . . 
. . . . . . . . . . . . 1 1 1 1 1 1 1 1 □ 3 1 1 4 ■ . □ . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . ■ . 1 4 1 1 1 1 1 1 1 1 □ 3 1 1 □ 4 1 1 1 . □ . . . . . . . . ■ . □ . . . . . . . . 
. . . . □ . □ . . 1 1 ■ 6 1 1 1 1 1 3 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . ■ . . . . . . . . 
. . . . . . . . 1 1 3 3 3 1 1 1 1 1 □ 3 1 1 1 1 3 3 1 1 1 1 3 . . . . . . . . . . . . . . . . . □ . 
. . . . . . . 1 1 1 6 3 3 1 1 3 3 1 1 1 1 1 1 1 □ □ 3 1 4 1 □ 3 . . . . . . . . . . . . . . . ■ . . 
□ . . . . . . 1 1 1 ■ 6 1 1 1 3 1 3 1 0 1 1 1 1 1 1 1 1 ■ 4 1 □ . ■ . . . ■ . . . ■ . . . . . . ■ . 
. . . . . . 3 1 1 1 3 3 1 1 3 1 3 0 6 0 0 0 1 1 1 1 1 1 1 1 1 4 1 . . . . . . . □ . . □ . . . . . . 
. . . . . . 3 1 1 1 3 1 1 4 3 1 0 3 ■ 4 1 1 1 0 1 1 3 1 1 1 1 ■ 4 . ■ . . . □ . □ . . . . . . . . . 
. . . . . . . 3 1 4 3 1 4 ■ 4 0 3 0 3 1 3 3 6 0 0 1 □ 3 1 1 4 4 1 . . . . . . . . . . . . . . . . . 
. . . . . 3 1 □ 4 ■ 6 1 □ 4 6 0 6 0 1 1 3 6 ■ 6 1 0 1 3 1 1 ■ ■ 4 1 . . . □ . . . . . . . . . . ■ . 
. . . . . 3 1 1 1 ■ 4 1 1 1 ■ 6 ■ 4 1 3 3 ■ 4 1 3 0 1 □ 3 1 1 1 ■ 4 . . . . . □ . . . □ . . . . . . 
□ . . . □ 5 1 1 1 1 1 1 1 1 3 3 1 1 3 3 1 1 1 3 0 0 0 1 1 1 1 1 1 1 1 . . . . . . . . . ■ . . . . . 
. . . . 1 1 1 1 1 1 1 1 1 3 0 1 1 3 3 1 3 1 3 0 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . 
■ . . 1 3 1 1 1 1 1 1 1 1 3 1 3 3 3 1 P □ 5 0 1 1 1 3 1 1 1 1 1 3 1 1 . . . . . . . . . . . . . . . 
. ■ . . 3 1 1 1 1 1 1 1 3 1 1 □ 0 1 1 1 3 0 1 1 3 3 3 1 1 1 1 1 □ 3 . . . . . . . . . . . . . . . . 
■ . . . 3 1 1 1 1 1 1 1 0 1 1 3 1 1 1 0 0 1 1 3 5 0 0 3 1 1 1 1 1 3 1 . . . . . . □ . . □ . . . . . 
. . . . . 1 1 1 1 1 1 3 3 1 3 1 1 1 0 1 1 1 3 3 □ 0 6 3 3 1 1 1 1 □ . . . . . . . . . . . □ . . . . 
. . . . . 1 1 1 1 1 3 1 □ 5 3 1 4 0 1 1 3 3 0 0 0 3 ■ 4 1 1 1 1 1 ■ . . . ■ . . . . □ . . . . . . . 
. . . . . . 1 1 1 1 3 1 3 3 1 1 ■ 4 1 3 3 0 0 3 3 1 1 □ 3 1 1 1 4 . . . . . . . . . . . □ ■ . . . . 
. . . . . □ 3 1 1 5 1 1 5 1 1 3 1 1 3 0 0 3 3 1 1 1 1 1 1 4 1 4 ■ . . . . . . . . . . . . . □ ■ . . 
. . . ■ . . 1 1 1 □ 3 3 □ 3 1 □ 3 3 0 0 3 1 1 1 1 1 1 1 4 ■ 4 □ 4 . . . . . . . . . . . . . . . . . 
. . . . . . . 4 1 1 3 1 1 1 1 3 3 3 3 1 1 1 1 1 1 1 1 1 □ 4 4 1 . . . . □ . . . . . □ . . . . . . . 
. . . . . . . ■ 4 3 1 1 1 1 3 3 3 1 1 1 1 1 1 1 1 1 1 1 1 1 ■ 4 □ . . . . . . . ■ . □ . . . . . . . 
. . . . □ □ . . ■ 4 1 1 1 3 3 3 1 1 1 1 1 1 1 1 1 1 1 3 3 3 3 . . . . . . . . ■ . . . . . . □ . . . 
. . . . . . . . . 3 1 6 3 3 1 1 1 1 1 1 1 1 1 1 6 5 3 3 3 3 3 . . . . . . . . . . . . □ . . . . . . 
. . . □ . □ . . . □ 6 ■ 4 1 1 1 1 1 1 1 1 3 3 3 ■ □ 5 3 3 . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . □ . . . □ 3 1 1 1 1 3 3 3 3 3 3 3 . □ . . . . . . . . . . . . . . . . . . . . . . . 
. . . □ . □ . ■ . . . . . . . 3 3 3 3 3 3 3 3 3 . . . . . . . . . . . . . . . . . . . . . . . . □ . 
. . . . . . . . . . . . . . . . . 3 3 3 3 3 . . . . . . . . . . . ■ . □ . . . ■ . . . . . . . . . . 
. . . . . . . . . ■ ■ . . . ■ . . . . 1 . . . . . . ■ . . . . . □ . . . . . . . . ■ . . . . . ■ . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . 
. P . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . □ . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . □ . □ . . . . . . . . . . . . . . . . ■ . 
";

            //Assert
            Assert.AreEqual(ActionTypeEnum.Attack, aIAction2.ActionType);
            Assert.AreEqual(5, aIAction2.Score);
            Assert.AreEqual(new Vector3(19, 0, 19), aIAction2.StartLocation);
            Assert.AreEqual(new Vector3(13, 0, 15), aIAction2.EndLocation);
            Assert.AreEqual(mapStringExpected2, mapString2);
        }
    }
}
