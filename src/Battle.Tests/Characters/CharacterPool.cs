using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Tests.Weapons;

namespace Battle.Tests.Characters
{
    public static class CharacterPool
    {
        public static Character CreateFredHero()
        {
            Character fred = new()
            {
                Name = "Fred",
                Hitpoints = 12,
                ArmorPoints = 0,
                ChanceToHit = 70,
                Experience = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 10,
                Location = new(0, 0, 0),
                ActionPoints = 2,
                Range = 10,
                WeaponEquipped = WeaponPool.CreateRifle(),
                UtilityItemEquipped = WeaponPool.CreateGrenade(),
                InHalfCover = false,
                InFullCover = false,
                InOverwatch = false
            };
            fred.Abilities.Add(new("Ability", AbilityTypeEnum.Unknown, 0));
            fred.Effects.Add(
                new()
                {
                    Name = "Fire",
                    Type = AbilityTypeEnum.FireDamage,
                    Adjustment = 1,
                    TurnExpiration = 2
                }
             );

            return fred;
        }

        public static Character CreateJeffBaddie()
        {
            Character fred = new()
            {
                Name = "Jeff",
                Hitpoints = 12,
                ArmorPoints = 0,
                ChanceToHit = 70,
                Experience = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 11,
                Location = new(8, 0, 8),
                ActionPoints = 2,
                Range = 10,
                WeaponEquipped = WeaponPool.CreateShotgun(),
                UtilityItemEquipped = WeaponPool.CreateGrenade(),
                InHalfCover = false,
                InFullCover = false,
                InOverwatch = false
            };
            return fred;
        }

        public static Character CreateHarryHeroSidekick()
        {
            Character harry = new()
            {
                Name = "Harry",
                Hitpoints = 12,
                ArmorPoints = 1,
                ChanceToHit = 70,
                Experience = 0,
                Level = 1,
                LevelUpIsReady = false,
                Speed = 12,
                Location = new(5, 0, 5),
                ActionPoints = 2,
                Range = 10,
                WeaponEquipped = WeaponPool.CreateSniperRifle(),
                UtilityItemEquipped = WeaponPool.CreateGrenade(),
                InHalfCover = true,
                InFullCover = false,
                InOverwatch = false
            };
            return harry;
        }
    }
}
