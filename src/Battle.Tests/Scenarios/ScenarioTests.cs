using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.MainGame;
using Battle.Logic.Movement;
using Battle.Logic.PathFinding;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battle.Tests.Scenarios
{
    [TestClass]
    [TestCategory("L0")]
    public class ScenarioTests
    {
        [TestMethod]
        public void SimpleScenarioMultipleTurnsTest()
        {
            //Arrange
            Game game = new();
            game.TurnNumber = 1;
            game.Map = MapUtility.InitializeMap(50, 50);
            game.Map[6, 5] = "W";
            game.Map[20, 11] = "W";
            Character fred = CharacterPool.CreateFredHero();
            Team team1 = new()
            {
                Name = "Player",
                Characters = new() { fred }
            };
            fred.Location = new(5, 0, 5);
            game.Teams.Add(team1);
            Character jeff = CharacterPool.CreateJeffBaddie();
            Team team2 = new()
            {
                Name = "Enemy",
                Characters = new() { jeff }
            };
            jeff.Location = new(20, 0, 10);
            game.Teams.Add(team2);
            Queue<int> diceRolls = new(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll


            //Assert - Setup
            Assert.AreEqual(1, game.TurnNumber);
            Assert.AreEqual(2, game.Teams.Count);
            Assert.AreEqual(50 * 50, game.Map.Length);
            Assert.AreEqual("Player", game.Teams[0].Name);
            Assert.AreEqual(1, game.Teams[0].Characters.Count);
            Assert.AreEqual("Enemy", game.Teams[1].Name);
            Assert.AreEqual(1, game.Teams[1].Characters.Count);

            //Act

            //Turn 1 - Team 1 starts
            //Fred runs to cover
            PathResult pathResult = Path.FindPath(fred.Location, new(9, 0, 10), game.Map);
            CharacterMovement.MoveCharacter(fred, game.Map, pathResult.Path, diceRolls, null);

            //Fred aims at Jeff, who is behind high cover. 
            List<Character> characters = fred.GetCharactersInView(game.Map, new List<Team>() { team2 });       
            Assert.AreEqual(characters[0], jeff);
            int chanceToHit = EncounterCore.GetChanceToHit(fred, fred.WeaponEquipped, jeff);
            int chanceToCrit = EncounterCore.GetChanceToCrit(fred, fred.WeaponEquipped, jeff, game.Map, false);
            DamageOptions damageOptions = EncounterCore.GetDamageRange(fred, fred.WeaponEquipped);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);

            //Fred shoots at Jeff, who is behind high cover. He hits him. 
            EncounterResult encounter1 = Encounter.AttackCharacter(fred,
                    fred.WeaponEquipped,
                    jeff,
                    game.Map,
                    diceRolls);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jeff, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log1, encounter1.LogString);

            //Turn 1 - Team 2 starts
            //Jeff aims back and misses
            List<Character> characters2 = jeff.GetCharactersInView(game.Map, new List<Team>() { team1 });
            Assert.AreEqual(characters2[0], fred);
            int chanceToHit2 = EncounterCore.GetChanceToHit(jeff, jeff.WeaponEquipped, fred);
            int chanceToCrit2 = EncounterCore.GetChanceToCrit(jeff, jeff.WeaponEquipped, jeff, game.Map, false);
            DamageOptions damageOptions2 = EncounterCore.GetDamageRange(jeff, jeff.WeaponEquipped);
            Assert.AreEqual(72, chanceToHit2);
            Assert.AreEqual(70, chanceToCrit2);
            Assert.AreEqual(3, damageOptions2.DamageLow);
            Assert.AreEqual(5, damageOptions2.DamageHigh);

            //Jeff shoots back and misses
            EncounterResult encounter2 = Encounter.AttackCharacter(jeff,
                    jeff.WeaponEquipped,
                    fred,
                    game.Map,
                    diceRolls);
            string log2 = @"
Jeff is attacking with Shotgun, targeted on Fred
Missed: Chance to hit: 72, (dice roll: 0)
0 XP added to character Jeff, for a total of 0 XP
";
            Assert.AreEqual(log2, encounter2.LogString);

            //Turn 2 - Team 1 starts
            //Fred shoots again, and kills Jeff.
            List<Character> characters3 = fred.GetCharactersInView(game.Map, new List<Team>() { team2 });
            Assert.AreEqual(characters3[0], jeff);
            int chanceToHit3 = EncounterCore.GetChanceToHit(fred, fred.WeaponEquipped, jeff);
            int chanceToCrit3 = EncounterCore.GetChanceToCrit(fred, fred.WeaponEquipped, jeff, game.Map, false);
            DamageOptions damageOptions3 = EncounterCore.GetDamageRange(fred, fred.WeaponEquipped);
            Assert.AreEqual(80, chanceToHit3);
            Assert.AreEqual(70, chanceToCrit3);
            Assert.AreEqual(3, damageOptions3.DamageLow);
            Assert.AreEqual(5, damageOptions3.DamageHigh);

            EncounterResult encounter3 = Encounter.AttackCharacter(fred,
                    fred.WeaponEquipped,
                    jeff,
                    game.Map,
                    diceRolls);
            string log3 = @"
Fred is attacking with Rifle, targeted on Jeff
Hit: Chance to hit: 80, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jeff, HP is now -5
Jeff is killed
100 XP added to character Fred, for a total of 110 XP
Fred is ready to level up
";
            Assert.AreEqual(log3, encounter3.LogString);

            //End of of battle

            //Assert
            Assert.AreEqual(-5,jeff.Hitpoints);
        }

    }
}

