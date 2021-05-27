﻿using Battle.Logic.Items;

namespace Battle.Tests.Items
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
                ClipRemaining = 4,
                ActionPointsRequired = 1,
                Type = WeaponType.Standard
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
                ClipRemaining = 4,
                ActionPointsRequired = 1,
                Type = WeaponType.Shotgun
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
                ClipRemaining = 4,
                ActionPointsRequired = 2,
                Type = WeaponType.SniperRifle
            };
            return shotgun;
        }
        public static Weapon CreateGrenade()
        {
            Weapon grenade = new()
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
                ClipSize = 1,
                ClipRemaining = 1,
                ActionPointsRequired = 1,
                Type = WeaponType.Grenade
            };
            return grenade;
        }

    }
}