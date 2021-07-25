using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Tests.Items;
using System.Numerics;

namespace Battle.Tests.Characters
{
    public static class CharacterPool
    {
        public static Character CreateFredHero(string[,,] map)
        {
            Character fred = new Character()
            {
                Name = "Fred",
                HitpointsMax = 4,
                HitpointsCurrent = 4,
                ArmorPointsMax = 0,
                ArmorPointsCurrent = 0,
                ActionPointsMax = 2,
                ActionPointsCurrent = 2,
                ChanceToHit = 70,
                Experience = 0,
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
            fred.SetLocation(new Vector3(0, 0, 0), map);
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

        public static Character CreateJeffBaddie(string[,,] map)
        {
            Character jeff = new Character()
            {
                Name = "Jeff",
                HitpointsMax = 4,
                HitpointsCurrent = 4,
                ArmorPointsMax = 0,
                ArmorPointsCurrent = 0,
                ActionPointsMax = 2,
                ActionPointsCurrent = 2,
                ChanceToHit = 70,
                Experience = 0,
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
            jeff.SetLocation(new Vector3(8, 0, 8), map);
            return jeff;
        }

        public static Character CreateHarryHeroSidekick(string[,,] map)
        {
            Character harry = new Character()
            {
                Name = "Harry",
                HitpointsCurrent = 12,
                ArmorPointsCurrent = 1,
                ChanceToHit = 70,
                Experience = 0,
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
            harry.SetLocation(new Vector3(5, 0, 5), map);
            return harry;
        }
    }
}
