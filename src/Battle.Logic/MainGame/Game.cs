using System.Collections.Generic;

namespace Battle.Logic.MainGame
{
    public class Game
    {
        public int TurnNumber { get; set; }
        public List<Team> Teams { get; set; }
        public string[,] Map { get; set; }
    }
}
