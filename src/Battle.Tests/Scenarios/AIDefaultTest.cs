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
            mission.StartMission();
            mission.MoveToNextTurn();

            Character sourceCharacter = mission.Teams[1].Characters[0];
            Team team1 = mission.Teams[0];
            Team team2 = mission.Teams[1];
            AIAction aIAction = mission.CalculateAIAction(sourceCharacter, mission.Teams);

            //Assert
            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, aIAction.ActionType);
            Assert.AreEqual(14, aIAction.Score);
            Assert.AreEqual(new Vector3(19, 0, 19), aIAction.StartLocation);
            Assert.AreEqual(new Vector3(24, 0, 17), aIAction.EndLocation);
            Assert.AreEqual("P", mission.Map[1, 0, 1]);

            //Act
            mission.MoveCharacter(sourceCharacter,
                team2,
                team1,
                aIAction.EndLocation);
            EncounterResult encounterResult = mission.AttackCharacter(sourceCharacter,
                sourceCharacter.WeaponEquipped,
                team1.GetCharacter(sourceCharacter.GetTargetCharacter()),
                team2,
                team1);

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
                   mission.Teams[1].Characters[1].Location,
                   mission.Teams[1].Characters[1].ShootingRange,
                   mission.Teams[0].Characters);
            Assert.AreEqual(0, charactersInView.Count);
            List<Character> charactersInView2 = FieldOfView.GetCharactersInView(mission.Map,
                   mission.Teams[1].Characters[0].Location,
                   mission.Teams[1].Characters[1].ShootingRange,
                   mission.Teams[0].Characters);
            Assert.AreEqual(2, charactersInView2.Count);
            CharacterAI ai2 = new CharacterAI();
            AIAction aIAction2 = ai2.CalculateAIAction(mission.Map,
                mission.Teams[1].Characters[1],
                mission.Teams,
                mission.RandomNumbers);

            //Assert
            Assert.AreEqual(ActionTypeEnum.DoubleMove, aIAction2.ActionType);
            //Assert.AreEqual(5, aIAction2.Score);
            Assert.AreEqual(new Vector3(26, 0, 32), aIAction2.StartLocation);
            Assert.AreEqual(new Vector3(21, 0, 19), aIAction2.EndLocation);
            //Assert.AreEqual(mapStringExpected2, mapString2);

            mission.MoveCharacter(mission.Teams[1].Characters[1],
                mission.Teams[1],
                mission.Teams[0],
                aIAction2.EndLocation);
        }
    }
}
