namespace Battle.Logic.Weapons
{
    public class Weapon
    {
        public string Name { get; set; }
        public int ChanceToHitAdjustment { get; set; }
        public int Range { get; set; }
        public int LowDamageRange { get; set; }
        public int HighDamageRange { get; set; }
        public int CriticalChance { get; set; }
        public int CriticalDamage { get; set; }
        public int ClipSize { get; set; }
        public WeaponEnum Type { get; set; }
    }

    public enum WeaponEnum
    {
        Unknown = 0,
        Standard = 1,
        Shotgun = 2,
        Sniper = 3,
        Sword = 4
    }
}
