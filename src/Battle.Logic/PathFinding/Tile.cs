using System;
using System.Drawing;

namespace Battle.PathFinding
{
    public class Tile
    {
        /// <summary>
        /// Creates a new instance of Node.
        /// </summary>
        /// <param name="x">The node's location along the X axis</param>
        /// <param name="y">The node's location along the Y axis</param>
        /// <param name="isWalkable">True if the node can be traversed, false if the node is a wall</param>
        /// <param name="endLocation">The location of the destination node</param>
        public Tile(int x, int y, string tileType, Point endLocation)
        {
            this.Location = new Point(x, y);
            this.State = TileState.Untested;
            this.TileType = tileType;
            this.H = GetTraversalCost(this.Location, endLocation);
            this.G = 0;
        }

        /// <summary>
        /// The node's location in the grid
        /// </summary>
        public Point Location { get; private set; }

        /// <summary>
        /// True when the node may be traversed, otherwise false
        /// </summary>
        /// //formerly IsWalkable
        public string TileType { get; set; }

        /// <summary>
        /// Cost from start to here
        /// </summary>
        public float G { get; private set; }

        /// <summary>
        /// Estimated cost from here to end
        /// </summary>
        public float H { get; private set; }

        /// <summary>
        /// Flags whether the node is open, closed or untested by the PathFinder
        /// </summary>
        public TileState State { get; set; }

        /// <summary>
        /// Estimated total cost (F = G + H)
        /// </summary>
        public float F
        {
            get { return this.G + this.H; }
        }

        public int TraversalCost
        {
            get { return Convert.ToInt32(F); }
        }

        private Tile _parentTile;
        /// <summary>
        /// Gets or sets the parent node. The start node's parent is always null.
        /// </summary>
        public Tile ParentTile
        {
            get
            {
                return this._parentTile;
            }
            set
            {
                // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
                this._parentTile = value;
                this.G = this._parentTile.G + GetTraversalCost(this.Location, this._parentTile.Location);
            }
        }

        //public override string ToString()
        //{
        //    return string.Format("{0}, {1}: {2}", this.Location.X, this.Location.Y, this.State);
        //}

        /// <summary>
        /// Gets the distance between two points
        /// </summary>
        internal static float GetTraversalCost(Point location, Point otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }

    /// <summary>
    /// Represents the search state of a Node
    /// </summary>
    public enum TileState
    {
        /// <summary>
        /// The node has not yet been considered in any possible paths
        /// </summary>
        Untested,
        /// <summary>
        /// The node has been identified as a possible step in a path
        /// </summary>
        Open,
        /// <summary>
        /// The node has already been included in a path and will not be considered again
        /// </summary>
        Closed
    }
}
