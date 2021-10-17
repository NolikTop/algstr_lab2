using System;
using src.list;

namespace src.stack
{
    public class Stack<T>
    {
        private readonly LinkedList<T> _list = new();
        
        public bool IsEmpty => _list.IsEmpty;
        public int Count => _list.Count;

        public T Top => _list.First;

        public void Push(T item)
        {
            var el = new LinkedListElement<T>(item)
            {
                Next = _list.FirstElement
            };

            _list.FirstElement = el;
        }

        public T Pop()
        {
            if (_list.IsEmpty)
            {
                throw new IndexOutOfRangeException("Stack is empty");
            }
            
            var el = _list.FirstElement!;
            _list.FirstElement = el.Next;
            
            return el.Value;
        }

        public void Clear() => _list.Clear();

        public override string ToString()
        {
            return "<" + string.Join("<-", _list) + ">";
        }
    }
}