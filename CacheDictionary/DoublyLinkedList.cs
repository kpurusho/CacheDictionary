using System;
using System.Collections;
using System.Collections.Generic;

namespace Cache
{
    internal class DoublyLinkedList<T> : IEnumerable<T>
    {
        private Node<T> _head;
        private Node<T> _tail;
        private int _count = 0;

        public Node<T> Head { get { return _head; } }

        public Node<T> Tail { get { return _tail; } }


        public IEnumerator<T> GetEnumerator()
        {
            var temp = _head;
            while (temp != null)
            {
                yield return temp.Data;
                temp = temp.Next;
            }
        }

        public Node<T> AddAndGetNode(T data)
        {
            var node = new Node<T>(data);
            AddFront(node);
            return _head;
        }

        public void AddFront(Node<T> node)
        {
            if (_head == null)
            {
                _head = node;
                _tail = _head;
            }
            else
            {
                node.Next = _head;
                _head.Previous = node;
                _head = node;
                _head.Previous = null;
            }
            _count++;
        }

        public void Clear()
        {
            while (_tail != null)
            {
                _tail = _tail.Previous;

                if (_tail != null)
                    _tail.Next = null;
            }
            _head = null;
            _count = 0;
        }

        public void MoveToFront(Node<T> node)
        {
            Remove(node);

            AddFront(node);
        }

        public void Remove(Node<T> node)
        {
            if (_count == 0) return;

            var currNode = node;
            var prevnode = node.Previous;
            var nextnode = node.Next;

            if (prevnode != null)
            {
                prevnode.Next = nextnode;
            }
            else
            {
                _head = nextnode;
            }

            if (nextnode != null)
            {
                nextnode.Previous = prevnode;
            }
            else
            {
                _tail = prevnode;
            }

            currNode.Previous = null;
            currNode.Next = null;
            _count--;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Node<T> RemoveFirst()
        {
            var retVal = _head;

            if (_head == null) throw new Exception("List is empty");

            _head = _head.Next;
            if (_head != null)
            {
                _head.Previous = null;
            }
            else
            {
                _tail = null;
            }

            _count--;

            return retVal;
        }


        public Node<T> RemoveLast()
        {
            var retVal = _tail;

            if (_tail == null) throw new Exception("List is empty");

            _tail = _tail.Previous;
            if (_tail != null)
            {
                _tail.Next = null;
            }
            else
            {
                _head = null;
            }

            _count--;

            return retVal;
        }
    }
}
