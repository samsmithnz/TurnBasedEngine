using System.Collections.Generic;
using System.Drawing;

namespace Battle.PathFinding
{

    public class Path
    {
        private int _width;
        private int _height;
        private Tile[,] _tiles;
        private readonly Tile _startTile;
        private readonly Tile _endTile;
        private readonly SearchParameters _searchParameters;
        private readonly Point _endLocation;

        /// <summary>
        /// Create a new instance of PathFinder
        /// </summary>
        /// <param name="searchParameters"></param>
        public Path(SearchParameters searchParameters)
        {
            _searchParameters = searchParameters;
            _endLocation = searchParameters.EndLocation;
            InitializeTiles(searchParameters.Map);
            _startTile = _tiles[searchParameters.StartingLocation.X, searchParameters.StartingLocation.Y];
            _startTile.State = TileState.Open;
            _endTile = _tiles[searchParameters.EndLocation.X, searchParameters.EndLocation.Y];
        }

        public Path(Point startingLocation, Point endLocation, string[,] map)
        {
            _endLocation = endLocation;
            InitializeTiles(map);
            _startTile = _tiles[startingLocation.X, startingLocation.Y];
            _startTile.State = TileState.Open;
            _endTile = _tiles[endLocation.X, endLocation.Y];
        }

        /// <summary>
        /// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
        /// </summary>
        /// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
        public PathResult FindPath()
        {
            // The start tile is the first entry in the 'open' list
            PathResult result = new();
            bool success = Search(_startTile);
            if (success)
            {
                // If a path was found, follow the parents from the end tile to build a list of locations
                Tile tile = _endTile;
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
        private void InitializeTiles(string[,] map)
        {
            _width = map.GetLength(0);
            _height = map.GetLength(1);
            _tiles = new Tile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _tiles[x, y] = new Tile(x, y, map[x, y], _endLocation);
                }
            }
        }

        /// <summary>
        /// Attempts to find a path to the destination tile using <paramref name="currentTile"/> as the starting location
        /// </summary>
        /// <param name="currentTile">The tile from which to find a path</param>
        /// <returns>True if a path to the destination has been found, otherwise false</returns>
        private bool Search(Tile currentTile)
        {
            // Set the current tile to Closed since it cannot be traversed more than once
            currentTile.State = TileState.Closed;
            List<Tile> nextTiles = GetAdjacentWalkableTiles(currentTile);

            // Sort by F-value so that the shortest possible routes are considered first
            nextTiles.Sort((tile1, tile2) => tile1.F.CompareTo(tile2.F));
            foreach (var nextTile in nextTiles)
            {
                // Check whether the end tile has been reached
                if (nextTile.Location == _endTile.Location)
                {
                    return true;
                }
                else
                {
                    // If not, check the next set of tiles
                    if (Search(nextTile)) // Note: Recurses back into Search(Tile)
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
        private List<Tile> GetAdjacentWalkableTiles(Tile fromTile)
        {
            List<Tile> walkableTiles = new();
            IEnumerable<Point> nextLocations = GetAdjacentLocations(fromTile.Location);

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                // Stay within the grid's boundaries
                if (x < 0 || x >= _width || y < 0 || y >= _height)
                {
                    continue;
                }

                Tile tile = _tiles[x, y];
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

                // Already-open tiles are only added to the list if their G-value is lower going via this route.
                if (tile.State == TileState.Open)
                {
                    float traversalCost = Tile.GetTraversalCost(tile.Location, tile.ParentTile.Location);
                    float gTemp = fromTile.G + traversalCost;
                    if (gTemp < tile.G)
                    {
                        tile.ParentTile = fromTile;
                        walkableTiles.Add(tile);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    tile.ParentTile = fromTile;
                    tile.State = TileState.Open;
                    walkableTiles.Add(tile);
                }
            }

            return walkableTiles;
        }

        /// <summary>
        /// Returns the eight locations immediately adjacent (orthogonally and diagonally) to <paramref name="fromLocation"/>
        /// </summary>
        /// <param name="fromLocation">The location from which to return all adjacent points</param>
        /// <returns>The locations as an IEnumerable of Points</returns>
        private static IEnumerable<Point> GetAdjacentLocations(Point fromLocation)
        {
            return new Point[]
            {
                new Point(fromLocation.X - 1, fromLocation.Y - 1),
                new Point(fromLocation.X - 1, fromLocation.Y  ),
                new Point(fromLocation.X - 1, fromLocation.Y + 1),
                new Point(fromLocation.X,   fromLocation.Y + 1),
                new Point(fromLocation.X + 1, fromLocation.Y + 1),
                new Point(fromLocation.X + 1, fromLocation.Y  ),
                new Point(fromLocation.X + 1, fromLocation.Y - 1),
                new Point(fromLocation.X,   fromLocation.Y - 1)
            };
        }
    }
}
