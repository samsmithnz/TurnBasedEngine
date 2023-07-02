using TBE.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TBE.Tests.Utility
{
    [TestClass]
    [TestCategory("L0")]
    public class QueueTests
    {
        [TestMethod]
        public void Queue3ItemsTest()
        {
            //Arrange
            Queue<int> queue = new();

            //Act
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            int item1 = queue.Dequeue();
            int item2 = queue.Dequeue();
            int item3 = queue.Dequeue();

            //Assert
            Assert.AreEqual(1, item1);
            Assert.AreEqual(2, item2);
            Assert.AreEqual(3, item3);
        }

        [TestMethod]
        public void Queue500ItemsListTest()
        {
            //Arrange
            Queue<int> queue;
            List<int> list = new();

            //Act
            for (int i = 0; i < 500; i++)
            {
                list.Add(i);
            }
            queue = new(list);

            //Assert
            Assert.AreEqual(500, queue.Count);

            for (int i = 0; i < 500; i++)
            {
                queue.Dequeue();
            }
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void NewQueue3ItemsTest()
        {
            //Arrange
            RandomNumberQueue queue = new();

            //Act
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            int item1 = queue.Dequeue();
            int item2 = queue.Dequeue();
            int item3 = queue.Dequeue();

            //Assert
            Assert.AreEqual(1, item1);
            Assert.AreEqual(2, item2);
            Assert.AreEqual(3, item3);
        }

        [TestMethod]
        public void NewQueue500ItemsListTest()
        {
            //Arrange
            RandomNumberQueue queue;
            List<int> list = new();

            //Act
            for (int i = 0; i < 500; i++)
            {
                list.Add(i);
            }
            queue = new(list);

            //Assert
            Assert.AreEqual(500, queue.Count);

            for (int i = 0; i < 501; i++)
            {
                queue.Dequeue();
            }
            Assert.AreEqual(199, queue.Count);
        }

        [TestMethod]
        public void QueueWith0ItemsListTest()
        {
            //Arrange
            RandomNumberQueue queue;

            //Act
            queue = new();

            //Assert
            Assert.AreEqual(0, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(99, queue.Count);
        }

        //[TestMethod]
        //public void QueueOverflowTest()
        //{
        //    //Arrange
        //    RandomNumberQueue queue;

        //    //Act
        //    queue = new();

        //    //Assert
        //    Assert.AreEqual(0, queue.Count);
        //    queue.Dequeue();
        //    Assert.AreEqual(99, queue.Count);
        //}

    }
}
