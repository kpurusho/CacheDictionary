using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CacheDictionary;

namespace Cache
{
    /// <summary>
    /// Dictionary with capability to retain n most recently used items in it. Where n is the capacity passed as argument to constructor.
    /// Item that get affected by following operations are marked Most Recently used 
    /// 1. addition of new KVP
    /// 2. Accessing a value
    /// 3. Updating a value
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region private members
        private readonly Dictionary<TKey, Node<CacheData<TKey, TValue>>> _dictionary = new Dictionary<TKey, Node<CacheData<TKey, TValue>>>();
        private readonly int _capacity = 32;
        private readonly DoublyLinkedList<CacheData<TKey, TValue>> _list = new DoublyLinkedList<CacheData<TKey, TValue>>();

        private void AddInternal(TKey key, TValue value)
        {
            if (_dictionary.Count >= _capacity)
            {
                _dictionary.Remove(_list.Tail.Data.Key);
                _list.RemoveLast();
            }

            var node = new Node<CacheData<TKey, TValue>>(new CacheData<TKey, TValue>(key, value));

            _dictionary.Add(key, node);
            _list.AddFront(node);

        }
        #endregion

        #region public members
        public CacheDictionary(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity","cannot be negative");
            _capacity = capacity;
        }

        public int CacheCapacity { get { return _capacity; } }
        #endregion

        #region IDictionary<TKey, TValue> implementation

        public void Add(TKey key, TValue value)
        {
            AddInternal(key, value);
        }

        public bool Remove(TKey key)
        {
            Node<CacheData<TKey, TValue>> node = null;
            if (_dictionary.TryGetValue(key, out node))
            {
                _list.Remove(node);
                return _dictionary.Remove(key);
            }
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            Node<CacheData<TKey, TValue>> node = null;
            if (_dictionary.TryGetValue(key, out node))
            {
                _list.MoveToFront(node);
                value = node.Data.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                var node = _dictionary[key];
                _list.MoveToFront(node);
                return node.Data.Value;
            }
            set
            {
                Node<CacheData<TKey, TValue>> node = null;
                if (_dictionary.TryGetValue(key, out node))
                {
                    node.Data.Value = value;
                    _list.MoveToFront(node);
                }
                else
                {
                    AddInternal(key, value);
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get { return _list.Select(data => data.Key).ToList(); }
        }

        public ICollection<TValue> Values
        {
            get { return _list.Select(data => data.Value).ToList(); }
        }

        #endregion

        #region IEnumerator<KeyValuePair<TKey, TValue>> implementation
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _list.Select(item => new KeyValuePair<TKey, TValue>(item.Key,item.Value)).GetEnumerator();
        }

        #endregion

        #region IEnumerable impementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>> impementation
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            AddInternal(item.Key,item.Value);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            Node<CacheData<TKey, TValue>> node = null;
            if (_dictionary.TryGetValue(item.Key, out node))
            {
                if (node.Data.Equals(new CacheData<TKey,TValue>(item.Key,item.Value)))
                {
                    _list.Remove(node);
                    return _dictionary.Remove(item.Key);
                }
            }
            return false;
        }

        public void Clear()
        {
            _dictionary.Clear();
            _list.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(new KeyValuePair<TKey, Node<CacheData<TKey, TValue>>>(item.Key, new Node<CacheData<TKey, TValue>>(new CacheData<TKey, TValue>(item.Key,item.Value))));
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion
    }
}
