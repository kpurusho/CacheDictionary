using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cache
{
    abstract class CacheItemLifeTimeManager<TKey, TValue>
    {
        protected readonly DoublyLinkedList<CacheData<TKey, TValue>> _list = new DoublyLinkedList<CacheData<TKey, TValue>>();

        public abstract TKey Purge();

        public virtual void Add(Node<CacheData<TKey, TValue>> node)
        {
            _list.AddFront(node);
        }
        public virtual void Remove(Node<CacheData<TKey,TValue>> node)
        {
            _list.Remove(node);
        }
        public virtual void Mark(Node<CacheData<TKey, TValue>> node)
        {
            _list.MoveToFront(node);
        }

        public ICollection<TKey> Keys
        {
            get { return _list.Select(data => data.Key).ToList(); }
        } 

        public ICollection<TValue> Values
        {
            get { return _list.Select(data => data.Value).ToList(); }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _list.Select(item => new KeyValuePair<TKey, TValue>(item.Key, item.Value)).GetEnumerator();
        }

        public static CacheItemLifeTimeManager<TKey,TValue> Create(CachePurgeStatergy statergy)
        {
            switch (statergy)
            {
                    case CachePurgeStatergy.MRU:
                    return new CacheItemLifeTimeManagerMRU<TKey, TValue>();

                    case CachePurgeStatergy.LRU:
                    return new CacheItemLifeTimeManagerLRU<TKey, TValue>();
            }
            return null;
        }

        public void Clear()
        {
            _list.Clear();
        }
    }
}
