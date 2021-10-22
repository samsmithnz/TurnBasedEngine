using Battle.Logic.Characters;
using Battle.Logic.Utility;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Game
{
    public class Team
    {
        public Team()
        {
            Characters = new List<Character>();
        }

        public string Name { get; set; }
        public bool IsAITeam { get; set; }
        public int CurrentCharacterIndex { get; set; }
        public List<Character> Characters { get; set; }
        public string Color { get; set; }
        public string[,,] FOVMap { get; set; }
        public HashSet<Vector3> FOVHistory { get; set; }

        public int GetNextCharacterIndex()
        {
            int result = TeamUtility.GetNextCharacter(CurrentCharacterIndex, null, Characters);
            if (result >= 0)
            {
                CurrentCharacterIndex = result;
                return result;
            }
            else
            {
                return -1;
            }
        }

        public int GetPreviousCharacterIndex()
        {
            int result = TeamUtility.GetPreviousCharacter(CurrentCharacterIndex, null, Characters);
            if (result >= 0)
            {
                CurrentCharacterIndex = result;
                return result;
            }
            else
            {
                return -1;
            }
        }

        public Character GetCharacter(string name)
        {
            Character result = null;
            foreach (Character character in Characters)
            {
                if (character.Name == name)
                {
                    result = character;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Needed after setup - only
        /// </summary>
        public void UpdateTargets(string[,,] map, List<Character> opponentCharacters)
        {
            foreach (Character character in Characters)
            {
                character.SetLocationAndRange(map, character.Location, character.FOVRange, opponentCharacters);
            }
        }

    }
}
