using Battle.Logic.Characters;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.FieldOfView
{
    public class FieldOfViewAreaEffectCalculator
    {
        public static List<Character> GetCharactersInArea(List<Character> characters, string[,] map, Vector3 location, int radius)
        {
            List<Character> results = new();
            List<Vector3> area = GetAreaOfEffect(map, location, radius);
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
            int offsetX = (int)location.X - 4;
            int offsetZ = (int)location.Z - 4;

            List<Vector3> results = new() { location };
            if (radius >= 1)
            {
                results.Add(new(3 + offsetX, 0, 4 + offsetZ));
                results.Add(new(4 + offsetX, 0, 3 + offsetZ));
                results.Add(new(4 + offsetX, 0, 5 + offsetZ));
                results.Add(new(5 + offsetX, 0, 4 + offsetZ));
            }
            if (radius >= 2)
            {
                results.Add(new(2 + offsetX, 0, 3 + offsetZ));
                results.Add(new(2 + offsetX, 0, 4 + offsetZ));
                results.Add(new(2 + offsetX, 0, 5 + offsetZ));
                results.Add(new(3 + offsetX, 0, 2 + offsetZ));
                results.Add(new(3 + offsetX, 0, 3 + offsetZ));
                results.Add(new(3 + offsetX, 0, 5 + offsetZ));
                results.Add(new(3 + offsetX, 0, 6 + offsetZ));
                results.Add(new(4 + offsetX, 0, 2 + offsetZ));
                results.Add(new(4 + offsetX, 0, 6 + offsetZ));
                results.Add(new(5 + offsetX, 0, 2 + offsetZ));
                results.Add(new(5 + offsetX, 0, 3 + offsetZ));
                results.Add(new(5 + offsetX, 0, 5 + offsetZ));
                results.Add(new(5 + offsetX, 0, 6 + offsetZ));
                results.Add(new(6 + offsetX, 0, 3 + offsetZ));
                results.Add(new(6 + offsetX, 0, 4 + offsetZ));
                results.Add(new(6 + offsetX, 0, 5 + offsetZ));
            }
            if (radius >= 3)
            {
                results.Add(new(1 + offsetX, 0, 3 + offsetZ));
                results.Add(new(1 + offsetX, 0, 4 + offsetZ));
                results.Add(new(1 + offsetX, 0, 5 + offsetZ));
                results.Add(new(2 + offsetX, 0, 2 + offsetZ));
                results.Add(new(2 + offsetX, 0, 6 + offsetZ));
                results.Add(new(3 + offsetX, 0, 1 + offsetZ));
                results.Add(new(3 + offsetX, 0, 7 + offsetZ));
                results.Add(new(4 + offsetX, 0, 1 + offsetZ));
                results.Add(new(4 + offsetX, 0, 7 + offsetZ));
                results.Add(new(5 + offsetX, 0, 1 + offsetZ));
                results.Add(new(5 + offsetX, 0, 7 + offsetZ));
                results.Add(new(6 + offsetX, 0, 2 + offsetZ));
                results.Add(new(6 + offsetX, 0, 6 + offsetZ));
                results.Add(new(7 + offsetX, 0, 3 + offsetZ));
                results.Add(new(7 + offsetX, 0, 4 + offsetZ));
                results.Add(new(7 + offsetX, 0, 5 + offsetZ));
            }

            //Filter out records we don't need
            int startingX = (int)location.X;
            int startingZ = (int)location.Z;
            //Use the range to find the borders in each primary direction from the starting location
            int minX = startingX - radius;
            if (minX < 0)
            {
                minX = 0;
            }
            int maxX = startingX + radius;
            if (maxX > map.GetLength(0) - 1)
            {
                maxX = map.GetLength(0) - 1;
            }
            int minZ = startingZ - radius;
            if (minZ < 0)
            {
                minZ = 0;
            }
            int maxZ = startingZ + radius;
            if (maxZ > map.GetLength(1) - 1)
            {
                maxZ = map.GetLength(1) - 1;
            }
            List<Vector3> filteredResults = new();
            foreach (Vector3 item in results)
            {
                if (item.X >= minX & item.X <= maxX & item.Z >= minZ & item.Z <= maxZ)
                {
                    filteredResults.Add(item);
                }
            }

            return filteredResults;
        }

        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="centerX">
        /// The x center position of the circle.
        /// </param>
        /// <param name="centerY">
        /// The y center position of the circle.
        /// </param>
        /// <param name="radius">
        /// The radius of the circle.
        /// </param>  
        /// <param name="color">
        /// The color to use.
        /// </param>    
        //private static void DrawCircle(this GenericImage image, int centerX, int centerY, int radius)
        //{
        //    //The center of the circle and its radius.
        //    int x = 100;
        //    int y = 100;
        //    int r = 50;
        //    //This here is sin(45) but i just hard-coded it.
        //    float sinus = 0.70710678118;
        //    //This is the distance on the axis from sin(90) to sin(45). 
        //    int range = r / (2 * sinus);
        //    for (int i = r; i >= range; --i)
        //    {
        //        int j = sqrt(r * r - i * i);
        //        for (int k = -j; k <= j; k++)
        //        {
        //            //We draw all the 4 sides at the same time.
        //            PutPixel(x - k, y + i);
        //            PutPixel(x - k, y - i);
        //            PutPixel(x + i, y + k);
        //            PutPixel(x - i, y - k);
        //        }
        //    }
        //    //To fill the circle we draw the circumscribed square.
        //    range = r * sinus;
        //    for (int i = x - range + 1; i < x + range; i++)
        //    {
        //        for (int j = y - range + 1; j < y + range; j++)
        //        {
        //            PutPixel(i, j);
        //        }
        //    }
        //}
    }
}
