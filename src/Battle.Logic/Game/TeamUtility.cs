using Battle.Logic.Characters;
using Battle.Logic.Utility;
using System.Collections.Generic;

namespace Battle.Logic.Game
{
    public static class TeamUtility
    {
        //Get the next character, from a list.
        //Ignore characters with no AP or HP. 
        //If at the end of the list, wrap to the start of the list

        //Return the index to the next character, and the character itself
        public static int GetNextCharacter(int currentIndex, List<string> characterNames, List<Character> teamCharacters)
        {
            List<Character> targetCharacters = new List<Character>();
            if (characterNames != null)
            {
                foreach (string item in characterNames)
                {
                    Character character = GetCharacter(targetCharacters, item);
                    if (character.ActionPointsCurrent > 0 && character.HitpointsCurrent > 0)
                    {
                        targetCharacters.Add(character);
                    }
                }
            }
            else
            {
                foreach (Character character in teamCharacters)
                {
                    if (character.ActionPointsCurrent > 0 && character.HitpointsCurrent > 0)
                    {
                        targetCharacters.Add(character);
                    }
                }
            }

            int index = WrappingList.FindNextIndex(currentIndex, teamCharacters);
            if (index >= 0)
            {
                currentIndex = index;
                return currentIndex;
            }
            else
            {
                return -1;
            }
        }

        //Return the index to the previous character, and the character itself
        public static int GetPreviousCharacter(int currentIndex, List<string> characterNames, List<Character> teamCharacters)
        {
            List<Character> targetCharacters = new List<Character>();
            if (characterNames != null)
            {
                foreach (string item in characterNames)
                {
                    Character character = GetCharacter(targetCharacters, item);
                    if (character.ActionPointsCurrent > 0 && character.HitpointsCurrent > 0)
                    {
                        targetCharacters.Add(character);
                    }
                }
            }
            else
            {
                foreach (Character character in teamCharacters)
                {
                    if (character.ActionPointsCurrent > 0 && character.HitpointsCurrent > 0)
                    {
                        targetCharacters.Add(character);
                    }
                }
            }

            int index = WrappingList.FindPreviousIndex(currentIndex, targetCharacters);
            if (index >= 0)
            {
                currentIndex = index;
                return currentIndex;
            }
            else
            {
                return -1;
            }
        }

        private static Character GetCharacter(List<Character> characters, string name)
        {
            Character result = null;
            foreach (Character character in characters)
            {
                if (character.Name == name)
                {
                    result = character;
                    break;
                }
            }
            return result;
        }
    }
}
