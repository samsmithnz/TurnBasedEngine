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
    }
}
