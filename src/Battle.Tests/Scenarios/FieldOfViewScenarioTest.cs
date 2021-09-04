using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.GameController;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using Battle.Tests.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Scenarios
{

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class FieldOfViewScenarioTest
    {
        [TestMethod]
        public void JeffMovesAndFOVUpdatesTest()
        {
            //arrange
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(10, 1, 10)
            };

            mission.Map[5, 0, 2] = CoverType.FullCover;
            mission.Map[5, 0, 3] = CoverType.FullCover;
            mission.Map[5, 0, 4] = CoverType.FullCover;
            mission.Map[5, 0, 5] = CoverType.FullCover;
            mission.Map[5, 0, 6] = CoverType.FullCover;
            mission.Map[5, 0, 7] = CoverType.HalfCover; //half cover here!
            mission.Map[5, 0, 8] = CoverType.FullCover;
            mission.Map[5, 0, 9] = CoverType.FullCover;
            Character fred = CharacterPool.CreateFredHero(mission.Map, new Vector3(1, 0, 1));
            fred.HitpointsCurrent = 1;
            Team team1 = new Team()
            {
                Name = "Good guys",
                Characters = new List<Character>() { fred }
            };
            mission.Teams.Add(team1);
            Character jeff = CharacterPool.CreateJeffBaddie(mission.Map, new Vector3(9, 0, 7));
            jeff.HitpointsCurrent = 5;
            jeff.InOverwatch = true;
            Team team2 = new Team()
            {
                Name = "Bad guys",
                Characters = new List<Character>() { jeff }
            };
            mission.Teams.Add(team2);
            RandomNumberQueue diceRolls = new RandomNumberQueue(new List<int> { 100, 100, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //act
            fred = FieldOfView.UpdateCharacterFOV(mission.Map, fred);
            string fovMapString = MapCore.GetMapStringWithMapMask(mission.Map, fred.FOVMap);
            string mapString = MapCore.GetMapString(mission.Map);

            //assert
            string expected = @"
. . . . . ■ . . . . 
. . . . . ■ . . . . 
. . . . . □ . . . P 
. . . . . ■ . . . . 
. . . . . ■ . . . . 
. . . . . ■ . . . . 
. . . . . ■ . . . . 
. . . . . ■ . . . . 
. P . . . . . . . . 
. . . . . . . . . . 
";
            Assert.AreEqual(expected, mapString);

            string expectedFOV = @"
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . □ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ . . . . 
. P . . . . . . . . 
. . . . . . . . . . 
";
            Assert.AreEqual(expectedFOV, fovMapString);

            jeff = FieldOfView.UpdateCharacterFOV(mission.Map, jeff);
            string jeffFOVMapString = MapCore.GetMapStringWithMapMask(mission.Map, jeff.FOVMap);
            string expectedJeffFOV = @"
▓ ▓ ▓ ▓ ▓ ■ . . . . 
. . . . . ■ . . . . 
. . . . . □ . . . P 
. . . . . ■ . . . . 
▓ ▓ ▓ ▓ ▓ ■ . . . . 
▓ ▓ ▓ ▓ ▓ ■ . . . . 
▓ ▓ ▓ ▓ ▓ ■ . . . . 
▓ ▓ ▓ ▓ ▓ ■ . . . . 
▓ ▓ ▓ ▓ ▓ ▓ . . . . 
▓ ▓ ▓ ▓ ▓ . . . . . 
";
            Assert.AreEqual(expectedJeffFOV, jeffFOVMapString);

            //Act, part 2 - moving up the Y axis
            PathFindingResult pathFindingResult = PathFinding.FindPath(fred.Location,
                new Vector3(1, 0, 9),
                mission.Map);
            List<ActionResult> movementResults = CharacterMovement.MoveCharacter(fred,
                mission.Map,
                pathFindingResult,
                diceRolls,
                new List<Character>() { jeff });

            for (int i = 0; i < movementResults.Count; i++)
            {
                if (i == 0)
                {
                    string expectedMovement = @"
. . . . . ■ ▓ . ▓ ▓ 
. . . . . ■ . ▓ ▓ ▓ 
. . . . . □ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. P . . . ■ ░ ░ ░ ░ 
. . . . . . . . ░ ░ 
. . . . . . . . . . 
";
                    Assert.AreEqual(expectedMovement, movementResults[i].FOVMapString);
                }
                else if (i == 1)
                {
                    string expectedMovement = @"
. . . . . ■ ▓ . ▓ ▓ 
. . . . . ■ . ▓ ▓ ▓ 
. . . . . □ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. P . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ░ ░ ░ ░ 
. . . . . . . ░ ░ ░ 
. . . . . . . . . ░ 
";
                    Assert.AreEqual(expectedMovement, movementResults[i].FOVMapString);
                }
                else if (i == 2)
                {
                    string expectedMovement = @"
. . . . . ■ ▓ . . ▓ 
. . . . . ■ . . ▓ ▓ 
. . . . . □ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. P . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ░ ░ ░ ░ 
. . . . . . . ░ ░ ░ 
. . . . . . . . ░ ░ 
";
                    Assert.AreEqual(expectedMovement, movementResults[i].FOVMapString);
                }
                else if (i == 3)
                {
                    string expectedMovement = @"
. . . . . ■ ▓ ░ . . 
. . . . . ■ . . . ▓ 
. . . . . □ . ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. P . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ░ ░ ░ ░ 
. . . . . . ░ ░ ░ ░ 
. . . . . . . ░ ░ ░ 
";
                    Assert.AreEqual(expectedMovement, movementResults[i].FOVMapString);
                }
                else if (i == 4)
                {
                    string expectedMovement = @"
. . . . . ■ ▓ ░ . . 
. . . . . ■ . . . . 
. . . . . □ . . ▓ ▓ 
. P . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ░ ░ ░ ░ 
. . . . . . ░ ░ ░ ░ 
. . . . . . . ░ ░ ░ 
";
                    Assert.AreEqual(expectedMovement, movementResults[i].FOVMapString);
                }
                else if (i == 5)
                {
                    string expectedMovement = @"
. . . . . ■ ▓ ░ . . 
. . . . . ■ . . . . 
. . . . . □ . . ▓ ▓ 
. P . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ▓ ▓ ▓ ▓ 
. . . . . ■ ░ ░ ░ ░ 
. . . . . . ░ ░ ░ ░ 
. . . . . . . ░ ░ ░ 
";
                    Assert.AreEqual(expectedMovement, movementResults[i].FOVMapString);
                }
            }
        }
    }
}
