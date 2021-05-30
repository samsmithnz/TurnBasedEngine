﻿using Battle.Logic.Characters;
using System.Collections.Generic;

namespace Battle.Logic.GameController
{
    public class Team
    {
        public Team()
        {
            Characters = new();
        }

        public string Name { get; set; }
        public List<Character> Characters { get; set; }
    }
}