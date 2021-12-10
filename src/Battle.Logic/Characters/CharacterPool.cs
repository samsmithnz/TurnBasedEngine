using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Items;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public static class CharacterPool
    {
        public static Character CreateFredHero(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character fred = new Character()
            {
                Name = "Fred",
                Background = "Fred is from Canada and is a nice guy.",
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
                HunkeredDown = false
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
                Name = "Harry",
                Background = "Harry worships Clint Eastwood movies",
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
                HunkeredDown = false
            };
            harry.SetLocationAndRange(map, startingLocation, fovRange, null);
            return harry;
        }

        public static Character CreateJeffHero(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character jeff = new Character()
            {
                Name = "Jeff",
                Background = "Jeff is a great guy",
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
                HunkeredDown = false
            };
            jeff.SetLocationAndRange(map, startingLocation, fovRange, null);
            return jeff;
        }

        public static Character CreateJethroBaddie(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character jethro = new Character()
            {
                Name = "Jethro",
                Background = "Jethro grew up on a farm, and likes ice cream",
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
                HunkeredDown = false
            };
            jethro.SetLocationAndRange(map, startingLocation, fovRange, null);
            return jethro;
        }

        public static Character CreateBartBaddie(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character bart = new Character()
            {
                Name = "Bart",
                Background = "Bart is a naughty boy",
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
                HunkeredDown = false
            };
            bart.SetLocationAndRange(map, startingLocation, fovRange, null);
            return bart;
        }

        public static Character CreateDerekBaddie(string[,,] map, Vector3 startingLocation, int fovRange = 40)
        {
            Character derek = new Character()
            {
                Name = "Derek",
                Background = "Derek does not like rules",
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
                HunkeredDown = false
            };
            derek.SetLocationAndRange(map, startingLocation, fovRange, null);
            return derek;
        }
    }
}
