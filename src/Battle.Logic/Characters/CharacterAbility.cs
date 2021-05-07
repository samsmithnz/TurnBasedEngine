namespace Battle.Logic.Characters
{
    public class CharacterAbility
    {
        public CharacterAbility(string name, AbilityTypeEnum type, int adjustment)
        {
            Name = name;
            Type = type;
            Adjustment = adjustment;
        }

        public string Name { get; set; }
        public AbilityTypeEnum Type { get; set; }
        public int Adjustment { get; set; }
    }

    public enum AbilityTypeEnum
    {
        Unknown = 0,
        Damage = 1, //The character has skill that causes more damage
        CriticalDamage = 2
    }
}
