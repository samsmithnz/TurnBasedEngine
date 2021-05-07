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
                Range = 18,
                LowDamageRange = 3,
                HighDamageRange = 5,
                CriticalChance = 20,
                CriticalDamage = 2,
                ClipSize = 4,
                ChanceToHitAdjustment = 10,
                Type = WeaponEnum.Standard
            };
            return rifle;
        }
    }
}