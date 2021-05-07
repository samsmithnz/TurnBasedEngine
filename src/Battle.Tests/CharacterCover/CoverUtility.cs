﻿using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.CharacterCover
{
    public class CoverUtility
    {
        public static string[,] InitializeMap(int xMax, int zMax, List<Vector3> coverLocations)
        {
            string[,] map = new string[xMax, zMax];
            
            //Initialize the map
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    map[x, z] = "";
                }
            }

            //assign cover locations, (currently just "W", for "wall")
            if (coverLocations != null && coverLocations.Count > 0)
            {
                foreach (Vector3 item in coverLocations)
                {
                    map[(int)item.X, (int)item.Z] = "W";
                }
            }

            return map;
        }

    }
}