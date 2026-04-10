using TBE.Logic.AbilitiesAndEffects;
using TBE.Logic.Items;
using System.Numerics;

namespace TBE.Logic.Characters
{
    public static class CharacterPool
    {
        public static Character CreateFredHero(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character fred = new Character()
            {
                ID = "Fred01",
                Name = "Fred",
                Background = "Fred is from Canada and is a nice guy.",
                CharacterClass = CharacterClass.Support,
                HitpointsCurrent = 4,
                HitpointsMax = 4,
                ArmorPointsCurrent = 0,
                ArmorPointsMax = 0,
                ActionPointsCurrent = 2,
                ActionPointsMax = 2,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 10,
                Intelligence = 75,
                MobilityRange = 8,
                ShootingRange = 30,
                WeaponEquipped = WeaponPool.CreateRifle(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                UtilityItemEquipped = ItemPool.CreateMedKit(),
                InOverwatch = false,
                HunkeredDown = false,
                Status = CharacterStatus.Available,
                StatusRecoveryTime = 0
            };
            fred.SetLocationAndRange(map, startingLocation, fovRange, null);
            fred.Abilities.Add(new Ability("Ability", AbilityType.Unknown, 0));
            fred.Effects.Add(
                new Effect()
                {
                    Name = "Fire",
                    Type = AbilityType.FireDamage,
                    Adjustment = 1,
                    TurnExpiration = 2
                }
             );

            return fred;
        }

        public static Character CreateHarryHero(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character harry = new Character()
            {
                ID = "Harry01",
                Name = "Harry",
                Background = "Harry worships Clint Eastwood movies",
                CharacterClass = CharacterClass.Sniper,
                HitpointsCurrent = 12,
                HitpointsMax = 12,
                ArmorPointsCurrent = 1,
                ArmorPointsMax = 1,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 12,
                Intelligence = 75,
                ActionPointsCurrent = 2,
                ActionPointsMax = 2,
                MobilityRange = 8,
                ShootingRange = 30,
                WeaponEquipped = WeaponPool.CreateSniperRifle(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                InOverwatch = false,
                HunkeredDown = false,
                Status = CharacterStatus.Available,
                StatusRecoveryTime = 0
            };
            harry.SetLocationAndRange(map, startingLocation, fovRange, null);
            return harry;
        }

        public static Character CreateJeffHero(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character jeff = new Character()
            {
                ID = "Jeff01",
                Name = "Jeff",
                Background = "Jeff is a great guy",
                CharacterClass = CharacterClass.Assault,
                HitpointsCurrent = 12,
                HitpointsMax = 12,
                ArmorPointsCurrent = 1,
                ArmorPointsMax = 1,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 12,
                Intelligence = 75,
                ActionPointsCurrent = 2,
                ActionPointsMax = 2,
                MobilityRange = 8,
                ShootingRange = 30,
                WeaponEquipped = WeaponPool.CreateRifle(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                InOverwatch = false,
                HunkeredDown = false,
                Status = CharacterStatus.Available,
                StatusRecoveryTime = 0
            };
            jeff.SetLocationAndRange(map, startingLocation, fovRange, null);
            return jeff;
        }

        public static Character CreateJethroBaddie(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character jethro = new Character()
            {
                ID = "Jethro01",
                Name = "Jethro",
                Background = "Jethro grew up on a farm, and likes ice cream",
                CharacterClass = CharacterClass.Assault,
                HitpointsCurrent = 4,
                HitpointsMax = 4,
                ArmorPointsCurrent = 0,
                ActionPointsCurrent = 2,
                ActionPointsMax = 2,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 11,
                Intelligence = 25,
                MobilityRange = 8,
                ShootingRange = 30,
                WeaponEquipped = WeaponPool.CreateShotgun(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                InOverwatch = false,
                HunkeredDown = false,
                Status = CharacterStatus.Available,
                StatusRecoveryTime = 0
            };
            jethro.SetLocationAndRange(map, startingLocation, fovRange, null);
            return jethro;
        }

        public static Character CreateBartBaddie(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character bart = new Character()
            {
                ID = "Bart01",
                Name = "Bart",
                Background = "Bart is a naughty boy",
                CharacterClass = CharacterClass.Heavy,
                HitpointsCurrent = 4,
                HitpointsMax = 4,
                ArmorPointsCurrent = 0,
                ArmorPointsMax = 0,
                ActionPointsCurrent = 2,
                ActionPointsMax = 2,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 11,
                Intelligence = 25,
                MobilityRange = 8,
                ShootingRange = 30,
                WeaponEquipped = WeaponPool.CreateRifle(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                InOverwatch = false,
                HunkeredDown = false,
                Status = CharacterStatus.Available,
                StatusRecoveryTime = 0
            };
            bart.SetLocationAndRange(map, startingLocation, fovRange, null);
            return bart;
        }

        public static Character CreateDerekBaddie(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character derek = new Character()
            {
                ID = "Derek01",
                Name = "Derek",
                Background = "Derek does not like rules",
                CharacterClass = CharacterClass.Heavy,
                HitpointsCurrent = 4,
                HitpointsMax = 4,
                ArmorPointsCurrent = 0,
                ArmorPointsMax = 0,
                ActionPointsCurrent = 2,
                ActionPointsMax = 2,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 11,
                Intelligence = 25,
                MobilityRange = 8,
                ShootingRange = 30,
                WeaponEquipped = WeaponPool.CreateRifle(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                InOverwatch = false,
                HunkeredDown = false,
                Status = CharacterStatus.Available,
                StatusRecoveryTime = 0
            };
            derek.SetLocationAndRange(map, startingLocation, fovRange, null);
            return derek;
        }
    }
}
