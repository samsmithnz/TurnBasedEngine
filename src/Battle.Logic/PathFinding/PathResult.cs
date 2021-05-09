using System.Collections.Generic;
using System.Numerics;

namespace Battle.PathFinding
{
    public class PathResult
    {

        public List<Tile> Tiles;
        public List<Vector3> Path;

        public PathResult()
        {
            Tiles = new();
            Path = new ();
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
