namespace Battle.Logic.AbilitiesAndEffects
{
    public enum AbilityType
    {
        Unknown = 0,
        Damage = 1, //The character causes more regular damage
        CriticalChance = 2, //Adjusts the chance to crit by [n]
        CriticalDamage = 3, //Adjusts the amount of critical damage by [n](when there is a critical hit)
        ArmorShredding = 4, //Removes [n] armor points
        ArmorPiercing = 5, //Ignores armor points
        FireDamage = 6, //Fire damage. Spreads and causes [n] damage over time for [m] turns
        PoisonDamage = 7, //poison damage. Causes [n] damage over time for [m] turns
        ExplosiveDamage = 8 // Causes an explosion for [n] damage with a radius [m]
    }
}
