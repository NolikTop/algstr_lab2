namespace src.list
{
    public class LinkedListElement<T>
    {
        internal LinkedListElement<T>? Next;
        internal T Value;

        internal LinkedListElement(T value)
        {
            Value = value;
        }

    }
}