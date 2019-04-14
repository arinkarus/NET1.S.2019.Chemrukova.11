using System;
using System.Collections;
using System.Collections.Generic;

namespace Queue
{
    public class Queue<T> : IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}" /> class.
        /// </summary>
        public Queue()
        {
            this.Elements = new T[this.Capacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}" /> class.
        /// </summary>
        public Queue(IEnumerable<T> collection)
        {
            this.CheckCollectionOnNull(collection);
            var list = new List<T>(collection);
            Elements = list.ToArray();
            Count = Elements.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}" /> class.
        /// </summary>
        public Queue(int capacity)
        {
            this.Capacity = capacity;
            Elements = new T[Capacity];
        }

        /// <summary>
        /// Queue capacity.
        /// </summary>
        private int capacity;

        /// <summary>
        /// Gets and sets capacity.
        /// </summary>
        private int Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                CheckCapacity(value);
                capacity = value;
            }
        }

        /// <summary>
        /// Count of elements in queue.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Elements in queue.
        /// </summary>
        private T[] Elements { get; set; }

        /// <summary>
        /// Front index.
        /// </summary>
        public int FrontIndex { get; private set; }

        /// <summary>
        /// Gets Back index.
        /// </summary>
        public int BackIndex
        {
            get { return (FrontIndex + Count) % Capacity; }
        }

        /// <summary>
        /// Gets value that determines if queue is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }

        /// <summary>
        /// Add item to queue.
        /// </summary>
        /// <param name="element">Element i add.</param>
        public void Enqueue(T element)
        {
            if (this.Count == Capacity)
            {
                this.IncreaseCapacity();
            }

            this.Elements[BackIndex] = element;
            this.Count++;
        }

        /// <summary>
        /// Returns item from queue and removes it from queue.
        /// </summary>
        /// <returns>Item from queue.</returns>
        public T Dequeue()
        {
            this.CheckForEmptiness();
            T element = this.Elements[FrontIndex];
            this.Elements[FrontIndex] = default(T);
            this.Count--;
            this.FrontIndex = (FrontIndex + 1) % Capacity;
            return element;
        }

        /// <summary>
        /// Returns item from queue.
        /// </summary>
        /// <returns>Item from queue.</returns>
        public T Peek()
        {
            this.CheckForEmptiness();
            return this.Elements[FrontIndex];
        }

        /// <summary>
        /// Gets enumerator.
        /// </summary>
        /// <returns>Returns enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return Elements[i];
            }
        }

        private void IncreaseCapacity()
        {
            this.Capacity++;
            this.Capacity *= 2;
            Queue<T> tempQueue = new Queue<T>(this.Capacity);
            while (this.Count > 0)
            {
                tempQueue.Enqueue(this.Dequeue());
            }

            this.Elements = tempQueue.Elements;
            this.Count = tempQueue.Count;
            this.FrontIndex = tempQueue.FrontIndex;
        }

        private void CheckCapacity(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException($"{nameof(capacity)} cant't be a negative number!");
            }
        }

        private void CheckForEmptiness()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty!");
            }
        }

        private void CheckCollectionOnNull(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException($"{nameof(collection)} is null, but collection is required!");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
