namespace EmreBeratKR.ObjectPool
{
    public class ObjectPoolStack<T> : System.Collections.Generic.IEnumerable<T>
    {
        private readonly System.Collections.Generic.Stack<T> m_Stack = new();


        public void Push(T obj)
        {
            m_Stack.Push(obj);
        }

        public bool TryPop(out T obj)
        {
            return m_Stack.TryPop(out obj);
        }

        public void Clear()
        {
            m_Stack.Clear();
        }

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return m_Stack.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}