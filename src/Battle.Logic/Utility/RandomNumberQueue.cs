using System;
using System.Collections;
using System.Collections.Generic;

namespace Battle.Logic.Utility
{
    public class RandomNumberQueue
    {
        private readonly List<int> list;

        public RandomNumberQueue()
        {
            list = new List<int>();
        }

        public RandomNumberQueue(List<int> l)
        {
            list = l;
        }

        public int Dequeue()
        {
            //If there are less than 100 items left in the list, generate 100 more
            if (list.Count <= 100)
            {
                //use the current time to generate a unique int
                long longSeed = DateTime.Now.Ticks;
                //Loop until the seed is a valid int
                while (longSeed < int.MaxValue && longSeed > int.MinValue)
                {
                    longSeed = DateTime.Now.Ticks;
                }
                List<int> newRandomNumberList = RandomNumber.GenerateRandomNumberList(0, 100, (int)longSeed, 100);
                for (int i = 0; i < 100; i++)
                {
                    list.Add(newRandomNumberList[i]);
                }
            }

            int result = list[0];
            list.RemoveAt(0);
            return result;
        }

        public void Enqueue(int i)
        {
            list.Add(i);
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }
    }
}
