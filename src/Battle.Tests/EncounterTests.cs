using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battle.Logic;
using System.Collections.Generic;

namespace Battle.Tests
{
    [TestClass]
    [TestCategory("L0")]
    public class EncounterTests
    {
        [TestMethod]
        public void FredSwordJeffHitEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            Weapon sword = WeaponPool.CreateSword();
            Character jeff = CharacterPool.CreateJeff();
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            bool result = Encounter.AttackCharacter(fred, sword, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FredSwordJeffMissEncounterTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFred();
            fred.ChanceToHit = 75;
            Weapon sword = WeaponPool.CreateSword();
            Character jeff = CharacterPool.CreateJeff();
            List<int> randomNumbers = RandomNumber.GenerateRandomNumberList(0, 100, 0, 1);

            //Act
            bool result = Encounter.AttackCharacter(fred, sword, jeff, randomNumbers);

            //Assert
            Assert.IsTrue(result == false);
        }

    }
}
