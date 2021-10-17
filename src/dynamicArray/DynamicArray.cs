using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace src.dynamicArray
{
    public class DynamicArray<T> : ICollection<T>
    {
        private T[] _internalArray = new T[2];

        public int Count { get; private set; }

        private int _capacity = 2;
        public int Capacity
        {
            get => _capacity;
            private set
            {
                var newArray = new T[value];
                _capacity = value;

                _internalArray.CopyTo(newArray, 0);
                _internalArray = newArray;
            }
        }

        public bool IsEmpty => Count == 0;

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => _internalArray[index];
            set
            {
                if (index >= Count)
                {
                    Count = index + 1;
                }
                
                if (index >= Capacity)
                {
                    Capacity = Math.Max(index+1, Capacity * 2);
                }
                _internalArray[index] = value;
            }
        }

        public void Add(T item)
        {
            
            if (Count >= Capacity)
            {
                Capacity *= 2;
            }

            _internalArray[Count++] = item;
        }
        
        public void Clear()
        {
            _internalArray = new T[2];
            Count = 0;
            _capacity = 2;
        }

        public bool Contains(T item)
        {
            return _internalArray.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _internalArray.CopyTo(array, arrayIndex);
        }
        
        public int IndexOf(T item)
        {
            for (var i = 0; i < Count; ++i)
            {
                if (!EqualityComparer<T>.Default.Equals(_internalArray[i], item)) continue;

                return i;
            }

            return -1;
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);

            if (index == -1)
            {
                return false;
            }

            RemoveAt(index);
            
            return true;
        }

        public void RemoveAt(int index)
        {
            Array.Copy(_internalArray, index + 1, _internalArray, index, _internalArray.Length - index - 1); // сдвигаем массив 
            Count--;
        }
        
        public override string ToString()
        {
            var r = $"[Count={Count}, Capacity={Capacity}]<";
            if (Count == 0)
            {
                r += "Empty dynamic array";
            }
            else
            {
                r += string.Join(",", this);
            }

            r += ">";

            return r;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DynamicArrayEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}