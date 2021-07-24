using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Map;
using Battle.Tests.Characters;
using Battle.Tests.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Encounters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class AreaAttackTests
    {

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJeffTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map);
            jeff.HitpointsCurrent = 4;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJeffWearingArmorTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map); 
            jeff.HitpointsCurrent = 2;
            jeff.ArmorPointsCurrent = 2;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(2, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HitpointsCurrent);
            Assert.AreEqual(2, jeff.ArmorPointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
Armor prevented 2 damage to character Jeff
2 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJeffWearingArmorThisShreddedTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.ShredderAbility());
            fred.SetLocation(new Vector3(2, 0, 0), map); 
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map); 
            jeff.HitpointsCurrent = 4;
            jeff.ArmorPointsCurrent = 2;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HitpointsCurrent);
            Assert.AreEqual(0, jeff.ArmorPointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
2 armor points shredded
4 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndHurtsJeffWearingArmorWithShreddedTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.Abilities.Add(AbilityPool.ShredderAbility());
            fred.SetLocation(new Vector3(2, 0, 0), map); 
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map);
            jeff.HitpointsCurrent = 4;
            jeff.ArmorPointsCurrent = 3;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(3, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, jeff.HitpointsCurrent);
            Assert.AreEqual(1, jeff.ArmorPointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
2 armor points shredded
Armor prevented 1 damage to character Jeff
3 damage dealt to character Jeff, HP is now 1
10 XP added to character Fred, for a total of 10 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndInjuriesJeffTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map); 
            jeff.HitpointsCurrent = 5;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, jeff.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jeff, HP is now 1
10 XP added to character Fred, for a total of 10 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJeffAndHarryTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "H" = enemy/harry
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ H . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map);
            jeff.HitpointsCurrent = 4;
            Character harry = CharacterPool.CreateHarryHeroSidekick(map);
            harry.SetLocation(new Vector3(3, 0, 3), map);
            harry.HitpointsCurrent = 4;
            harry.ArmorPointsCurrent = 0;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff, harry };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(8, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HitpointsCurrent);
            Assert.AreEqual(0, harry.HitpointsCurrent);
            Assert.AreEqual(200, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff,  Harry
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Harry, HP is now 0
Harry is killed
100 XP added to character Fred, for a total of 200 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJeffAndHarryIsSavedByArmorTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "H" = enemy/harry
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ H . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map); 
            jeff.HitpointsCurrent = 4;
            Character harry = CharacterPool.CreateHarryHeroSidekick(map);
            harry.SetLocation(new Vector3(3, 0, 3), map); 
            harry.HitpointsCurrent = 4;
            harry.ArmorPointsCurrent = 1;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff, harry };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HitpointsCurrent);
            Assert.AreEqual(1, harry.HitpointsCurrent);
            Assert.AreEqual(110, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff,  Harry
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
Armor prevented 1 damage to character Harry
3 damage dealt to character Harry, HP is now 1
10 XP added to character Fred, for a total of 110 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndInjuriesJeffAndKillsHarryTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  "H" = enemy/harry
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ H . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map); 
            jeff.HitpointsCurrent = 15;
            Character harry = CharacterPool.CreateHarryHeroSidekick(map);
            harry.SetLocation(new Vector3(3, 0, 3), map); 
            harry.HitpointsCurrent = 4;
            harry.ArmorPointsCurrent = 0;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff, harry };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(8, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(4, fred.HitpointsCurrent);
            Assert.AreEqual(0, harry.HitpointsCurrent);
            Assert.AreEqual(110, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff,  Harry
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jeff, HP is now 11
10 XP added to character Fred, for a total of 10 XP
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Harry, HP is now 0
Harry is killed
100 XP added to character Fred, for a total of 110 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeWithCriticalAbilityAndKillsJeffTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . . . .
            //  . E ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            fred.Abilities.Add(AbilityPool.BiggestBoomsAbility1());
            fred.Abilities.Add(AbilityPool.BiggestBoomsAbility2());
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map); 
            jeff.HitpointsCurrent = 6;
            Character harry = CharacterPool.CreateHarryHeroSidekick(map);
            harry.SetLocation(new Vector3(3, 0, 3), map); 
            harry.HitpointsCurrent = 4;
            harry.ArmorPointsCurrent = 0;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff, harry };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.AllCharacters != null);
            Assert.AreEqual(10, result.DamageDealt);
            //Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, jeff.HitpointsCurrent);
            Assert.AreEqual(0, harry.HitpointsCurrent);
            Assert.IsTrue(result.TargetCharacter != null);
            Assert.IsTrue(result.AllCharacters.Count > 0);
            Assert.AreEqual(200, result.SourceCharacter.Experience);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff,  Harry
Damage range: 3-4, (dice roll: 100)
Critical chance: 20, (dice roll: 80)
Critical damage range: 5-6, (dice roll: 100)
6 damage dealt to character Jeff, HP is now 0
Jeff is killed
100 XP added to character Fred, for a total of 100 XP
Damage range: 3-4, (dice roll: 100)
Critical chance: 20, (dice roll: 0)
4 damage dealt to character Harry, HP is now 0
Harry is killed
100 XP added to character Fred, for a total of 200 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FOVFredThrowsGrenadeAndDestoriesCoverUpdatingFOVTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jeff
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  . . E . .
            //  . . ■ . . 
            //  . . . . . 
            //  . . . . .
            //  . . P . .
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(2, 0, 4), map); 
            jeff.HitpointsCurrent = 5;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff };

            //Act 1: get the FOV
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, fred.Location, 10);

            //Assert: check the initial FOV
            Assert.AreEqual(85, fov.Count);
            bool foundItem = false;
            foreach (Vector3 item in fov)
            {
                if (item.X == 2 && item.Z == 3)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == false);
            foundItem = false;
            foreach (Vector3 item in fov)
            {
                if (item.X == 2 && item.Z == 4)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == false);
            //Assert.AreEqual(new Vector3(1, 0, 3), results[0]);

            //Act 2: Now destroy the cover
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);
            fov = FieldOfView.GetFieldOfView(map, fred.Location, 10);

            //Assert 2: Check the FOV now
            Assert.AreEqual(98, fov.Count);
            foundItem = false;
            foreach (Vector3 item in fov)
            {
                if (item.X == 2 && item.Z == 3)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == true);
            foundItem = false;
            foreach (Vector3 item in fov)
            {
                if (item.X == 2 && item.Z == 4)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == true);

            //Assert 3: Check the area attack result 
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, jeff.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.Experience);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jeff
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jeff, HP is now 1
10 XP added to character Fred, for a total of 10 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void NoDiceRollsEncounterTest()
        {
            //Arrange
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map);
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            Queue<int> diceRolls = null;
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result == null);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void NoItemAvailableToThrowTest()
        {
            //Arrange
            string[,,] map = MapUtility.InitializeMap(10, 1, 10);
            map[2, 0, 3] = CoverType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map);
            fred.SetLocation(new Vector3(2, 0, 0), map);
            fred.UtilityWeaponEquipped = null;
            Character jeff = CharacterPool.CreateJeffBaddie(map);
            jeff.SetLocation(new Vector3(1, 0, 3), map); 
            jeff.HitpointsCurrent = 4;
            Queue<int> diceRolls = new Queue<int>(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new Vector3(2, 0, 4);
            List<Character> characterList = new List<Character>() { fred, jeff };

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(fred, fred.UtilityWeaponEquipped, characterList, map, diceRolls, targetThrowingLocation);

            //Assert
            Assert.IsTrue(result == null);
        }

    }
}