using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CacheTest
{
    [TestClass]
    public class CacheUnitTest
    {
        private CacheDictionary<int,int> _cache = new CacheDictionary<int,int>(5);

        private void FillCache()
        {
            for (var i = 0; i < _cache.CacheCapacity; i++)
            {
                _cache.Add(i, i);
            }
        }

        [TestInitialize]
        public void Intialize()
        {
            
        }

        [TestMethod]
        public void Add_KeyNotPresent_KVPAdded()
        {
            _cache.Add(0,0);
            Assert.IsTrue(_cache.ContainsKey(0));
            Assert.AreEqual(0,_cache[0]);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_KeyAlreadyPresent_ArgumentExceptionThrown()
        {
            _cache.Add(0, 0);
            _cache.Add(0, 2);
        }

        [TestMethod]
        public void Add_CacheCapacityReached_LRUKVPRemovedAndNewKVPAdded()
        {
            FillCache();
            _cache.Add(5,5);
            Assert.IsFalse(_cache.ContainsKey(0));
            Assert.IsTrue(_cache.ContainsKey(5));
            Assert.AreEqual(_cache.CacheCapacity, _cache.Count);
        }

        [TestMethod]
        public void Add_CacheCapacityReached_MRUKVPRemovedAndNewKVPAdded()
        {
            _cache = new CacheDictionary<int, int>(5,CachePurgeStatergy.MRU);
            FillCache();
            _cache.Add(5, 5);
            Assert.IsFalse(_cache.ContainsKey(4));
            Assert.IsTrue(_cache.ContainsKey(5));
            Assert.AreEqual(_cache.CacheCapacity, _cache.Count);
        }

        [TestMethod]
        public void AddKVP_KeyNotPresent_KVPAdded()
        {
            _cache.Add(new KeyValuePair<int,int>(0,0));
            Assert.IsTrue(_cache.ContainsKey(0));
            Assert.AreEqual(0, _cache[0]);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddKVP_KeyAlreadyPresent_ArgumentExceptionThrown()
        {
            _cache.Add(new KeyValuePair<int, int>(0, 0));
            _cache.Add(new KeyValuePair<int, int>(0, 2));
        }

        [TestMethod]
        public void AddKVP_CacheCapacityReached_LRUKVPRemovedAndNewKVPAdded()
        {
            FillCache();
            _cache.Add(new KeyValuePair<int,int>(5,5));
            Assert.IsFalse(_cache.ContainsKey(0));
            Assert.IsTrue(_cache.ContainsKey(5));
            Assert.AreEqual(_cache.CacheCapacity, _cache.Count);
        }

        [TestMethod]
        public void AddKVP_CacheCapacityReached_MRUKVPRemovedAndNewKVPAdded()
        {
            _cache = new CacheDictionary<int, int>(5, CachePurgeStatergy.MRU);
            FillCache();
            _cache.Add(new KeyValuePair<int, int>(5, 5));
            Assert.IsFalse(_cache.ContainsKey(4));
            Assert.IsTrue(_cache.ContainsKey(5));
            Assert.AreEqual(_cache.CacheCapacity, _cache.Count);
        }


        [TestMethod]
        public void Remove_KeyPresent_KeyRemovedAndRemoveReturnsTrue()
        {
            _cache.Add(0,0);
            Assert.IsTrue(_cache.Remove(0));
            Assert.IsFalse(_cache.ContainsKey(0));
        }

        [TestMethod]
        public void Remove_KeyNotPresent_RemoveReturnsFalse()
        {
            _cache.Add(0, 0);
            Assert.IsFalse(_cache.Remove(-1));
            Assert.AreEqual(1,_cache.Count);
        }

        [TestMethod]
        public void RemoveKVP_KeyPresent_KeyRemovedAndRemoveReturnsTrue()
        {
            _cache.Add(0, 0);
            Assert.IsTrue(_cache.Remove(new KeyValuePair<int,int>(0,0)));
            Assert.IsFalse(_cache.ContainsKey(0));
        }

        [TestMethod]
        public void RemoveKVP_KeyPresentValueNotPresent_RemoveReturnsFalse()
        {
            _cache.Add(0, 0);
            Assert.IsFalse(_cache.Remove(new KeyValuePair<int, int>(0, 1)));
            Assert.AreEqual(1, _cache.Count);
        }

        [TestMethod]
        public void RemoveKVP_KVPPresent_RemoveReturnsTrue()
        {
            _cache.Add(0, 0);
            Assert.IsTrue(_cache.Remove(new KeyValuePair<int, int>(0, 0)));
            Assert.IsFalse(_cache.ContainsKey(0));
        }

        [TestMethod]
        public void Clear_CacheNotEmtpy_CacheEmptied()
        {
            _cache.Add(0,0);
            _cache.Add(1,1);
            _cache.Clear();
            Assert.AreEqual(0,_cache.Count);
        }

        [TestMethod]
        public void Contains_KVPPresent_ReturnsTrue()
        {
            _cache.Add(0, 0);
            _cache.Add(1, 1);
            Assert.IsTrue(_cache.Contains(new KeyValuePair<int,int>(0,0)));
        }

        [TestMethod]
        public void Contains_KeyPresentValueNotPresent_ReturnsFalse()
        {
            _cache.Add(0, 0);
            _cache.Add(1, 1);
            Assert.IsFalse(_cache.Contains(new KeyValuePair<int, int>(0, 1)));
        }

        [TestMethod]
        public void ContainsKey_KeyPresent_ReturnsTrue()
        {
            _cache.Add(0, 0);
            _cache.Add(1, 1);
            Assert.IsTrue(_cache.ContainsKey(0));
        }

        [TestMethod]
        public void ContainsKey_KeyNotPresent_ReturnsFalse()
        {
            _cache.Add(0, 0);
            _cache.Add(1, 1);
            Assert.IsFalse(_cache.ContainsKey(3));
        }

        [TestMethod]
        public void TryGetValue_KeyNotPresent_ReturnsFalse()
        {
            FillCache();
            int value;
            Assert.IsFalse(_cache.TryGetValue(5,out value));
        }

        [TestMethod]
        public void TryGetValue_KeyPresent_ReturnsTrueAndValue()
        {
            FillCache();
            int value;
            Assert.IsTrue(_cache.TryGetValue(4, out value));
            Assert.AreEqual(4, value);
        }

        [TestMethod]
        public void TryGetValue_LRUDicAndKeyPresent_KeyMarkedLastForPurge()
        {
            FillCache();
            int value;
            Assert.IsTrue(_cache.TryGetValue(0, out value));    //0 moved to top of MRU list
            Assert.AreEqual(0, value);

            _cache.Add(5,5);
            Assert.IsTrue(_cache.ContainsKey(5));   //newly added key 5 is present
            Assert.IsTrue(_cache.ContainsKey(0));   //0 is present
            Assert.IsFalse(_cache.ContainsKey(1));  //next LRU key is removed when 5 is added
        }

        [TestMethod]
        public void TryGetValue_MRUDicAndKeyPresent_KeyMarkedLastForPurge()
        {
            _cache = new CacheDictionary<int, int>(5, CachePurgeStatergy.MRU);
            FillCache();
            int value;
            Assert.IsTrue(_cache.TryGetValue(0, out value));    //0 moved to top of MRU list
            Assert.AreEqual(0, value);

            _cache.Add(5, 5);
            Assert.IsTrue(_cache.ContainsKey(5));   //newly added key 5 is present
            Assert.IsFalse(_cache.ContainsKey(0));   //0 is not present
        }

        [TestMethod]
        public void IndexerGet_KeyPresent_ValueReturned()
        {
            FillCache();
            Assert.AreEqual(2,_cache[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void IndexerGet_KeyNotPresent_KeyNotFoundExeceptionThrown()
        {
            FillCache();
            Assert.AreEqual(5, _cache[5]);
        }

        [TestMethod]
        public void IndexerSet_LRUDicAndKeyPresent_ValueUpdatedAndMarkedLastForPurge()
        {
            FillCache();
            _cache[0] = 100;
            Assert.AreEqual(100,_cache[0]);

            _cache.Add(5, 5);
            Assert.IsTrue(_cache.ContainsKey(5));   //newly added key 5 is present
            Assert.IsTrue(_cache.ContainsKey(0));   //0 is present
            Assert.IsFalse(_cache.ContainsKey(1));  //next LRU key is removed when 5 is added
        }

        [TestMethod]
        public void IndexerSet_MRUDicAndKeyPresent_ValueUpdatedAndMarkedForPurge()
        {
            _cache = new CacheDictionary<int, int>(5, CachePurgeStatergy.MRU);
            FillCache();
            _cache[0] = 100;
            Assert.AreEqual(100, _cache[0]);

            _cache.Add(5, 5);
            Assert.IsTrue(_cache.ContainsKey(5));   //newly added key 5 is present
            Assert.IsFalse(_cache.ContainsKey(0));   //0 is not present
        }


        [TestMethod]
        public void IndexerSet_LRUDicAndKeyNotPresent_KVPAddedAndMarkedLastForPurge()
        {
            FillCache();
            _cache[5] = 5;
            Assert.AreEqual(5, _cache[5]);

            Assert.IsTrue(_cache.ContainsKey(5));   //newly added key 5 is present
            Assert.IsFalse(_cache.ContainsKey(0));   //0 is not present
        }

        [TestMethod]
        public void IndexerSet_MRUDicAndKeyNotPresent_KVPAddedAndMarkedForPurge()
        {
            _cache = new CacheDictionary<int, int>(5,CachePurgeStatergy.MRU);
            FillCache();
            _cache[5] = 5;
            Assert.AreEqual(5, _cache[5]);

            Assert.IsTrue(_cache.ContainsKey(5));   //newly added key 5 is present
            Assert.IsFalse(_cache.ContainsKey(4));   //0 is not present
            _cache.Add(6,6);
            Assert.IsFalse(_cache.ContainsKey(5));   //key 5 is not present
        }

        [TestMethod]
        public void Keys_KeysPresent_ReturnListOfKeys()
        {
            FillCache();
            int key = 4;
            foreach(var k in _cache.Keys)
            {
                Assert.AreEqual(key--,k);
            }
        }

        [TestMethod]
        public void Keys_ValuesPresent_ReturnListOfValues()
        {
            FillCache();
            int value = 4;
            foreach (var v in _cache.Values)
            {
                Assert.AreEqual(value--, v);
            }
        }

        [TestMethod]
        public void GetEnumerator_KVPPresent_ReturnsListOfKVP()
        {
            FillCache();
            int kvp = 4;
            foreach (var kv in _cache)
            {
                Assert.AreEqual(kvp,kv.Key);
                Assert.AreEqual(kvp--,kv.Value);
            }
        }
    }
}
