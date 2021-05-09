using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Logic.FieldOfView
{
    public class FieldOfViewCalculator
    {

        //public static List<Vector3> GetFieldOfView(string[,] map, int startingX, int startingZ, int range)
        //{
        //    //Count out to each square based on range in each primary direction from the starting location
        //    //Get a list of all border squares
        //    //Create a list of results for each boundry
        //    //Filter the list for items in the way.

        //    List<Vector3> results = FieldOfViewCalculator.GetPointsOnLine(1, 3, 4, 2).ToList<Vector3>();
        //    List<Vector3> newResults = new();
        //    foreach (Vector3 item in results)
        //    {
        //        if (map[(int)item.X, (int)item.Z] != "" )
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            newResults.Add(item);
        //        }
        //    }
        //    return newResults;
        //}

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
