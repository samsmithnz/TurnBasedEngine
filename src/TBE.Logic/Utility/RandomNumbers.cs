using System;
using System.Collections.Generic;

namespace TBE.Logic.Utility
{
    public static class RandomNumber
    {
        public static int GenerateRandomNumber(int minValue, int maxValue, int? seed = null)
        {
            if (seed == null)
            {
                Random rand = new Random();
                return rand.Next(minValue, maxValue);
            }
            else
            {
                Random rand = new Random((int)seed);
                return rand.Next(minValue, maxValue);
            }
        }

        public static List<int> GenerateRandomNumberList(int minValue, int maxValue, int seed = 0, int listSize = 10)
        {
            Random rand = new Random(seed);
            List<int> result = new List<int>();
            for (int i = 0; i < listSize; i++)
            {
                result.Add(rand.Next(minValue, maxValue));
            }
            return result;
        }

        public static int ScaleRandomNumber(int minValue, int maxValue, int value)
        {
            //The scale formula is: =(value - minValue) / (maxValue - minValue);
            int result = (value - 0) * (maxValue - minValue) / (100 - 0) + minValue;

            return result;
        }

    }
}
