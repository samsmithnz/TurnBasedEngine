using System.Collections.Generic;
using System.Numerics;

namespace TBE.Logic.Map
{
    public static class PathFinding
    {
        private const int NEIGHBOR_COUNT = 8;

        private static readonly int[] _adjacentXOffsets = new int[NEIGHBOR_COUNT] { -1, -1, -1, 0, 1, 1, 1, 0 };
        private static readonly int[] _adjacentZOffsets = new int[NEIGHBOR_COUNT] { -1, 0, 1, 1, 1, 0, -1, -1 };

        private static int _width;
        private static int _height;
        private static int _breadth;
        private static string[,,] _map;
        private static MapTile[,,] _tiles;
        private static Vector3 _endLocation;

        /// <summary>
        /// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
        /// </summary>
        /// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
        public static PathFindingResult FindPath(string[,,] map, Vector3 startLocation, Vector3 endLocation)
        {
            _endLocation = endLocation;
            InitializeTiles(map);
            MapTile startTile = GetOrCreateTile((int)startLocation.X, (int)startLocation.Y, (int)startLocation.Z);
            startTile.State = TileState.Open;
            MapTile endTile = GetOrCreateTile((int)endLocation.X, (int)endLocation.Y, (int)endLocation.Z);

            // The start tile is the first entry in the 'open' list
            PathFindingResult result = new PathFindingResult();
            if (startLocation != endLocation && endTile.TileType != string.Empty)
            {
                return result;
            }

            bool success = Search(startTile, endTile);
            if (success)
            {
                // If a path was found, follow the parents from the end tile to build a list of locations
                MapTile tile = endTile;
                while (tile.ParentTile != null)
                {
                    result.Tiles.Add(tile);
                    result.Path.Add(tile.Location);
                    tile = tile.ParentTile;
                }

                // Reverse the list so it's in the correct order when returned
                result.Path.Reverse();
                result.Tiles.Reverse();
            }

            return result;
        }

        /// <summary>
        /// Builds the tile grid from a simple grid of booleans indicating areas which are and aren't walkable
        /// </summary>
        /// <param name="map">A boolean representation of a grid in which true = walkable and false = not walkable</param>
        private static void InitializeTiles(string[,,] map)
        {
            _map = map;
            _width = map.GetLength(0);
            _height = map.GetLength(1);
            _breadth = map.GetLength(2);
            _tiles = new MapTile[_width, _height, _breadth];
        }

        private static MapTile GetOrCreateTile(int x, int y, int z)
        {
            MapTile tile = _tiles[x, y, z];
            if (tile == null)
            {
                tile = new MapTile(x, y, z, _map[x, y, z], _endLocation);
                _tiles[x, y, z] = tile;
            }

            return tile;
        }

        /// <summary>
        /// Attempts to find a path to the destination tile using <paramref name="currentTile"/> as the starting location
        /// </summary>
        /// <param name="currentTile">The tile from which to find a path</param>
        /// <returns>True if a path to the destination has been found, otherwise false</returns>
        private static bool Search(MapTile currentTile, MapTile endTile)
        {
            // Set the current tile to Closed since it cannot be traversed more than once
            currentTile.State = TileState.Closed;
            List<MapTile> nextTiles = GetAdjacentWalkableTiles(currentTile);
            foreach (MapTile nextTile in nextTiles)
            {
                // Check whether the end tile has been reached
                if (nextTile == endTile)
                {
                    return true;
                }
                else
                {
                    // If not, check the next set of tiles
                    if (Search(nextTile, endTile)) // Note: Recurses back into Search(Tile)
                    {
                        return true;
                    }
                }
            }

            // The method returns false if this path leads to be a dead end
            return false;
        }

        /// <summary>
        /// Returns any tiles that are adjacent to <paramref name="fromTile"/> and may be considered to form the next step in the path
        /// </summary>
        /// <param name="fromTile">The tile from which to return the next possible tiles in the path</param>
        /// <returns>A list of next possible tiles in the path</returns>
        private static List<MapTile> GetAdjacentWalkableTiles(MapTile fromTile)
        {
            List<MapTile> walkableTiles = new List<MapTile>(NEIGHBOR_COUNT);
            int y = (int)fromTile.Location.Y;
            for (int i = 0; i < NEIGHBOR_COUNT; i++)
            {
                int x = (int)fromTile.Location.X + _adjacentXOffsets[i];
                int z = (int)fromTile.Location.Z + _adjacentZOffsets[i];

                // Stay within the grid's boundaries
                if (x < 0 || x >= _width || z < 0 || z >= _breadth)
                {
                    continue;
                }

                if (_map[x, y, z] != string.Empty)
                {
                    continue;
                }

                MapTile tile = GetOrCreateTile(x, y, z);
                // Ignore non-walkable tiles
                if (tile.TileType != "")
                {
                    continue;
                }

                // Ignore already-closed tiles
                if (tile.State == TileState.Closed)
                {
                    continue;
                }

                //// Already-open tiles are only added to the list if their G-value is lower going via this route.
                if (tile.State == TileState.Open)
                {
                    //float traversalCost = MapTile.GetTraversalCost(tile.Location, tile.ParentTile.Location);
                    //float gTemp = fromTile.G + traversalCost;
                    //if (gTemp < tile.G)
                    //{
                    //    tile.ParentTile = fromTile;
                    //    walkableTiles.Add(tile);
                    //}
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    tile.ParentTile = fromTile;
                    tile.State = TileState.Open;
                    InsertTileByFValue(walkableTiles, tile);
                }
            }

            return walkableTiles;
        }

        private static void InsertTileByFValue(List<MapTile> walkableTiles, MapTile tile)
        {
            int insertIndex = walkableTiles.Count;
            for (int i = 0; i < walkableTiles.Count; i++)
            {
                if (tile.F < walkableTiles[i].F)
                {
                    insertIndex = i;
                    break;
                }
            }

            walkableTiles.Insert(insertIndex, tile);
        }
    }
}
