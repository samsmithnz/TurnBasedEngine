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

            //Move to the next character
            index++;
            //if we overflow, move to the front of the list
            if (index > characters.Count - 1)
            {
                index = 0;
            }

            //Double check that this character has action points. If the do not, start to loop through the list of characters
            if (characters[index].ActionPointsCurrent > 0 && characters[index].HitpointsCurrent > 0)
            {
                foundFreeCharacter = true;
            }
            else
            {
                for (int i = index; i <= characters.Count - 1; i++)
                {
                    if (characters[i].ActionPointsCurrent > 0 && characters[i].HitpointsCurrent > 0)
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
                        if (characters[i].ActionPointsCurrent > 0 && characters[i].HitpointsCurrent > 0)
                        {
                            foundFreeCharacter = true;
                            index = i;
                            break;
                        }
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

            //Move to the previous character
            index--;
            //if we overflow, move to the end of the list
            if (index < 0)
            {
                index = characters.Count - 1;
            }

            //Double check that this character has action points and health. If the do not, start to loop through the list of characters
            if (characters[index].ActionPointsCurrent > 0 && characters[index].HitpointsCurrent > 0)
            {
                foundFreeCharacter = true;
            }
            else
            {
                for (int i = index; i >= 0; i--)
                {
                    if (characters[i].ActionPointsCurrent > 0 && characters[i].HitpointsCurrent > 0)
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
                        if (characters[i].ActionPointsCurrent > 0 && characters[i].HitpointsCurrent > 0)
                        {
                            foundFreeCharacter = true;
                            index = i;
                            break;
                        }
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
