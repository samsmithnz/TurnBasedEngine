using Battle.Logic.Characters;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Map
{
    public static class FieldOfViewAreaEffect
    {
        public static List<Character> GetCharactersInArea(List<Character> characters, string[,] map, Vector3 location, int radius)
        {
            List<Character> results = new();
            List<Vector3> area = MapCore.GetMapArea(map, location, radius,false, true);
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

        //I started down a path of implementing bresenhams-circle-algorithm, but it's complex and there wasn't an open source version to lift
        //Practically, most range will be 3 squares - for now, I'm just going to hardcode the 31 tiles involved
        public static List<Vector3> GetAreaOfEffect(string[,] map, Vector3 location, int radius)
        {
            List<Vector3> results = MapCore.GetMapArea(map, location, radius);
            return results;
            //int offsetX = (int)location.X - 4;
            //int offsetZ = (int)location.Z - 4;

            //List<Vector3> results = new() { location };
            //if (radius >= 1)
            //{
            //    results.Add(new(3 + offsetX, 0, 4 + offsetZ));
            //    results.Add(new(4 + offsetX, 0, 3 + offsetZ));
            //    results.Add(new(4 + offsetX, 0, 5 + offsetZ));
            //    results.Add(new(5 + offsetX, 0, 4 + offsetZ));
            //}
            //if (radius >= 2)
            //{
            //    results.Add(new(2 + offsetX, 0, 3 + offsetZ));
            //    results.Add(new(2 + offsetX, 0, 4 + offsetZ));
            //    results.Add(new(2 + offsetX, 0, 5 + offsetZ));
            //    results.Add(new(3 + offsetX, 0, 2 + offsetZ));
            //    results.Add(new(3 + offsetX, 0, 3 + offsetZ));
            //    results.Add(new(3 + offsetX, 0, 5 + offsetZ));
            //    results.Add(new(3 + offsetX, 0, 6 + offsetZ));
            //    results.Add(new(4 + offsetX, 0, 2 + offsetZ));
            //    results.Add(new(4 + offsetX, 0, 6 + offsetZ));
            //    results.Add(new(5 + offsetX, 0, 2 + offsetZ));
            //    results.Add(new(5 + offsetX, 0, 3 + offsetZ));
            //    results.Add(new(5 + offsetX, 0, 5 + offsetZ));
            //    results.Add(new(5 + offsetX, 0, 6 + offsetZ));
            //    results.Add(new(6 + offsetX, 0, 3 + offsetZ));
            //    results.Add(new(6 + offsetX, 0, 4 + offsetZ));
            //    results.Add(new(6 + offsetX, 0, 5 + offsetZ));
            //}
            //if (radius >= 3)
            //{
            //    results.Add(new(1 + offsetX, 0, 3 + offsetZ));
            //    results.Add(new(1 + offsetX, 0, 4 + offsetZ));
            //    results.Add(new(1 + offsetX, 0, 5 + offsetZ));
            //    results.Add(new(2 + offsetX, 0, 2 + offsetZ));
            //    results.Add(new(2 + offsetX, 0, 6 + offsetZ));
            //    results.Add(new(3 + offsetX, 0, 1 + offsetZ));
            //    results.Add(new(3 + offsetX, 0, 7 + offsetZ));
            //    results.Add(new(4 + offsetX, 0, 1 + offsetZ));
            //    results.Add(new(4 + offsetX, 0, 7 + offsetZ));
            //    results.Add(new(5 + offsetX, 0, 1 + offsetZ));
            //    results.Add(new(5 + offsetX, 0, 7 + offsetZ));
            //    results.Add(new(6 + offsetX, 0, 2 + offsetZ));
            //    results.Add(new(6 + offsetX, 0, 6 + offsetZ));
            //    results.Add(new(7 + offsetX, 0, 3 + offsetZ));
            //    results.Add(new(7 + offsetX, 0, 4 + offsetZ));
            //    results.Add(new(7 + offsetX, 0, 5 + offsetZ));
            //}

            ////Filter out records we don't need
            //int startingX = (int)location.X;
            //int startingZ = (int)location.Z;
            ////Use the range to find the borders in each primary direction from the starting location
            //int minX = startingX - radius;
            //if (minX < 0)
            //{
            //    minX = 0;
            //}
            //int maxX = startingX + radius;
            //if (maxX > map.GetLength(0) - 1)
            //{
            //    maxX = map.GetLength(0) - 1;
            //}
            //int minZ = startingZ - radius;
            //if (minZ < 0)
            //{
            //    minZ = 0;
            //}
            //int maxZ = startingZ + radius;
            //if (maxZ > map.GetLength(1) - 1)
            //{
            //    maxZ = map.GetLength(1) - 1;
            //}
            //List<Vector3> filteredResults = new();
            //foreach (Vector3 item in results)
            //{
            //    if (item.X >= minX && item.X <= maxX && item.Z >= minZ && item.Z <= maxZ)
            //    {
            //        filteredResults.Add(item);
            //    }
            //}

            //return filteredResults;
        }
       
    }
}
