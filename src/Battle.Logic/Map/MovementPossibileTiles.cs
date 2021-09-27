using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Map
{
    public static class MovementPossibileTiles
    {
        public static List<KeyValuePair<Vector3, int>> GetMovementPossibileTiles(string[,,] map, Vector3 sourceLocation, int range, int actionPoints)
        {
            List<KeyValuePair<Vector3, int>> results = new List<KeyValuePair<Vector3, int>>();
            List<Vector3> verifiedTiles = new List<Vector3>();
            for (int i = 1; i <= actionPoints; i++)
            {
                List<Vector3> possibleTiles = MapCore.GetMapArea(map, sourceLocation, range * i, false);
                foreach (Vector3 item in possibleTiles)
                {
                    PathFindingResult result = PathFinding.FindPath(sourceLocation, item, map);
                    if (result.Tiles.Count > 0 && result.Tiles[result.Tiles.Count - 1].TraversalCost <= range * i && !verifiedTiles.Contains(item))
                    {
                        verifiedTiles.Add(item);
                        results.Add(new KeyValuePair<Vector3, int>(item, i));
                    }
                }
            }
            return results;
        }

        public static List<Vector3> ExtractVectorListFromKeyValuePair(List<KeyValuePair<Vector3, int>> list, int filter = 0)
        {
            List<Vector3> results = new List<Vector3>();
            foreach (KeyValuePair<Vector3, int> item in list)
            {
                if (filter == 0)
                {
                    results.Add(item.Key);
                }
                else if (item.Value == filter)
                {
                    results.Add(item.Key);
                }
            }
            return results;
        }
    }
}
