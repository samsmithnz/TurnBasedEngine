using System.Collections.Generic;

namespace Battle.Logic.GameController
{
    public class Mission
    {
        public Mission()
        {
            Teams = new();
        }

        public int TurnNumber { get; set; }
        public List<Team> Teams { get; set; }
        public string[,,] Map { get; set; }

        //Start mission

        //End mission, add records
    }
}
