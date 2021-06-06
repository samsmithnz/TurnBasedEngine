using Battle.Logic.Items;

namespace Battle.Tests.Items
{
    public static class WeaponPool
    {
        public static Weapon CreateRifle()
        {
            Weapon rifle = new Weapon()
            {
                Name = "Rifle",
                ChanceToHitAdjustment = 10,
                Range = 18,
                DamageRangeLow = 3,
                DamageRangeHigh = 5,
                CriticalChance = 20,
                CriticalDamageLow = 5,
                CriticalDamageHigh = 7,
                AmmoMax = 4,
                AmmoCurrent = 4,
                ActionPointsRequired = 1,
                Type = WeaponType.Standard
            };
            return rifle;
        }

        public static Weapon CreateShotgun()
        {
            Weapon shotgun = new Weapon()
            {
                Name = "Shotgun",
                ChanceToHitAdjustment = 10,
                Range = 17,
                DamageRangeLow = 3,
                DamageRangeHigh = 5,
                CriticalChance = 20,
                CriticalDamageLow = 6,
                CriticalDamageHigh = 8,
                AmmoMax = 4,
                AmmoCurrent = 4,
                ActionPointsRequired = 1,
                Type = WeaponType.Shotgun
            };
            return shotgun;
        }

        public static Weapon CreateSniperRifle()
        {
            Weapon shotgun = new Weapon()
            {
                Name = "Sniper Rifle",
                ChanceToHitAdjustment = 10,
                Range = 50,
                DamageRangeLow = 3,
                DamageRangeHigh = 5,
                CriticalChance = 25,
                CriticalDamageLow = 6,
                CriticalDamageHigh = 8,
                AmmoMax = 4,
                AmmoCurrent = 4,
                ActionPointsRequired = 2,
                Type = WeaponType.SniperRifle
            };
            return shotgun;
        }
        public static Weapon CreateGrenade()
        {
            Weapon grenade = new Weapon()
            {
                Name = "Grenade",
                ChanceToHitAdjustment = 0,
                Range = 10,
                AreaEffectRadius = 3,
                DamageRangeLow = 3,
                DamageRangeHigh = 4,
                CriticalChance = 0,
                CriticalDamageLow = 0,
                CriticalDamageHigh = 0,
                AmmoMax = 1,
                AmmoCurrent = 1,
                ActionPointsRequired = 1,
                Type = WeaponType.Grenade
            };
            return grenade;
        }

    }
}