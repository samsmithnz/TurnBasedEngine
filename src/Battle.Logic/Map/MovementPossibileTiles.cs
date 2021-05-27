using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Map
{
    public static class MovementPossibileTiles
    {
        public static List<Vector3> GetMovementPossibileTiles(string[,,] map, Vector3 sourceLocation, int range)
        {
            List<Vector3> possibleTiles = MapCore.GetMapArea(map, sourceLocation, range, false);
            List<Vector3> verifiedTiles = new();
            foreach (Vector3 item in possibleTiles)
            {
                PathFindingResult result = PathFinding.FindPath(sourceLocation, item, map);
                if (result.Tiles.Count > 0 && result.Tiles[^1].TraversalCost <= range)
                {
                    verifiedTiles.Add(item);
                }
            }
            return verifiedTiles;
        }
    }
}
