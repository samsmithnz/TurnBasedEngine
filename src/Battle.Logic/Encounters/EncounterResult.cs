using Battle.Logic.Characters;

namespace Battle.Logic.Encounters
{
    public class EncounterResult
    {
        public Character SourceCharacter { get; set; }
        public Character TargetCharacter { get; set; }
        public int DamageDealt { get; set; }
        public bool IsCriticalHit { get; set; }
    }
}
