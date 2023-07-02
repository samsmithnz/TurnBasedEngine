namespace TBE.Logic.AbilitiesAndEffects
{
    public class Effect
    {
        public string Name { get; set; }
        public AbilityType Type { get; set; }
        public int Adjustment { get; set; }
        public int TurnExpiration { get; set; }
    }
}
