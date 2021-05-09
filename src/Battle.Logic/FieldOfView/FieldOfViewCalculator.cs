using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Logic.FieldOfView
{
    public class FieldOfViewCalculator
    {

        public static List<Vector3> GetFieldOfView(string[,] map, int startingX, int startingZ, int range)
        {
            //Use the range to find the borders in each primary direction from the starting location
            int minX = startingX - range;
            if (minX < 0)
            {
                minX = 0;
            }
            int maxX = startingX + range;
            if (maxX > map.GetLength(0) - 1)
            {
                maxX = map.GetLength(0)-1;
            }
            int minZ = startingZ - range;
            if (minZ < 0)
            {
                minZ = 0;
            }
            int maxZ = startingZ + range;
            if (maxZ > map.GetLength(0) - 1)
            {
                maxZ = map.GetLength(0) - 1;
            }

            //Get a list of all border squares
            HashSet<Vector3> borderTiles = new();
            //Add the top and bottom rows
            for (int i = minX; i <= maxX; i++)
            {
                borderTiles.Add(new(i, 0, minZ));
                borderTiles.Add(new(i, 0, maxZ));
            }
            //Add the left and right sides
            for (int i = minZ; i < maxZ; i++)
            {
                borderTiles.Add(new(minX, 0, i));
                borderTiles.Add(new(maxX, 0, i));
            }

            //For each border tile, draw a line from the starting point to the border
            HashSet<Vector3> results = new();
            foreach (Vector3 borderItem in borderTiles)
            {
                List<Vector3> singleLineCheck = FieldOfViewCalculator.GetPointsOnLine(startingX, startingZ, (int)borderItem.X, (int)borderItem.Z).ToList<Vector3>();
                foreach (Vector3 fovItem in singleLineCheck)
                {
                    //If we find an object, stop adding tiles
                    if (map[(int)fovItem.X, (int)fovItem.Z] != "")
                    {
                        break;
                    }
                    else if ((int)fovItem.X == startingX && (int)fovItem.Z == startingZ)
                    {
                        //Don't add this one, it's the origin
                    }
                    else
                    { 
                        results.Add(fovItem);
                    }
                }
            }

            return results.ToList();

            //Create a list of results for each boundry
            //Filter the list for items in the way.

            //List<Vector3> results = FieldOfViewCalculator.GetPointsOnLine(1, 3, 4, 2).ToList<Vector3>();
            //List<Vector3> newResults = new();
            //foreach (Vector3 item in results)
            //{
            //    if (map[(int)item.X, (int)item.Z] != "")
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        newResults.Add(item);
            //    }
            //}
            //return newResults;
        }

        //http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
        //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        public static IEnumerable<Vector3> GetPointsOnLine(int x0, int z0, int x1, int z1)
        {
            bool steep = Math.Abs(z1 - z0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = z0;
                z0 = t;
                t = x1; // swap x1 and y1
                x1 = z1;
                z1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = z0; // swap y0 and y1
                z0 = z1;
                z1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(z1 - z0);
            int error = dx / 2;
            int ystep = (z0 < z1) ? 1 : -1;
            int y = z0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new((steep ? y : x), 0, (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }
    }
}
