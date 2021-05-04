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
                Range = 1,
                DamageRange = 10,
                CriticalChance = 20
            };
            return rifle;
        }
    }
}