using Battle.Logic.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battle.Logic.Encounters
{
    public class EncounterResult
    {
        public EncounterResult()
        {
            AllCharacters = new List<Character>();
            Log = new List<string>();
        }

        public Character SourceCharacter { get; set; }
        private Character _targetCharacter;
        public Character TargetCharacter
        {
            get
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
        public int ArmorShredded { get; set; }
        public int ArmorAbsorbed { get; set; }
        public bool IsCriticalHit { get; set; }
        public List<string> Log { get; set; }
        public string LogString
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append(Environment.NewLine);
                foreach (string item in Log)
                {
                    result.Append(item);
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
        }
    }
}
