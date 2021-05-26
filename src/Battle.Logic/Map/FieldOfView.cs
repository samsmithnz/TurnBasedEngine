using Battle.Logic.Characters;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Map
{
    public static class FieldOfView
    {
        public static List<Vector3> GetFieldOfView(string[,,] map, Vector3 location, int range)
        {
            return MapCore.GetMapArea(map, location, range, true);
        }

        public static List<Character> GetCharactersInArea(List<Character> characters, string[,,] map, Vector3 location, int range)
        {
            List<Character> results = new();
            List<Vector3> area = MapCore.GetMapArea(map, location, range, false, true);
            foreach (Character character in characters)
            {
                foreach (Vector3 item in area)
                {
                    if (character.Location == item)
                    {
                        results.Add(character);
                    }
                }
            }
            return results;
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

    }
}
