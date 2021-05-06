using System;
using System.Collections.Generic;

namespace Battle.Logic.Utility
{
    public static class RandomNumber
    {
        public static int GenerateRandomNumber(int minValue, int maxValue, int? seed = null)
        {
            if (seed == null)
            {
                Random rand = new();
                return rand.Next(minValue, maxValue);
            }
            else
            {
                Random rand = new((int)seed);
                return rand.Next(minValue, maxValue);
            }
        }

        public static List<int> GenerateRandomNumberList(int minValue, int maxValue, int seed = 0, int listSize = 10)
        {
            Random rand = new(seed);
            List<int> result = new(seed);
            for (int i = 0; i < listSize; i++)
            {
                result.Add(rand.Next(minValue, maxValue));
            }
            return result;
        }

    }
}
