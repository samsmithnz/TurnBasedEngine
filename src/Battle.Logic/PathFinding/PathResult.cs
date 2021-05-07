﻿using System.Collections.Generic;
using System.Drawing;

namespace Battle.PathFinding
{
    public class PathResult
    {

        public List<Tile> Tiles;
        public List<Point> Path;

        public PathResult()
        {
            Tiles = new List<Tile>();
            Path = new List<Point>();
        }

        public Tile GetLastTile()
        {
            if (Tiles.Count > 0)
            {
                return Tiles[^1]; //"^1" is the same asa "Tiles.Count - 1"
            }
            else
            {
                return null;
            }
        }
    }
}