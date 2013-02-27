namespace Cache
{
    internal class Node<T>
    {
        private T _data;
        public T Data
        {
            get { return _data; } 
            set { _data = value; }
        }
        public Node<T> Previous;
        public Node<T> Next;

        public Node(T data)
        {
            _data = data;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node<T>)
            {
                var n = obj as Node<T>;
                return n.Data.Equals(Data);
            }
            return false;
        }
    }
}
