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

        //get the first character on the team with action points
        public Character GetCharacterWithActionPoints()
        {
            foreach (Character character in Characters)
            {
                if (character.ActionPointsCurrent > 0)
                {
                    return character;
                }
            }
            return null;
        }

        public void NextCharacter()
        {
            CurrentCharacterIndex = WrappingList.FindNextIndex(CurrentCharacterIndex, Characters);
        }

        public void PreviousCharacter()
        {
            CurrentCharacterIndex = WrappingList.FindPreviousIndex(CurrentCharacterIndex, Characters);
        }


    }
}
