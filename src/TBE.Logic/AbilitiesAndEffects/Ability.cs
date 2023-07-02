namespace TBE.Logic.AbilitiesAndEffects
{
    public class Ability
    {
        public Ability(string name, AbilityType type, int adjustment)
        {
            Name = name;
            Type = type;
            Adjustment = adjustment;
        }

        public string Name { get; set; }
        public AbilityType Type { get; set; }
        public int Adjustment { get; set; }
    }

}
