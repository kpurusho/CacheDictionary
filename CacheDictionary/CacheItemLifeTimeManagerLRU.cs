using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cache
{
    class CacheItemLifeTimeManagerLRU<TKey, TValue> : CacheItemLifeTimeManager<TKey, TValue>
    {
        public override TKey Purge()
        {
            var removedNode = _list.RemoveLast();
            return removedNode.Data.Key;
        }
    }
}
