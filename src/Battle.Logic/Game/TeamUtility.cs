using Battle.Logic.Characters;
using Battle.Logic.Utility;
using System.Collections.Generic;

namespace Battle.Logic.Game
{
    public static class TeamUtility
    {
        //Return the index to the next character, and the character itself
        public static (int, Character) GetNextCharacter(int currentIndex, List<Character> characters)
        {
            int index = WrappingList.FindNextIndex(currentIndex, characters);
            if (index >= 0)
            {
                currentIndex = index;
                return (currentIndex, characters[currentIndex]);
            }
            else
            {
                return (-1, null);
            }
        }

        //Return the index to the previous character, and the character itself
        public static (int, Character) GetPreviousCharacter(int currentIndex, List<Character> characters)
        {
            int index = WrappingList.FindPreviousIndex(currentIndex, characters);
            if (index >= 0)
            {
                currentIndex = index;
                return (currentIndex, characters[currentIndex]);
            }
            else
            {
                return (-1, null);
            }
        }
    }
}
