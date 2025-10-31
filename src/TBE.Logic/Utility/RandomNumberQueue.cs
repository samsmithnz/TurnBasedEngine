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
                //Generate random numbers without a fixed seed for better randomness
                for (int i = 0; i < 100; i++)
                {
                    Queue.Add(RandomNumber.GenerateRandomNumber(0, 100));
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
