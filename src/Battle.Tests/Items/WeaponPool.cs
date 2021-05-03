using Battle.Logic.Weapons;

namespace Battle.Tests.Weapons
{
    public static class WeaponPool
    {
        public static Weapon CreateSword()
        {
            Weapon sword = new()
            {
                Name = "Sword",
                Range = 1,
                DamageRange = 10,
                CriticalChance = 20
            };
            return sword;
        }
    }
}