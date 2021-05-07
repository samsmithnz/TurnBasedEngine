using Battle.Logic.Weapons;

namespace Battle.Tests.Weapons
{
    public static class WeaponPool
    {
        public static Weapon CreateRifle()
        {
            Weapon rifle = new()
            {
                Name = "Rifle",
                ChanceToHitAdjustment = 10,
                Range = 18,
                DamageRangeLow = 3,
                DamageRangeHigh = 5,
                CriticalChance = 20,
                CriticalDamageLow = 5,
                CriticalDamageHigh = 7,
                ClipSize = 4,
                Type = WeaponEnum.Standard
            };
            return rifle;
        }

        public static Weapon CreateShotgun()
        {
            Weapon shotgun = new()
            {
                Name = "Shotgun",
                ChanceToHitAdjustment = 10,
                Range = 17,
                DamageRangeLow = 3,
                DamageRangeHigh = 5,
                CriticalChance = 20,
                CriticalDamageLow = 6,
                CriticalDamageHigh = 8,
                ClipSize = 4,
                Type = WeaponEnum.Shotgun
            };
            return shotgun;
        }

        public static Weapon CreateSniperRifle()
        {
            Weapon shotgun = new()
            {
                Name = "Sniper Rifle",
                ChanceToHitAdjustment = 10,
                Range = 50,
                DamageRangeLow = 3,
                DamageRangeHigh = 5,
                CriticalChance = 25,
                CriticalDamageLow = 6,
                CriticalDamageHigh = 8,
                ClipSize = 4,
                Type = WeaponEnum.SniperRifle
            };
            return shotgun;
        }

    }
}