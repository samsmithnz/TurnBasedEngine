using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.GameController;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CharacterAITests
    {

        [TestMethod]
        public void CharacterJethroAIMovesIntoFullCoverTest()
        {
            //Arrange
            Mission mission = new Mission
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[20, 0, 20] = CoverType.FullCover;
            mission.Map[18, 0, 18] = CoverType.HalfCover;
            Character jethro = CharacterPool.CreateJethroBaddie(null, new Vector3(8, 0, 8));
            jethro.SetLocation(new Vector3(15, 0, 15), mission.Map);

            //Act            
            ActionResult actionResult = CharacterAI.CalculateAction(mission.Map, jethro, mission.RandomNumbers);

            //Assert
            Assert.AreEqual(new Vector3(15, 0, 15), actionResult.StartLocation);
            Assert.AreEqual(new Vector3(20, 0, 19), actionResult.EndLocation);
            Assert.IsTrue(jethro.InFullCover);
        }

    }
}
