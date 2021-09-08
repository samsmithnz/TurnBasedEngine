using Battle.Logic.Characters;
using System.Collections.Generic;

namespace Battle.Logic.GameController
{
    public class Team
    {
        public Team()
        {
            Characters = new List<Character>();
        }

        public string Name { get; set; }
        public List<Character> Characters { get; set; }
        public string Color { get; set; }
        public string[,,] TeamFOVMap { get; set; }

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
    }
}
