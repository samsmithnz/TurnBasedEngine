namespace Battle.Logic.Weapons
{
    public class Weapon
    {
        public string Name { get; set; }
        public int ChanceToHitAdjustment { get; set; }
        public int CriticalChance { get; set; }
        public int Range { get; set; }
        public int DamageRange { get; set; }
    }
}
