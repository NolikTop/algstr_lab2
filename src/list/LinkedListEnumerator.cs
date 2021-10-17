using System.Collections;
using System.Collections.Generic;

namespace src.list
{
    public class LinkedListEnumerator<T> : IEnumerator<T>
    {
        private LinkedListElement<T>? _current;

        public T Current
        {
            get
            {
                var c = _current!;
                _current = _current?.Next;
                return c.Value;
            }
        }

        public LinkedListEnumerator(src.list.LinkedList<T> list)
        {
            if (list.FirstElement != null)
            {
                _current = list.FirstElement;
            }
        }

        public bool MoveNext()
        {
            return _current != null;
        }

        public void Reset() => _current = default;

        public void Dispose() => Reset();

        object IEnumerator.Current => Current!;
    }
}