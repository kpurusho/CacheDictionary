using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cache
{
    class CacheItemLifeTimeManagerMRU<TKey,TValue> : CacheItemLifeTimeManager<TKey,TValue>
    {
        public override TKey Purge()
        {
            var removedNode = _list.RemoveFirst();
            return removedNode.Data.Key;
        }
    }
}
