namespace Cache
{
    internal class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Previous;
        public Node<T> Next;

        public Node(T data)
        {
            Data = data;
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
