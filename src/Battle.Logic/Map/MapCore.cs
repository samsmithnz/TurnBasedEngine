using Battle.Logic.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Battle.Logic.Map
{
    public class MapCore
    {
        public static List<Vector3> GetMapArea(string[,] map, Vector3 sourceLocation, int range, bool lookingForFOV=true)
        {
            int startingX = (int)sourceLocation.X;
            int startingZ = (int)sourceLocation.Z;
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
                List<Vector3> singleLineCheck = FieldOfView.GetPointsOnLine(startingX, startingZ, (int)borderItem.X, (int)borderItem.Z).ToList<Vector3>();
                if (singleLineCheck.Count > 0 &&
                    singleLineCheck[^1].X == startingX &&
                    singleLineCheck[^1].Z == startingZ) // note that ^1 is the same as singleLineCheck.Count - 1
                {
                    //Reverse the list
                    singleLineCheck.Reverse();

                }
                double lineLength = GetLengthOfLine(singleLineCheck[0], singleLineCheck[^1], 1);
                double lineSegment = lineLength / singleLineCheck.Count;
                double currentLength = 0;
                for (int i = 0; i < singleLineCheck.Count; i++)
                {
                    currentLength += lineSegment;
                    Vector3 fovItem = singleLineCheck[i];
                    //If we find an object, stop adding tiles
                    if (lookingForFOV == true && map[(int)fovItem.X, (int)fovItem.Z] == CoverType.FullCover)
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
                    //We don't round, so this will extend the range a tiny part - but I think that is ok.
                    if (currentLength >= range)
                    {
                        break;
                    }
                }
            }

            return results.ToList();
        }

        public static double GetLengthOfLine(Vector3 start, Vector3 end, int decimals = 0)
        {
            double lineLength = Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Z - start.Z), 2));
            return Math.Round(lineLength, decimals);
        }
    }
}
