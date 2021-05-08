using System.Collections.Generic;

namespace Battle.Logic.Characters
{
    public class CharacterActions
    {
        //Get a list of current actions the character can take
        public List<string> GetActions(Character character)
        {
            if (character.ActionPoints <= 0)
            {
                return new ();
            }
            else
            {
                return new();
            }
        }
    }
}
