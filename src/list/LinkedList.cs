using System;
using System.Collections;
using System.Collections.Generic;

namespace src.list
{
    public class LinkedList<T> : IList<T>
    {
        internal LinkedListElement<T>? FirstElement = null;

        public T First => NotNullOrIndexOutOfRange(FirstElement).Value;
        public T Last => NotNullOrIndexOutOfRange(LastElement()).Value;
        
        public int Count
        {
            get
            {
                var count = 0;
                for (var it = FirstElement; it != null; it = it.Next, count++) { }

                return count;
            }
        }
        
        public bool IsEmpty => FirstElement == null;

        public bool IsReadOnly => false;
        
        public T this[int index]
        {
            get => ElementAt(index).Value;
            set => ElementAt(index).Value = value;
        }

        public void Add(T item)
        {
            var last = LastElement();
            if (last == null)
            {
                FirstElement = new LinkedListElement<T>(item);
            }
            else
            {
                last.Next = new LinkedListElement<T>(item);
            }
        }

        public void Clear()
        {
            FirstElement = null;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var it = ElementAt(arrayIndex);

            for (var i = 0; it != null && i < array.Length; it = it.Next)
            {
                array[i] = it.Value;
            }
        }

        public bool Remove(T item)
        {
            LinkedListElement<T>? prev = null;
            for (var it = FirstElement; it != null; prev = it, it = it.Next)
            {
                if (!EqualityComparer<T>.Default.Equals(it.Value, item)) continue;
                
                if (prev == null)
                {
                    FirstElement = it.Next;
                    return true;
                }
                
                prev.Next = it.Next;
                return true;
            }

            return false;
        }

        public int IndexOf(T item)
        {
            var i = 0;
            for (var it = FirstElement; it != null; it = it.Next, i++)
            {
                if (EqualityComparer<T>.Default.Equals(it.Value, item)) return i;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            this[index] = item;
        }

        public void RemoveAt(int index)
        {
            switch (index)
            {
                case 0:
                    FirstElement = FirstElement?.Next;
                    return; 
                case > 0:
                    var i = 0;
                    LinkedListElement<T>? prev = null;
                    
                    for (var it = FirstElement; it != null; prev = it, it = it.Next, i++)
                    {
                        if (i != index) continue;
                        
                        prev!.Next = it.Next;
                        return;
                    }
                    break;
            }
            
            throw new IndexOutOfRangeException();
        }

        public override string ToString()
        {
            if (FirstElement == null)
            {
                return "<Empty list>";
            }
            return "<" + string.Join("->", this) + ">";
        }

        private LinkedListElement<T> ElementAt(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }
            
            var i = 0;

            for (var it = FirstElement; it != null; it = it.Next, i++)
            {
                if (i != index) continue;

                return it;
            }

            throw new IndexOutOfRangeException();
        }
        
        private LinkedListElement<T>? LastElement()
        {
            var it = FirstElement;
            if (it == null) return null;

            for (; it.Next != null; it = it.Next) {}

            return it;
        }

        private static LinkedListElement<T> NotNullOrIndexOutOfRange(LinkedListElement<T>? element)
        {
            if (element is null)
            {
                throw new IndexOutOfRangeException();
            }

            return element;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}