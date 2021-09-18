using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Tests.Items;
using System.Numerics;

namespace Battle.Tests.Characters
{
    public static class CharacterPool
    {
        public static Character CreateFredHero(string[,,] map, Vector3 location)
        {
            Character fred = new Character()
            {
                Name = "Fred",
                Background = "Fred is from Canada and is a nice guy.",
                HitpointsMax = 4,
                HitpointsCurrent = 4,
                ArmorPointsMax = 0,
                ArmorPointsCurrent = 0,
                ActionPointsMax = 2,
                ActionPointsCurrent = 2,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 10,
                MobilityRange = 8,
                ShootingRange = 30,
                FOVRange = 40,
                WeaponEquipped = WeaponPool.CreateRifle(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                UtilityItemEquipped = ItemPool.CreateMedKit(),
                InHalfCover = false,
                InFullCover = false,
                InOverwatch = false,
                HunkeredDown = false
            };
            fred.SetLocation(location, map);
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

        public static Character CreateJethroBaddie(string[,,] map, Vector3 location)
        {
            Character jethro = new Character()
            {
                Name = "Jethro",
                Background = "Jethro grew up on a farm, and likes ice cream",
                HitpointsMax = 4,
                HitpointsCurrent = 4,
                ArmorPointsMax = 0,
                ArmorPointsCurrent = 0,
                ActionPointsMax = 2,
                ActionPointsCurrent = 2,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 11,
                MobilityRange = 8,
                ShootingRange = 30,
                FOVRange = 40,
                WeaponEquipped = WeaponPool.CreateShotgun(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                InHalfCover = false,
                InFullCover = false,
                InOverwatch = false,
                HunkeredDown = false
            };
            jethro.SetLocation(location, map);
            return jethro;
        }

        public static Character CreateHarryHeroSidekick(string[,,] map, Vector3 location)
        {
            Character harry = new Character()
            {
                Name = "Harry",
                Background = "Harry worships Clint Eastwood movies",
                HitpointsCurrent = 12,
                ArmorPointsCurrent = 1,
                ChanceToHit = 70,
                XP = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 12,
                ActionPointsCurrent = 2,
                MobilityRange = 8,
                ShootingRange = 30,
                FOVRange = 40,
                WeaponEquipped = WeaponPool.CreateSniperRifle(),
                UtilityWeaponEquipped = WeaponPool.CreateGrenade(),
                InHalfCover = true,
                InFullCover = false,
                InOverwatch = false,
                HunkeredDown = false
            };
            harry.SetLocation(location, map);
            return harry;
        }
    }
}
