using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Battle.Logic.FieldOfView
{
    public static class FieldOfViewCalculator
    {
        public static List<Vector3> GetFieldOfView(string[,] map, Vector3 location, int range)
        {
            int startingX = (int)location.X;
            int startingZ = (int)location.Z;
            //Use the range to find the borders in each primary direction from the starting location
            int minX = startingX - range;
            if (minX < 0)
            {
                minX = 0;
            }
            int maxX = startingX + range;
            if (maxX > map.GetLength(0) - 1)
            {
                maxX = map.GetLength(0) - 1;
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
                if (singleLineCheck.Count > 0 &&
                    singleLineCheck[^1].X == startingX &&
                    singleLineCheck[^1].Z == startingZ) // note that ^1 is the same as singleLineCheck.Count - 1
                {
                    //Reverse the list
                    singleLineCheck.Reverse();

                }
                foreach (Vector3 fovItem in singleLineCheck)
                {
                    //If we find an object, stop adding tiles
                    if (map[(int)fovItem.X, (int)fovItem.Z] != "")
                    {
                        break;
                    }
                    else if ((int)fovItem.X == startingX && (int)fovItem.Z == startingZ)
                    {
                        //Don't add this one, it's the origin/ where the character is looking from
                    }
                    else
                    {
                        results.Add(fovItem);
                    }
                }
            }

            return results.ToList();
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
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }


        public static string GetMapString(string[,] map, int xMax, int zMax)
        {
            StringBuilder sb = new();
            sb.Append(Environment.NewLine);
            for (int z = zMax - 1; z >= 0; z--)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (map[x, z] != "")
                    {
                        sb.Append(map[x, z] + " ");
                    }
                    else
                    {
                        sb.Append("□ ");
                    }
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static string[,] ApplyListToMap(string[,] map, List<Vector3> list, string tile)
        {
            foreach (Vector3 item in list)
            {
                if (map[(int)item.X, (int)item.Z] == "")
                {
                    map[(int)item.X, (int)item.Z] = tile;
                }
            }

            return map;
        }
    }
}
