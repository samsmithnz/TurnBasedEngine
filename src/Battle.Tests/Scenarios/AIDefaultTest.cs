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

            //Act - Turn 1 Jethro bad AI
            AIAction aIAction = mission.CalculateAIAction(jethro, mission.Teams);

            //Assert - Turn 1 Jethro bad AI
            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, aIAction.ActionType);
            Assert.AreEqual(14, aIAction.Score);
            Assert.AreEqual(new Vector3(19, 0, 19), aIAction.StartLocation);
            Assert.AreEqual(new Vector3(24, 0, 17), aIAction.EndLocation);
            Assert.AreEqual("P", mission.Map[1, 0, 1]);

            //Act - Turn 1 Jethro bad action
            mission.MoveCharacter(jethro,
                teamBad,
                teamGood,
                aIAction.EndLocation);
            EncounterResult encounterResult = mission.AttackCharacter(jethro,
                jethro.WeaponEquipped,
                teamGood.GetCharacter(jethro.GetTargetCharacter()),
                teamBad,
                teamGood);

            //Assert - Turn 1 Jethro bad action
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

            //Act - Turn 1 Bart bad AI
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

            //Assert - Turn 1 Bart bad AI
            Assert.AreEqual(ActionTypeEnum.DoubleMove, aIAction2.ActionType);
            Assert.AreEqual(7, aIAction2.Score);
            Assert.AreEqual(new Vector3(26, 0, 32), aIAction2.StartLocation);
            Assert.AreEqual(new Vector3(21, 0, 19), aIAction2.EndLocation);

            //Act - Turn 1 Bart bad action
            mission.MoveCharacter(bart,
                teamBad,
                teamGood,
                aIAction2.EndLocation);
            List<Character> charactersInView2 = FieldOfView.GetCharactersInView(mission.Map,
                bart.Location,
                bart.ShootingRange,
                teamGood.Characters);

            //Assert - Turn 1 Bart bad action
            Assert.AreEqual(1, charactersInView2.Count);
            Assert.AreEqual("Harry", charactersInView2[0].Name);

            //Move to turn 2 - good guys
            mission.MoveToNextTurn();
            //Move to turn 2 - bad guys
            mission.MoveToNextTurn();

            //Act - Turn 2 Jethro bad AI
            AIAction aIAction3 = mission.CalculateAIAction(jethro, mission.Teams);

            //Assert - Turn 2 Jethro bad AI
            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, aIAction3.ActionType);
            Assert.AreEqual(10, aIAction3.Score);
            Assert.AreEqual(new Vector3(24, 0, 17), aIAction3.StartLocation);
            Assert.AreEqual(new Vector3(28, 0, 12), aIAction3.EndLocation);

            //Act - Turn 2 Jethro bad action
            mission.MoveCharacter(jethro,
                teamBad,
                teamGood,
                aIAction.EndLocation);
            EncounterResult encounterResult3 = mission.AttackCharacter(jethro,
                jethro.WeaponEquipped,
                teamGood.GetCharacter(jethro.GetTargetCharacter()),
                teamBad,
                teamGood);

            //Assert - Turn 2 Jethro bad action
            string log3 = @"
Jethro is attacking with Shotgun, targeted on Harry
Hit: Chance to hit: 80, (dice roll: 90)
Damage range: 3-5, (dice roll: 44)
Critical chance: 70, (dice roll: 97)
Critical damage range: 9-13, (dice roll: 44)
Armor prevented 1 damage to character Harry
9 damage dealt to character Harry, HP is now 3
10 XP added to character Jethro, for a total of 110 XP
Jethro is ready to level up
";
            Assert.AreEqual(log3, encounterResult3.LogString);

            //Act - Turn 2 Bart bad AI
            AIAction aIAction4 = mission.CalculateAIAction(bart, mission.Teams);

            //Assert - Turn 2 Bart bad AI
            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, aIAction4.ActionType);
            Assert.AreEqual(11, aIAction4.Score);
            Assert.AreEqual(new Vector3(21, 0, 19), aIAction4.StartLocation);
            Assert.AreEqual(new Vector3(15, 0, 19), aIAction4.EndLocation);

            //Act - Turn 2 Bart bad action
            mission.MoveCharacter(bart,
                teamBad,
                teamGood,
                aIAction4.EndLocation);
            EncounterResult encounterResult4 = mission.AttackCharacter(bart,
                bart.WeaponEquipped,
                teamGood.GetCharacter(bart.GetTargetCharacter()),
                teamBad,
                teamGood);

            //Assert - Turn 2 Bart bad action
            string log4 = @"
Bart is attacking with Rifle, targeted on Harry
Hit: Chance to hit: 80, (dice roll: 29)
Damage range: 3-5, (dice roll: 46)
Critical chance: 70, (dice roll: 63)
Critical damage range: 8-12, (dice roll: 46)
Armor prevented 1 damage to character Harry
8 damage dealt to character Harry, HP is now -5
Harry is killed
100 XP added to character Bart, for a total of 100 XP
Bart is ready to level up
";
            Assert.AreEqual(log4, encounterResult4.LogString);
        }
    }
}
