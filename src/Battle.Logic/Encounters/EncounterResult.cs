using Battle.Logic.Characters;
using System.Collections.Generic;

namespace Battle.Logic.Encounters
{
    public class EncounterResult
    {
        public Character SourceCharacter { get; set; }
        private Character _targetCharacter;
        public Character TargetCharacter { get
            { 
                if (_targetCharacter == null)
                {
                    _targetCharacter = AllCharacters[0];
                }
                return _targetCharacter;
            }
            set
            {
                _targetCharacter = value;
            } 
        }
        public List<Character> AllCharacters { get; set; }
        public int DamageDealt { get; set; }
        public bool IsCriticalHit { get; set; }
    }
}
