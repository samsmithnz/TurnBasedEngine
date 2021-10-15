using Battle.Logic.Characters;
using System.Collections.Generic;

namespace Battle.Logic.Utility
{
    /// <summary>
    /// A wrapping list object, that allows you to move forward or backwards through a list, wrapping to the beginning/end, if required
    /// </summary>
    public static class WrappingList
    {
        //Move to the next item in the character list with action points
        public static int FindNextIndex(int index, List<Character> characters)
        {
            bool foundFreeCharacter = false;

            index++;
            if (index > characters.Count - 1)
            {
                index = 0;
            }

            for (int i = index; i <= characters.Count - 1; i++)
            {
                if (characters[i].ActionPointsCurrent > 0)
                {
                    foundFreeCharacter = true;
                    index = i;
                    break;
                }
            }
            if (foundFreeCharacter == false)
            {
                //look in the first half of the list
                for (int i = 0; i <= index; i++)
                {
                    if (characters[i].ActionPointsCurrent > 0)
                    {
                        foundFreeCharacter = true;
                        index = i;
                        break;
                    }
                }
            }

            if (foundFreeCharacter == false)
            {
                index = -1;
            }

            return index;
        }

        //Move to the previous item in the character list with action points
        public static int FindPreviousIndex(int index, List<Character> characters)
        {
            bool foundFreeCharacter = false;

            index--;
            if (index < 0)
            {
                index = characters.Count - 1;
            }

            for (int i = index; i >= 0; i--)
            {
                if (characters[i].ActionPointsCurrent > 0)
                {
                    foundFreeCharacter = true;
                    index = i;
                    break;
                }
            }
            if (foundFreeCharacter == false)
            {
                //look in the second half of the list
                for (int i = characters.Count - 1; i >= index; i--)
                {
                    if (characters[i].ActionPointsCurrent > 0)
                    {
                        foundFreeCharacter = true;
                        index = i;
                        break;
                    }
                }
            }

            if (foundFreeCharacter == false)
            {
                index = -1;
            }

            return index;
        }
    }
}
