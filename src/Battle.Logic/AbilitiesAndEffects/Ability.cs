namespace Battle.Logic.AbilitiesAndEffects
{
    public class Ability
    {
        public Ability(string name, AbilityTypeEnum type, int adjustment)
        {
            Name = name;
            Type = type;
            Adjustment = adjustment;
        }

        public string Name { get; set; }
        public AbilityTypeEnum Type { get; set; }
        public int Adjustment { get; set; }
    }

}
