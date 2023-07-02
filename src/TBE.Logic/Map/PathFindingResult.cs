using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Map
{
    public class PathFindingResult
    {
        public PathFindingResult()
        {
            Tiles = new List<MapTile>();
            Path = new List<Vector3>();
        }

        public List<MapTile> Tiles { get; set; }
        public List<Vector3> Path { get; set; }

        public MapTile GetLastTile()
        {
            if (Tiles.Count > 0)
            {
                return Tiles[Tiles.Count - 1];
            }
            else
            {
                return null;
            }
        }
    }
}
