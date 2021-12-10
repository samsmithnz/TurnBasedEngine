using Battle.Logic.Characters;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Game
{
    public class Team
    {
        public Team(int targetTeamIndex)
        {
            Characters = new List<Character>();
            TargetTeamIndex = targetTeamIndex;
        }

        public string Name { get; set; }
        public bool IsAITeam { get; set; }
        public int TargetTeamIndex { get; set; }
        public int CurrentCharacterIndex { get; set; }
        public List<Character> Characters { get; set; }
        public string Color { get; set; }
        public string[,,] FOVMap { get; set; }
        public HashSet<Vector3> FOVHistory { get; set; }

        public int GetNextCharacterIndex()
        {
            int result = TeamUtility.GetNextCharacter(CurrentCharacterIndex, Characters);
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
            int result = TeamUtility.GetPreviousCharacter(CurrentCharacterIndex, Characters);
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

        public int GetCharacterIndex(string name)
        {
            int result = -1;
            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].Name == name)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        public Character GetCharacter(int index)
        {
            Character result = null;
            if (index >= 0)
            {
                result = Characters[index];
            }
            return result;
        }

        public Character GetFirstCharacter()
        {
            foreach (Character character in Characters)
            {
                if (character.HitpointsCurrent > 0 && character.ActionPointsCurrent > 0)
                {
                    return character;
                }
            }
            return null;
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
