using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Items;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Encounters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class AbilityAttackTests
    {
        [TestMethod]
        public void FredAttacksJeffWithRifleAndHitsBaseTest()
        {
            //Arrange
            Character fred = CharacterPool.CreateFredHero();
            Weapon rifle = fred.WeaponEquipped;
            Character jeff = CharacterPool.CreateJeffBaddie();
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Queue<int> diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            int chanceToHit = EncounterCore.GetChanceToHit(fred, rifle, jeff);
            int chanceToCrit = EncounterCore.GetChanceToCrit(fred, rifle, jeff, map, false);
            DamageOptions damageOptions = EncounterCore.GetDamageRange(fred, rifle);
            EncounterResult result = Encounter.AttackCharacter(fred, rifle, jeff, map, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.IsNotNull(damageOptions);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);
            Assert.AreEqual(8, damageOptions.CriticalDamageLow);
            Assert.AreEqual(12, damageOptions.CriticalDamageHigh);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

    }
}