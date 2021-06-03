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

        //Get the final location - which will usually be just off the map
        public static Vector3 GetMissedLocation(Vector3 source, Vector3 target, string[,,] map)
        {
            int xMax = map.GetLength(0);
            int yMax = map.GetLength(1);
            int zMax = map.GetLength(2);

            int xDifference = (int)(target.X - source.X);
            int yDifference = (int)(target.Y - source.Y);
            int zDifference = (int)(target.Z - source.Z);

            int x = (int)(target.X + xDifference);
            int y = (int)(target.Y + yDifference);
            int z = (int)(target.Z + zDifference);

            //int x = (int)Math.Ceiling(xMax / target.X);
            //int y = (int)Math.Ceiling(yMax / target.Y);
            //int z = (int)Math.Ceiling(zMax / target.Z);
            int xFinal = x;// (int)target.X * x;
            if (xFinal < 0)
            {
                xFinal = 0;
            }
            int yFinal = y;//(int)target.Y * y;
            if (yFinal < 0)
            {
                yFinal = 0;
            }
            int zFinal = z;//(int)target.Z * z;
            if (zFinal < 0)
            {
                zFinal = 0;
            }
            return new(xFinal, yFinal, zFinal);
        }

        //http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
        //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        public static IEnumerable<Vector3> GetPointsOnLine(int x0, int z0, int x1, int z1)
        {
            bool steep = Math.Abs(z1 - z0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and z0
                x0 = z0;
                z0 = t;
                t = x1; // swap x1 and z1
                x1 = z1;
                z1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = z0; // swap z0 and z1
                z0 = z1;
                z1 = t;
            }
            int dx = x1 - x0;
            int dz = Math.Abs(z1 - z0);
            int error = dx / 2;
            int zstep = (z0 < z1) ? 1 : -1;
            int z = z0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new((steep ? z : x), 0, (steep ? x : z));
                error -= dz;
                if (error < 0)
                {
                    z += zstep;
                    error += dx;
                }
            }
            yield break;
        }

    }
}
