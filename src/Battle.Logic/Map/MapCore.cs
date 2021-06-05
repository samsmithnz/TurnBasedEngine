using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Map
{
    public static class MapCore
    {
        public static List<Vector3> GetMapArea(string[,,] map, Vector3 sourceLocation, int range, bool lookingForFOV = true, bool includeSourceLocation = false)
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
            if (maxZ > map.GetLength(2) - 1)
            {
                maxZ = map.GetLength(2) - 1;
            }

            //Get a list of all border squares
            HashSet<Vector3> borderTiles = new();
            //Add the top and bottom rows
            for (int x = minX; x <= maxX; x++)
            {
                borderTiles.Add(new(x, 0, minZ));
                borderTiles.Add(new(x, 0, maxZ));
            }
            //Add the left and right sides
            for (int z = minZ; z < maxZ; z++)
            {
                borderTiles.Add(new(minX, 0, z));
                borderTiles.Add(new(maxX, 0, z));
            }

            //For each border tile, draw a line from the starting point to the border
            HashSet<Vector3> results = new();
            foreach (Vector3 borderItem in borderTiles)
            {
                List<Vector3> singleLineCheck = FieldOfView.GetPointsOnLine(new(startingX, 0, startingZ), borderItem);
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
                    if (lookingForFOV == true && map[(int)fovItem.X, (int)fovItem.Y, (int)fovItem.Z] == CoverType.FullCover)
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

            if (includeSourceLocation == true)
            {
                results.Add(sourceLocation);
            }

            return results.ToList();
        }

        public static double GetLengthOfLine(Vector3 start, Vector3 end, int decimals = 0)
        {
            double lineLength = Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Z - start.Z), 2));
            return Math.Round(lineLength, decimals);
        }

        public static string GetMapString(string[,,] map)
        {
            int xMax = map.GetLength(0);
            //int yMax = map.GetLength(1);
            int zMax = map.GetLength(2);
            StringBuilder sb = new();
            sb.Append(Environment.NewLine);
            int y = 0;
            for (int z = zMax - 1; z >= 0; z--)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (map[x, y, z] != "")
                    {
                        sb.Append(map[x, y, z] + " ");
                    }
                    else
                    {
                        sb.Append(". ");
                    }
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static string[,,] ApplyListToMap(string[,,] map, List<Vector3> list, string tile)
        {
            foreach (Vector3 item in list)
            {
                if (map[(int)item.X, (int)item.Y, (int)item.Z] == "")
                {
                    map[(int)item.X, (int)item.Y, (int)item.Z] = tile;
                }
            }
            return map;
        }

        public static string[,,] ApplyListToExistingMap(string[,,] map, List<Vector3> list, string tile)
        {
            foreach (Vector3 item in list)
            {
                map[(int)item.X, (int)item.Y, (int)item.Z] = tile;
            }
            return map;
        }

        public static string GetMapStringWithItems(string[,,] map, List<Vector3> list)
        {
            string[,,] mapNew = MapCore.ApplyListToMap((string[,,])map.Clone(), list, "o");
            string mapString = MapCore.GetMapString(mapNew);
            return mapString;
        }

        public static string GetMapStringWithItemLayers(string[,,] map, List<Vector3> baseList, List<Vector3> overlayList)
        {
            string[,,] mapNew = MapCore.ApplyListToMap((string[,,])map.Clone(), baseList, "o");
            mapNew = MapCore.ApplyListToExistingMap(mapNew, overlayList, "O");

            string mapString = MapCore.GetMapString(mapNew);
            return mapString;

        }
    }
}
