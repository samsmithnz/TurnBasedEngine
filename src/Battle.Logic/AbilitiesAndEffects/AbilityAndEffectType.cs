namespace Battle.Logic.AbilitiesAndEffects
{
    public enum AbilityTypeEnum
    {
        Unknown = 0,
        Damage = 1, //The character causes more regular damage
        CriticalChance = 2, //The character has a better chance to crit
        CriticalDamage = 3, //The character causes more critical damage
        ArmorShredding = 4, //Removes armor 
        ArmorPiercing = 5, //Ignores armor
        FireDamage = 6, //fire damage. Spreads and causes damage over time for n turns
        PoisonDamage = 7 //poison damage. causes damage over time for n turns
    }
}
