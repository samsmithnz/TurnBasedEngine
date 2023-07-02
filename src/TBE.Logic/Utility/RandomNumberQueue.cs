using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TBE.Logic.Utility
{
    public class RandomNumberQueue
    {
        private readonly List<int> _queue;
        public List<int> Queue
        {
            get
            {
                return _queue;
            }
        }

        public RandomNumberQueue()
        {
            _queue = new List<int>();
        }

        public RandomNumberQueue(List<int> list)
        {
            _queue = list;
        }

        public int Dequeue()
        {
            //If there are less than 100 items left in the list, generate 100 more
            if (_queue.Count <= 100)
            {
                //use the current time to generate a unique int
                long longSeed = DateTime.Now.Ticks;
                //Loop until the seed is a valid int
                while (longSeed < int.MaxValue && longSeed > int.MinValue)
                {
                    longSeed = DateTime.Now.Ticks;
                }
                List<int> newRandomNumberList = RandomNumber.GenerateRandomNumberList(0, 100, (int)longSeed, 100);
                for (int i = 0; i < newRandomNumberList.Count; i++)
                {
                    Queue.Add(newRandomNumberList[i]);
                }
            }

            int result = _queue[0];
            _queue.RemoveAt(0);
            return result;
        }

        public void Enqueue(int i)
        {
            _queue.Add(i);
        }

        [JsonIgnore]
        public int Count
        {
            get
            {
                return _queue.Count;
            }
        }
    }
}
