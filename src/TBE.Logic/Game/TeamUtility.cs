using TBE.Logic.Characters;
using TBE.Logic.Utility;
using System.Collections.Generic;

namespace TBE.Logic.Game
{
    public static class TeamUtility
    {
        //Get the next character, from a list.
        //Ignore characters with no AP or HP. 
        //If at the end of the list, wrap to the start of the list

        //Return the index to the next character, and the character itself
        public static int GetNextCharacter(int currentIndex, List<Character> teamCharacters)
        {
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
        public static int GetPreviousCharacter(int currentIndex, List<Character> teamCharacters)
        {
            int index = WrappingList.FindPreviousIndex(currentIndex, teamCharacters);
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

    }
}
