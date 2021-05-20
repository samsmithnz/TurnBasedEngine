using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.PathFinding
{
    public class PathResult
    {

        public PathResult()
        {
            Tiles = new();
            Path = new ();
        }

        public List<Tile> Tiles { get; set; }
        public List<Vector3> Path { get; set; }

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
