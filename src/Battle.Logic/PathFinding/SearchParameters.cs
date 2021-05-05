using System.Drawing;

namespace Battle.PathFinding
{
    /// <summary>
    /// Defines the parameters which will be used to find a path across a section of the map
    /// </summary>
    public class SearchParameters
    {
        public Point StartingLocation { get; set; }
        public Point EndLocation { get; set; }
        public string[,] Map { get; set; }

        public SearchParameters(Point startingLocation, Point endLocation, string[,] map)
        {
            StartingLocation = startingLocation;
            EndLocation = endLocation;
            Map = map;
        }
    }
}
