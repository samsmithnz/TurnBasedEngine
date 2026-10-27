using TBE.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;
using TBE.Logic.Utility;

namespace TBE.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class MissedShotsTests
    {
        [TestMethod]
        [DataRow(20, 10, 0)]
        [DataRow(20, -10, 0)]
        [DataRow(-20, 10, 0)]
        [DataRow(-20, -10, 0)]
        [DataRow(20, 0, 0)]
        [DataRow(-20, 0, 0)]
        [DataRow(0, 10, 0)]
        [DataRow(0, -10, 0)]
        [DataRow(20, 10, 100)]
        [DataRow(20, -10, 100)]
        [DataRow(-20, 10, 100)]
        [DataRow(-20, -10, 100)]
        [DataRow(20, 0, 100)]
        [DataRow(-20, 0, 100)]
        [DataRow(0, 10, 100)]
        [DataRow(0, -10, 100)]
        public void MissDirectionRotatesOriginalBearingBeyondRectangularMapTest(int x, int z, int roll)
        {
            Vector3 source = new(50, 1, 20);
            Vector3 target = source + new Vector3(x, 0, z);
            string[,,] map = MapCore.InitializeMap(100, 3, 40);
            RandomNumberQueue diceRolls = new(new int[] { roll, 42 });

            Vector3 result = FieldOfView.GetMissedLocation(map, source, target, 50, diceRolls);
            Vector3 direction = result - source;
            double angle = Math.Atan2(x * direction.Z - z * direction.X, x * direction.X + z * direction.Z) * 180 / Math.PI;

            Assert.AreEqual(roll == 0 ? -25 : 25, angle, 1);
            Assert.AreEqual(source.Y, result.Y);
            Assert.IsTrue(result.X < 0 || result.X >= 100 || result.Z < 0 || result.Z >= 40);
            Assert.AreEqual(42, diceRolls.Peek());
            Assert.AreEqual(result, FieldOfView.GetMissedLocation(map, source, target, 50, new RandomNumberQueue(new int[] { roll })));
        }

        [TestMethod]
        public void WorseMissHasWiderDeviationTest()
        {
            string[,,] map = MapCore.InitializeMap(100, 1, 100);
            Vector3 source = new(10, 0, 50);
            Vector3 target = new(50, 0, 50);

            Vector3 nearMiss = FieldOfView.GetMissedLocation(map, source, target, 1, new RandomNumberQueue(new int[] { 100 }));
            Vector3 badMiss = FieldOfView.GetMissedLocation(map, source, target, 100, new RandomNumberQueue(new int[] { 100 }));

            Assert.IsTrue(nearMiss.Z > target.Z);
            Assert.IsTrue(badMiss.Z - source.Z > nearMiss.Z - source.Z);
            Assert.IsTrue(badMiss.X - source.X < nearMiss.X - source.X);
        }

        [TestMethod]
        [DataRow(-10, 0)]
        [DataRow(110, 100)]
        public void MissSeverityIsClampedTest(int severity, int expectedSeverity)
        {
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 source = new(0, 0, 0);
            Vector3 target = new(2, 0, 2);

            Vector3 result = FieldOfView.GetMissedLocation(map, source, target, severity, new RandomNumberQueue(new int[] { 100 }));
            Vector3 expected = FieldOfView.GetMissedLocation(map, source, target, expectedSeverity, new RandomNumberQueue(new int[] { 100 }));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CoincidentSourceAndTargetProducesValidEndpointTest()
        {
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 source = new(5, 0, 5);

            Vector3 result = FieldOfView.GetMissedLocation(map, source, source, 1, new RandomNumberQueue(new int[] { 100 }));

            Assert.IsTrue(result.X >= 10);
            Assert.AreEqual(source.Y, result.Y);
        }

        [TestMethod]
        [DataRow(0, 4)]
        [DataRow(100, 16)]
        public void MissedShotHitsCoverOnEitherSideTest(int roll, int coverZ)
        {
            string[,,] map = MapCore.InitializeMap(30, 1, 30);
            Vector3 source = new(2, 0, 10);
            Vector3 target = new(10, 0, 10);
            map[2, 0, 10] = "P";
            map[10, 0, 10] = "P";
            map[8, 0, coverZ] = CoverType.FullCover;
            map[9, 0, coverZ + (roll == 0 ? -1 : 1)] = CoverType.FullCover;

            Vector3 result = FieldOfView.MissedShot(map, source, target, 100, new RandomNumberQueue(new int[] { roll }));

            Assert.AreEqual(new Vector3(8, 0, coverZ), result);
        }

        [TestMethod]
        [DataRow(10, 1)]
        [DataRow(10, -1)]
        [DataRow(-10, 1)]
        [DataRow(-10, -1)]
        [DataRow(0, 10)]
        [DataRow(0, -10)]
        public void MissedShotReturnsMapEdgeWhenNoCoverTest(int x, int z)
        {
            string[,,] map = MapCore.InitializeMap(30, 1, 10);
            Vector3 source = new(15, 0, 5);
            Vector3 target = source + new Vector3(x, 0, z);

            Vector3 result = FieldOfView.MissedShot(map, source, target, 50, new RandomNumberQueue(new int[] { 100 }));

            Assert.IsTrue(result.X >= 0 && result.X < 30 && result.Z >= 0 && result.Z < 10);
            Assert.IsTrue(result.X == 0 || result.X == 29 || result.Z == 0 || result.Z == 9);
        }
    }
}
