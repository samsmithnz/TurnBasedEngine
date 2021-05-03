using Battle.Logic;

namespace Battle.Tests
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
