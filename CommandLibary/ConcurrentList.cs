using System;
using System.Collections;
using System.Collections.Generic;

namespace Example.CommandLibrary
{
    /// <summary>
    /// The concurrent list class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentList<T> : ICollection<T>
    {
        private readonly List<T> itemList;
        private readonly object listMonitor;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <summary>
        /// Create an instance of the concurrent List
        /// </summary>
        public ConcurrentList()
        {
            listMonitor = new object();
            itemList = new List<T>();
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                lock (listMonitor)
                {
                    return itemList.Count;
                }
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            lock (listMonitor)
            {
                itemList.Clear();
            }
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            lock (listMonitor)
            {
                return itemList.Contains(item);
            }
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (listMonitor)
            {
                itemList.CopyTo(array, arrayIndex);
            }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            lock (listMonitor)
            {
                return itemList.Remove(item);
            }
        }

        /// <summary>
        /// Add an item to the concurrent list
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (listMonitor)
            {
                if (itemList.Contains(item))
                {
                    throw new ArgumentException($"It's not allowed to add the same reference twice");
                }
                itemList.Add(item);
            }
        }

        /// <summary>
        /// Try to get the first item and remove it from concurrent list
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public ExtendedValueResult<T> TryGetFirstItem()
        {
            lock (listMonitor)
            {
                var result = new ExtendedValueResult<T>(false);
                if (itemList.Count == 0)
                {
                    return result;
                }

                var item = itemList[0];
                var removed = itemList.Remove(item);
                result.Update(removed,item);
                return result;
            }
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }
    }
}
