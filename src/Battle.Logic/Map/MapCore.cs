using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Map
{
    public static class MapCore
    {
        public static string[,,] InitializeMap(int xMax, int yMax, int zMax, string initialString = "")
        {
            string[,,] map = new string[xMax, yMax, zMax];

            //Initialize the map
            int y = 0;
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    map[x, y, z] = initialString;
                }
            }

            return map;
        }

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
            HashSet<Vector3> borderTiles = new HashSet<Vector3>();
            //Add the top and bottom rows
            for (int x = minX; x <= maxX; x++)
            {
                borderTiles.Add(new Vector3(x, 0, minZ));
                borderTiles.Add(new Vector3(x, 0, maxZ));
            }
            //Add the left and right sides
            for (int z = minZ; z < maxZ; z++)
            {
                borderTiles.Add(new Vector3(minX, 0, z));
                borderTiles.Add(new Vector3(maxX, 0, z));
            }

            //For each border tile, draw a line from the starting point to the border
            HashSet<Vector3> results = new HashSet<Vector3>();
            foreach (Vector3 borderItem in borderTiles)
            {
                List<Vector3> singleLineCheck = FieldOfView.GetPointsOnLine(new Vector3(startingX, 0, startingZ), borderItem);
                if (singleLineCheck.Count > 0 &&
                    singleLineCheck[singleLineCheck.Count - 1].X == startingX &&
                    singleLineCheck[singleLineCheck.Count - 1].Z == startingZ)
                {
                    //Reverse the list, so that items are in order from source to destination
                    singleLineCheck.Reverse();
                }
                double lineLength = GetLengthOfLine(singleLineCheck[0], singleLineCheck[singleLineCheck.Count - 1], 1);
                double lineSegment = lineLength / singleLineCheck.Count;
                double currentLength = 0;
                for (int i = 0; i < singleLineCheck.Count; i++)
                {
                    currentLength += lineSegment;
                    Vector3 fovItem = singleLineCheck[i];
                    if (fovItem.X >= 0 && fovItem.Y >= 0 && fovItem.Z >= 0)
                    {
                        //If we find an object, stop adding tiles
                        if (lookingForFOV && map[(int)fovItem.X, (int)fovItem.Y, (int)fovItem.Z] == CoverType.FullCover)
                        {
                            //Add the wall
                            results.Add(fovItem);
                            //Then break!
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
                    //We don't round, so this will extend the range a tiny part - but I think that is ok.
                    if (currentLength >= range)
                    {
                        break;
                    }
                }
            }
            if (includeSourceLocation)
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
            StringBuilder sb = new StringBuilder();
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
                //Check that the square is empty - we don't want to overwrite something that exists and only put a tile on an unused tile
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

        public static string GetMapStringWithItemValues(string[,,] mapTemplate, List<KeyValuePair<Vector3, int>> list)
        {
            string[,,] map = (string[,,])mapTemplate.Clone();
            foreach (KeyValuePair<Vector3, int> item in list)
            {
                map[(int)item.Key.X, (int)item.Key.Y, (int)item.Key.Z] = item.Value.ToString();
            }
            return MapCore.GetMapString(map);
        }

        public static string GetMapStringWithMapMask(string[,,] map, string[,,] mapMask)
        {
            int xMax = map.GetLength(0);
            //int yMax = map.GetLength(1);
            int zMax = map.GetLength(2);
            int xMaskMax = map.GetLength(0);
            //int yMaskMax = map.GetLength(1);
            int zMaskMax = map.GetLength(2);
            if (xMax != xMaskMax && zMax != zMaskMax)
            {
                throw new Exception("Mask map is not the same size as the parent map");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            int y = 0;
            for (int z = zMax - 1; z >= 0; z--)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (mapMask[x, y, z] != "")
                    {
                        sb.Append(mapMask[x, y, z] + " ");
                    }
                    else
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
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
