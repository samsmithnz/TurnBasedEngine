using Battle.Logic.Characters;
using Battle.Logic.FieldOfView;
using Battle.Logic.Movement;
using Battle.Logic.PathFinding;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Overwatch
{
    [TestClass]
    [TestCategory("L0")]
    public class OverwatchTests
    {
        [TestMethod]
        public void FredInOverwatchWhileJeffMoves()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.InOverwatch = true;
            //Weapon rifle = fred.WeaponEquiped;
            Character jeff = CharacterPool.CreateJeff();
            string[,] map = MapUtility.InitializeMap(10, 10);
            Vector3 destination = new(6, 0, 0);

            //Act
            List<Vector3> fov = FieldOfViewCalculator.GetFieldOfView(map, (int)fred.Location.X, (int)fred.Location.Z, fred.Range);
            KeyValuePair<Character, List<Vector3>> fredFOV = new(fred, fov);

            Path jeffPath = new(jeff.Location, destination, map);
            PathResult pathResult = jeffPath.FindPath();
            jeff = CharacterMovement.MoveCharacter(jeff, pathResult.Path, new() { fredFOV });

            //Assert
            Assert.IsTrue(jeffPath != null);
            Assert.IsTrue(pathResult != null);
            Assert.AreEqual(0, jeff.HP  );
        }
    }
}
