namespace Battle.Logic.Weapons
{
    public class Weapon
    {
        public string Name { get; set; }
        public int ChanceToHitAdjustment { get; set; }
        public int Range { get; set; }
        public int AreaEffectRadius { get; set; }
        public int DamageRangeLow { get; set; }
        public int DamageRangeHigh { get; set; }
        public int CriticalChance { get; set; }
        public int CriticalDamageLow { get; set; }
        public int CriticalDamageHigh { get; set; }
        public int ClipSize { get; set; }
        public int ClipRemaining { get; set; }
        public int ActionPointsRequired { get; set; }
        public WeaponEnum Type { get; set; }
    }

    public enum WeaponEnum
    {
        Unknown = 0,
        Standard = 1,
        Shotgun = 2,
        SniperRifle = 3,
        Sword = 4,
        Grenade = 5
    }
}
