using TBE.Logic.Characters;
using TBE.Logic.Game;
using TBE.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TBE.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class DoorTests
    {
        [TestMethod]
        public void ClosedDoorBlocksLineOfSightWithoutProvidingCover()
        {
            string[,,] map = MapCore.InitializeMap(5, 1, 1);
            Vector3 source = new(0, 0, 0);
            Vector3 behindDoor = new(4, 0, 0);
            map[2, 0, 0] = CoverType.DoorClosed;

            List<Vector3> fieldOfView = FieldOfView.GetFieldOfView(map, source, 5);
            CoverState coverState = CharacterCover.CalculateCover(map, new Vector3(1, 0, 0), new List<Vector3>());

            Assert.IsTrue(fieldOfView.Contains(new Vector3(2, 0, 0)));
            Assert.IsFalse(fieldOfView.Contains(behindDoor));
            Assert.IsFalse(coverState.InFullCover);
            Assert.IsFalse(coverState.InHalfCover);

            map[2, 0, 0] = CoverType.DoorOpen;
            fieldOfView = FieldOfView.GetFieldOfView(map, source, 5);
            Assert.IsTrue(fieldOfView.Contains(behindDoor));
        }

        [TestMethod]
        public void MovingThroughClosedDoorOpensItAndKeepsItOpen()
        {
            string[,,] map = MapCore.InitializeMap(3, 1, 1);
            Character character = CharacterPool.CreateFredHero(map, Vector3.Zero);
            map[1, 0, 0] = CoverType.DoorClosed;

            PathFindingResult path = PathFinding.FindPath(map, character.Location, new Vector3(2, 0, 0));
            CharacterMovement.MoveCharacter(map, character, path, null, null, null);

            Assert.AreEqual(new Vector3(2, 0, 0), character.Location);
            Assert.AreEqual(CoverType.DoorOpen, map[1, 0, 0]);
        }

        [TestMethod]
        public void AdjacentDoorCommandsDoNotConsumeActionPoints()
        {
            string[,,] map = MapCore.InitializeMap(3, 1, 1);
            Character character = CharacterPool.CreateFredHero(map, Vector3.Zero);
            map[1, 0, 0] = CoverType.DoorClosed;
            Mission mission = new Mission() { Map = map };
            character.ActionPointsCurrent = 0;
            int actionPoints = character.ActionPointsCurrent;

            Assert.IsTrue(character.GetCurrentActions(map).Any(action => action.Name == "_open_door"));
            Assert.IsTrue(mission.OpenDoor(character));
            Assert.AreEqual(CoverType.DoorOpen, map[1, 0, 0]);
            Assert.IsTrue(character.GetCurrentActions(map).Any(action => action.Name == "_close_door"));
            Assert.IsTrue(mission.CloseDoor(character));
            Assert.AreEqual(CoverType.DoorClosed, map[1, 0, 0]);
            Assert.AreEqual(0, character.ActionPointsCurrent);
            Assert.AreEqual(actionPoints, character.ActionPointsCurrent);
        }

        [TestMethod]
        public void DoorCannotCloseWhileCharacterOccupiesIt()
        {
            string[,,] map = MapCore.InitializeMap(3, 1, 1);
            map[1, 0, 0] = CoverType.DoorOpen;
            Character adjacentCharacter = CharacterPool.CreateFredHero(map, Vector3.Zero);
            Character characterInDoorway = CharacterPool.CreateHarryHero(map, new Vector3(1, 0, 0));
            Team team = new Team(0);
            team.Characters.Add(adjacentCharacter);
            team.Characters.Add(characterInDoorway);
            Mission mission = new Mission() { Map = map, Teams = new List<Team> { team, new Team(0) } };

            Assert.IsFalse(mission.CloseDoor(adjacentCharacter));
            Assert.AreEqual(CoverType.DoorOpen, characterInDoorway.TileUnderCharacter);

            characterInDoorway.SetLocationAndRange(map, new Vector3(2, 0, 0), characterInDoorway.FOVRange, null);
            Assert.AreEqual(CoverType.DoorOpen, map[1, 0, 0]);
            Assert.IsTrue(mission.CloseDoor(adjacentCharacter));
            Assert.AreEqual(CoverType.DoorClosed, map[1, 0, 0]);
        }
    }
}
