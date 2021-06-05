using Battle.Logic.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //Follow the missed shot to find the target
        public static Vector3 MissedShot(Vector3 source, Vector3 target, string[,,] map)
        {
            //Get the final missed location the projectile is heading down
            Vector3 finalLocation = GetMissedLocation(source, target, map);

            //Get all of the points along this line to the final location
            List<Vector3> points = GetPointsOnLine(source, finalLocation);

            //Check each point to see if it goes off the map, or hits another object
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 item = points[i];
                //If the item gets to the edge of the map - return the edge location
                if (((int)item.X > map.GetLength(0) - 1 ||
                    (int)item.Y > map.GetLength(1) - 1 ||
                    (int)item.Z > map.GetLength(2) - 1) &&
                    points.Count > 0)
                {
                    return points[i - 1];
                }
                //If the item hits something, return that position
                else if (map[(int)item.X, (int)item.Y, (int)item.Z] != "")
                {
                    return item;
                }
            }

            return Vector3.Zero;
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

            int xAdjustment = 1;
            if (xDifference < 0)
            {
                xAdjustment = -1;
            }
            int yAdjustment = 1;
            if (yDifference < 0)
            {
                yAdjustment = -1;
            }
            int zAdjustment = 1;
            if (zDifference < 0)
            {
                zAdjustment = -1;
            }
            //int x = (int)(target.X + xDifference);
            //int y = (int)(target.Y + yDifference);
            //int z = (int)(target.Z + zDifference);

            int xMultiplier = (int)Math.Ceiling((xMax - target.X) / xDifference);
            if (xDifference < 0)
            {
                xMultiplier = (int)Math.Ceiling((xMax - target.X) / xDifference * xAdjustment);
            }
            int yMultiplier = (int)Math.Ceiling((yMax - target.Y) / yDifference);
            if (yDifference < 0)
            {
                yMultiplier = (int)Math.Ceiling((yMax - target.Y) / yDifference * yAdjustment);
            }
            int zMultiplier = (int)Math.Ceiling((zMax - target.Z) / zDifference);
            if (zDifference < 0)
            {
                zMultiplier = (int)Math.Ceiling((zMax - target.Z) / zDifference * zAdjustment);
            }
            int xFinal = ((int)target.X + (xDifference * xMultiplier));
            //if (xFinal < 0)
            //{
            //    xFinal = 0;
            //}
            int yFinal = ((int)target.Y + (yDifference * yMultiplier));
            //if (yFinal < 0)
            //{
            //    yFinal = 0;
            //}
            int zFinal = ((int)target.Z + (zDifference * zMultiplier));
            //if (zFinal < 0)
            //{
            //    zFinal = 0;
            //}
            return new(xFinal, yFinal, zFinal);
        }

        //http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
        //https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        private static IEnumerable<Vector3> GetPointsOnLine(int x0, int z0, int x1, int z1)
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

        public static List<Vector3> GetPointsOnLine(Vector3 source, Vector3 target)
        {
            List<Vector3> points = GetPointsOnLine((int)source.X, (int)source.Z, (int)target.X, (int)target.Z).ToList<Vector3>();
            //Reverse the list
            //points.Reverse(); //TODO: uncommenting this doesn't work as expected...

            return points;
        }

    }
}
