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
            if (_queue.Count <= GameConstants.RANDOM_QUEUE_THRESHOLD)
            {
                //Generate random numbers without a fixed seed for better randomness
                for (int i = GameConstants.FIRST_INDEX; i < GameConstants.RANDOM_BATCH_SIZE; i++)
                {
                    _queue.Enqueue(RandomNumber.GenerateRandomNumber(GameConstants.PERCENTAGE_MIN, GameConstants.PERCENTAGE_MAX));
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
            if (_queue.Count > GameConstants.DEAD_HITPOINTS)
            {
                _queue.Dequeue();
                List<int> tempList = new List<int>(_queue);
                tempList.Insert(GameConstants.FIRST_INDEX, value);
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
