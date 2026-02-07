using TBE.Logic.Utility;
using System;

namespace TBE.Logic.Map
{
    public static class MapGeneration
    {
        public static string[,,] GenerateRandomMap(string[,,] map, int probOfMapBeingBlocked)
        {
            int width = map.GetLength(GameConstants.X_DIMENSION_INDEX);
            int height = map.GetLength(GameConstants.Y_DIMENSION_INDEX);
            int breadth = map.GetLength(GameConstants.Z_DIMENSION_INDEX);
            int y = GameConstants.FLAT_MAP_Y_COORDINATE;
            for (int z = GameConstants.FIRST_INDEX; z < breadth; z++)
            {
                for (int x = GameConstants.FIRST_INDEX; x < width; x++)
                {
                    if (((x != GameConstants.FIRST_INDEX && z != GameConstants.FIRST_INDEX) || (x != width - GameConstants.LAST_INDEX_OFFSET && z != height - GameConstants.LAST_INDEX_OFFSET)) && probOfMapBeingBlocked > RandomNumber.GenerateRandomNumber(GameConstants.MAP_GEN_RANDOM_MIN, GameConstants.MAP_GEN_RANDOM_MAX))
                    {
                        map[x, y, z] = CoverType.FullCover;
                    }
                }
            }
            return map;
        }

        public static void DebugPrintOutMap(string[,,] map)
        {
            int width = map.GetLength(GameConstants.X_DIMENSION_INDEX);
            //int height = map.GetLength(1);
            int breadth = map.GetLength(GameConstants.Z_DIMENSION_INDEX);
            int y = GameConstants.FLAT_MAP_Y_COORDINATE;
            for (int z = GameConstants.FIRST_INDEX; z < breadth; z++)
            {
                for (int x = GameConstants.FIRST_INDEX; x < width; x++)
                {
                    if (map[x, y, z] != GameConstants.EMPTY_TILE)
                    {
                        Console.WriteLine(" this.map[" + x + ", " + z + "] = " + map[x, y, z] + ";");
                    }
                }
            }
        }


    }
}
