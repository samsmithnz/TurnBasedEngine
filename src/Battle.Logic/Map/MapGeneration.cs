using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Battle.Logic.Characters;
using Battle.Logic.Utility;

namespace Battle.Logic.Map
{
    public static class MapGeneration
    {
        public static string[,] GenerateRandomMap(string[,] map, int probOfMapBeingBlocked)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (((x != 0 && z != 0) || (x != width - 1 && z != height - 1)) && probOfMapBeingBlocked > RandomNumber.GenerateRandomNumber(1, 100))
                    {
                        map[x, z] = CoverType.FullCover;
                    }
                }
            }
            return map;
        }

        public static void DebugPrintOutMap(string[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, z] != "")
                    {
                        Console.WriteLine(" this.map[" + x + ", " + z + "] = "+ map[x, z] + ";");
                    }
                }
            }
        }
  

    }
}
