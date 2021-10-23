using Battle.Logic.Characters;
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
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class AIDefaultTest
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        [TestMethod]
        public void AIDefaultPassivePlayerTest()
        {
            //Arrange
            string path = _rootPath + @"\SaveGames\Saves\Save015.json";

            //Act
            string fileContents;
            using (var streamReader = new StreamReader(path))
            {
                fileContents = streamReader.ReadToEnd();
            }
            Mission mission = GameSerialization.LoadGame(fileContents);
            //Starts with turn 1 - good guys
            mission.StartMission();
            //Move to turn 1 - bad guys
            mission.MoveToNextTurn();

            Character jethro = mission.Teams[1].Characters[0];
            Character bart = mission.Teams[1].Characters[1];
            Team teamGood = mission.Teams[0];
            Team teamBad = mission.Teams[1];
            AIAction aIAction = mission.CalculateAIAction(jethro, mission.Teams);

            //Assert
            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, aIAction.ActionType);
            Assert.AreEqual(14, aIAction.Score);
            Assert.AreEqual(new Vector3(19, 0, 19), aIAction.StartLocation);
            Assert.AreEqual(new Vector3(24, 0, 17), aIAction.EndLocation);
            Assert.AreEqual("P", mission.Map[1, 0, 1]);

            //Act
            mission.MoveCharacter(jethro,
                teamBad,
                teamGood,
                aIAction.EndLocation);
            EncounterResult encounterResult = mission.AttackCharacter(jethro,
                jethro.WeaponEquipped,
                teamGood.GetCharacter(jethro.GetTargetCharacter()),
                teamBad,
                teamGood);

            //Assert
            string log = @"
Jethro is attacking with Shotgun, targeted on Fred
Hit: Chance to hit: 80, (dice roll: 81)
Damage range: 3-5, (dice roll: 76)
Critical chance: 70, (dice roll: 55)
Critical damage range: 9-13, (dice roll: 76)
12 damage dealt to character Fred, HP is now -8
Fred is killed
100 XP added to character Jethro, for a total of 100 XP
Jethro is ready to level up
";
            Assert.AreEqual(log, encounterResult.LogString);
            Assert.AreEqual("", mission.Map[1, 0, 1]);

            //Act
            List<Character> charactersInView = FieldOfView.GetCharactersInView(mission.Map,
                   bart.Location,
                   bart.ShootingRange,
                   teamGood.Characters);
            Assert.AreEqual(0, charactersInView.Count);
            CharacterAI ai2 = new CharacterAI();
            AIAction aIAction2 = ai2.CalculateAIAction(mission.Map,
                bart,
                mission.Teams,
                mission.RandomNumbers);

            //Assert
            Assert.AreEqual(ActionTypeEnum.DoubleMove, aIAction2.ActionType);
            Assert.AreEqual(7, aIAction2.Score);
            Assert.AreEqual(new Vector3(26, 0, 32), aIAction2.StartLocation);
            Assert.AreEqual(new Vector3(21, 0, 19), aIAction2.EndLocation);
            
            mission.MoveCharacter(bart,
                teamBad,
                teamGood,
                aIAction2.EndLocation);

            List<Character> charactersInView2 = FieldOfView.GetCharactersInView(mission.Map,
                bart.Location,
                bart.ShootingRange,
                teamGood.Characters);
            Assert.AreEqual(1, charactersInView2.Count);
            Assert.AreEqual("Harry", charactersInView2[0].Name);

            //Move to turn 2 - good guys
            mission.MoveToNextTurn();
            //Move to turn 2 - bad guys
            mission.MoveToNextTurn();


        }
    }
}
