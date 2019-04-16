using System;
using System.Collections;
using System.Collections.Generic;

namespace Queue
{
    /// <summary>
    /// Class that represents queue.
    /// </summary>
    /// <typeparam name="T">Given type.</typeparam>
    public class Queue<T> : IEnumerable<T>
    {
        #region Constants
        
        /// <summary>
        /// Default capacity.
        /// </summary>
        private const int DefaultCapacity = 4;

        #endregion

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}" /> class.
        /// </summary>
        public Queue()
        {
            this.Capacity = DefaultCapacity;
            this.Elements = new T[this.Capacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}" /> class.
        /// </summary>
        /// <param name="items">Given items.</param>
        public Queue(IEnumerable<T> items)
        {
            this.Capacity = DefaultCapacity;
            this.Elements = new T[this.Capacity];
            this.CheckCollectionOnNull(items);
            foreach (var item in items)
            {
                this.Enqueue(item);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}" /> class.
        /// </summary>
        /// <param name="capacity">Given capacity.</param>
        public Queue(int capacity)
        {
            this.Capacity = capacity;
            Elements = new T[Capacity];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets front index.
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
        /// Gets a value indicating whether queue is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }

        /// <summary>
        /// Gets count of elements in queue.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets or sets capacity.
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
        /// Stack version.
        /// </summary>
        private int Version { get; set; }

        /// <summary>
        /// Elements in queue.
        /// </summary>
        private T[] Elements { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// Queue capacity.
        /// </summary>
        private int capacity;

        #endregion

        #region Public methods

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
            this.Version++;
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
            this.Version++;
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
        /// <returns>Enumerator.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns if queue contains given item.
        /// </summary>
        /// <param name="item">Given item.</param>
        /// <returns>If element is in queue.</returns>
        public bool Contains(T item)
        {
            var equalityComparer = EqualityComparer<T>.Default;
            foreach (var element in this)
            {
                if (equalityComparer.Equals(item, element))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Private methods

        private void IncreaseCapacity()
        {
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

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Enumerator

        public struct Enumerator : IEnumerator<T>
        {
            /// <summary>
            /// Before start state of iteration.
            /// </summary>
            private const int BeforeStartState = -2;

            /// <summary>
            /// After iteration was ended state.
            /// </summary>
            private const int EndState = -1;

            /// <summary>
            /// Queue instance.
            /// </summary>
            private readonly Queue<T> queue;

            /// <summary>
            /// Version when iteration started.
            /// </summary>
            private readonly int version;

            /// <summary>
            /// Current state.
            /// </summary>
            private int state;

            /// <summary>
            /// Current element.
            /// </summary>
            private T currentElement;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="queue">Queue instance.</param>
            public Enumerator(Queue<T> queue)
            {
                this.queue = queue;
                version = queue.Version;
                this.state = -2;
                currentElement = default(T);
            }

            /// <summary>
            /// Returns current element.
            /// </summary>
            public T Current
            {
                get
                {
                    if (this.state == BeforeStartState)
                    {
                        throw new InvalidOperationException("Iteration was not started!");
                    }

                    if (this.state == EndState)
                    {
                        throw new InvalidOperationException("Iteration was ended");
                    }

                    return this.currentElement;
                }
            }

            /// <summary>
            /// Returns current element.
            /// </summary>
            object IEnumerator.Current => this.Current;

            /// <summary>
            /// Moves next.
            /// </summary>
            /// <returns>Returns false if there is no current. Otherwise - true.</returns>
            public bool MoveNext()
            { 
                bool result;
                CheckForQueueChange();
                if (this.state == BeforeStartState)
                {
                    this.state = 0;
                    result = queue.Count > 0;
                    if (result)
                    {
                        currentElement = queue.Elements[this.state];
                    }

                    return result;
                }

                result = ++state < this.queue.Count;
                if (result)
                {
                    currentElement = queue.Elements[this.state];
                }
                else
                {
                    currentElement = default(T);
                    this.state = -2;
                }

                return result;
            }

            public void Dispose()
            {
                this.state = -2;
                currentElement = default(T);
            }

            public void Reset()
            {
                this.CheckForQueueChange();
                this.state = BeforeStartState;
                this.currentElement = default(T);     
                throw new NotImplementedException();
            }
  
            private void CheckForQueueChange()
            {
                if (this.version != this.queue.Version)
                {
                    throw new InvalidOperationException("Queue was changed");
                }
            }
        }

        #endregion  Iterator
    }
}
