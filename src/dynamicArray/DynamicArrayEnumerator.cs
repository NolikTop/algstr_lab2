using System.Collections;
using System.Collections.Generic;

namespace src.dynamicArray
{
    public class DynamicArrayEnumerator<T> : IEnumerator<T>
    {
        private DynamicArray<T>? _array;
        private int _currentIndex;
        public T Current => _array![_currentIndex-1];

        public DynamicArrayEnumerator(DynamicArray<T> array)
        {
            _array = array;
            _currentIndex = 0;
        }

        public bool MoveNext()
        {
            return _currentIndex++ < _array!.Count;
        }

        public void Reset()
        {
            _array = default;
            _currentIndex = default;
        }

        public void Dispose() => Reset();
        
        object IEnumerator.Current => Current!;
    }
}