using System.Collections.Generic;

namespace Battle.Logic.GameController
{
    public class Game
    {
        public Game()
        {
            Teams = new();
        }

        public int TurnNumber { get; set; }
        public List<Team> Teams { get; set; }
        public string[,,] Map { get; set; }
    }
}
