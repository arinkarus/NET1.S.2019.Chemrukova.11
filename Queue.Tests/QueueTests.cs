using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Queue;

namespace Queue.Tests
{
    public class QueueTests
    {
        [Test]
        public void CreateQueue_NegativeCapacity_ThrowArgumentException() =>
           Assert.Throws<ArgumentException>(() => new Queue.Queue<int>(-10));

        [Test]
        public void CreateQueue_CollectionIsNull_ThrowArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new Queue.Queue<int>(null));

        [Test]
        public void CreateQueue_Collection_IncreaseCountProperty()
        {
            var list = new List<string>() { "hello", "world" };
            var queue = new Queue<string>(list);
            Assert.AreEqual(list.Count, queue.Count);
        }

        [Test]
        public void Enqueue_TwoElementGiven_ReturnCount()
        {
            var queue = new Queue<int>();
            queue.Enqueue(5);
            queue.Enqueue(1111);
            Assert.AreEqual(2, queue.Count);
        }

        [Test]
        public void Dequeue_QueueIsEmpty_ThrowsInvalidOperationException()
        {
            var queue = new Queue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Test]
        public void Peek_QueueIsEmpty_ThrowsInvalidOperationException()
        {
            var queue = new Queue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        [Test]
        public void Peek_QueueHasElements_ReturnElement()
        {
            var queue = new Queue<char>();
            queue.Enqueue('a');
            queue.Enqueue('b');
            queue.Enqueue('c');
            char element = queue.Peek();
            Assert.IsTrue(element == 'a' && queue.Count == 3);
        }

        [Test]
        public void Dequeue_QueueHasElements_ReturnAndRemoveElement()
        {
            var queue = new Queue<char>();
            queue.Enqueue('a');
            queue.Enqueue('b');
            queue.Enqueue('c');
            char element = queue.Dequeue();
            Assert.IsTrue(element == 'a' && queue.Count == 2);
        }

        [Test]
        public void Foreach_QueueHasElements_ReturnElements()
        {
            var result = new List<int>();
            var queue = new Queue<char>();
            queue.Enqueue('a');
            queue.Enqueue('b');
            queue.Enqueue('c');
            foreach (var symbol in queue)
            {
                result.Add(symbol);
            }
            CollectionAssert.AreEqual(new List<char>() { 'a', 'b', 'c' },
                result);
        }

        [Test]
        public void IsEmpty_QueueIsEmtpy_ReturnTrue()
        {
            var queue = new Queue<int>();
            Assert.IsTrue(queue.IsEmpty);
        }

        [Test]
        public void Current_BeforeMoveNext_ThrowInvalidOperationException() 
        {
            var queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            var enumerator = queue.GetEnumerator();
            Assert.Throws<InvalidOperationException>(() => { int current = enumerator.Current; });
        }

        [Test]
        public void Current_AfterEnumerationEnded_ThrowInvalidOperationException()
        {
            var queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            var enumerator = queue.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            enumerator.MoveNext();
            Assert.Throws<InvalidOperationException>(() => { int current = enumerator.Current; });
        }

        [Test]
        public void EnumeratorMoveNext_EmptyQueue_ReturnFalse()
        {
            var queue = new Queue<string>();
            var enumerator = queue.GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void Contains_DoesContain_ReturnTrue()
        {
            var queueOfObjs = new Queue<object>();
            var firstObj = new object();
            queueOfObjs.Enqueue(new object());
            queueOfObjs.Enqueue(firstObj);
            Assert.IsTrue(queueOfObjs.Contains(firstObj));
        }

        [Test]
        public void Contains_DoesntContain_ReturnFalse()
        {
            var queue = new Queue<char>();
            queue.Enqueue('a');
            queue.Enqueue('b');
            queue.Enqueue('c');
            Assert.IsFalse(queue.Contains('x'));
        }

        [Test]
        public void Foreach_ChangeQueueWhileIterating_ThrowsInvalidOperationException()
        {
            var queue = new Queue<char>();
            queue.Enqueue('a');
            queue.Enqueue('b');
            queue.Enqueue('c');
            Assert.Throws<InvalidOperationException>(() => { foreach (var item in queue) { queue.Enqueue('c'); } });
        }

    }
}
