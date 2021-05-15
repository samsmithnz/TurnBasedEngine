namespace Battle.Logic.AbilitiesAndEffects
{
    public class Effect
    {
        public string Name { get; set; }
        public AbilityTypeEnum Type { get; set; }
        public int Adjustment { get; set; }
        public int TurnExpiration { get; set; }
    }
}
