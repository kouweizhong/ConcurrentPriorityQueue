﻿using System;
using System.Collections;
using ConcurrentPriorityQueue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConcurrentPriorityQueueTests.FunctionalTests
{
    [TestClass]
    public class ConcurrentPriorityQueueTests
    {
        [TestMethod]
        public void Initialize()
        {
            var target = new ConcurrentPriorityQueue<string, int>(5);

            Assert.AreEqual(0, target.Count);
            Assert.AreEqual(5, target.Capacity);

            AssertEx.Throws<ArgumentOutOfRangeException>(() => target = new ConcurrentPriorityQueue<string, int>(0));
        }

        [TestMethod]
        public void Enqueue()
        {
            var target = new ConcurrentPriorityQueue<string, int>(4);

            Assert.AreEqual(0, target.Count);

            target.Enqueue("a", 1);
            Assert.AreEqual(1, target.Count);

            target.Enqueue("b", 2);
            Assert.AreEqual(2, target.Count);

            target.Enqueue("c", 3);
            Assert.AreEqual(3, target.Count);

            target.Enqueue("d", 4);
            Assert.AreEqual(4, target.Count);
        }

        [TestMethod]
        public void Dequeue()
        {
            var target = new ConcurrentPriorityQueue<string, int>(4);
            target.Enqueue("a", 1);
            target.Enqueue("b", 4);
            target.Enqueue("c", 3);
            target.Enqueue("d", 2);

            Assert.AreEqual(4, target.Count);
            Assert.AreEqual("b", target.Dequeue());
            Assert.AreEqual(3, target.Count);
            Assert.AreEqual("c", target.Dequeue());
            Assert.AreEqual(2, target.Count);
            Assert.AreEqual("d", target.Dequeue());
            Assert.AreEqual(1, target.Count);
            Assert.AreEqual("a", target.Dequeue());
            Assert.AreEqual(0, target.Count);

            AssertEx.Throws<InvalidOperationException>(() => target.Dequeue());
        }

        [TestMethod]
        public void GetEnumerator()
        {
            var target = new ConcurrentPriorityQueue<string, int>(6);
            target.Enqueue("a", 1);
            target.Enqueue("b", 2);
            target.Enqueue("c", 3);
            target.Enqueue("d", 4);
            target.Enqueue("e", 5);

            var enumerator1 = target.GetEnumerator();
            var enumerator2 = ((IEnumerable)target).GetEnumerator();

            Assert.AreNotEqual(enumerator1, enumerator2);

            string result = string.Join(",", target);
            Assert.AreEqual("e,d,c,b,a", result);
        }

        [TestMethod]
        public void EnqueueAfterExceedingCapacity()
        {
            var target = new ConcurrentPriorityQueue<string, int>(3);

            Assert.AreEqual(0, target.Count);

            target.Enqueue("a", 1);
            target.Enqueue("b", 2);
            target.Enqueue("c", 3);

            target.Enqueue("d", 4);
            Assert.AreEqual(4, target.Count);
            Assert.AreEqual(6, target.Capacity);
            string result = string.Join(",", target);
            Assert.AreEqual("d,c,b,a", result);

            target.Enqueue("e", 0);
            Assert.AreEqual(5, target.Count);
            result = string.Join(",", target);
            Assert.AreEqual("d,c,b,a,e", result);
        }

        [TestMethod]
        public void EnqueueDequeue()
        {
            var target = new ConcurrentPriorityQueue<string, int>(7);

            target.Enqueue("a", 7);
            target.Enqueue("b", 6);
            target.Enqueue("c", 5);
            Assert.AreEqual("a", target.Dequeue());
            target.Enqueue("d", 4);
            Assert.AreEqual("b", target.Dequeue());
            target.Enqueue("a", 7);
            Assert.AreEqual("a", target.Dequeue());
            Assert.AreEqual("c", target.Dequeue());
            Assert.AreEqual("d", target.Dequeue());
            Assert.AreEqual(0, target.Count);
        }

    }
}