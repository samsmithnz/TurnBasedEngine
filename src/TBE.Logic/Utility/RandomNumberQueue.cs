using Newtonsoft.Json;
using System.Collections.Generic;

namespace TBE.Logic.Utility
{
    public class RandomNumberQueue
    {
        private readonly Queue<int> _queue;
        public IEnumerable<int> Queue
        {
            get
            {
                return _queue;
            }
        }

        public RandomNumberQueue()
        {
            _queue = new Queue<int>();
        }

        public RandomNumberQueue(IEnumerable<int> items)
        {
            _queue = new Queue<int>(items);
        }

        public int Dequeue()
        {
            //If there are less than 100 items left in the queue, generate 100 more
            if (_queue.Count <= 100)
            {
                //Generate random numbers without a fixed seed for better randomness
                for (int i = 0; i < 100; i++)
                {
                    _queue.Enqueue(RandomNumber.GenerateRandomNumber(0, 100));
                }
            }

            return _queue.Dequeue();
        }

        public void Enqueue(int i)
        {
            _queue.Enqueue(i);
        }

        public void ReplaceFirstValue(int value)
        {
            if (_queue.Count > 0)
            {
                _queue.Dequeue();
                List<int> tempList = new List<int>(_queue);
                tempList.Insert(0, value);
                _queue.Clear();
                foreach (int item in tempList)
                {
                    _queue.Enqueue(item);
                }
            }
            else
            {
                _queue.Enqueue(value);
            }
        }

        public int Peek()
        {
            return _queue.Peek();
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
