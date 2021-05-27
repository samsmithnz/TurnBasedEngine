namespace Battle.Tests.Map
{
    public class MapUtility
    {
        public static string[,,] InitializeMap(int xMax, int yMax, int zMax)
        {
            string[,,] map = new string[xMax, yMax, zMax];

            //Initialize the map
            int y = 0;
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    map[x, y, z] = "";
                }
            }

            return map;
        }
    }
}
