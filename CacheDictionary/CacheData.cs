using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CacheDictionary
{
    internal class CacheData<TKey, TValue>
    {
        private readonly TKey _key;
        private TValue _value;
        public CacheData(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public TKey Key
        {
            get { return _key; }
        }

        public TValue Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as CacheData<TKey, TValue>;

            if (rhs != null)
            {
                return Key.Equals(rhs.Key) && Value.Equals(rhs.Value);
            }

            return false;
        }

    }
}
