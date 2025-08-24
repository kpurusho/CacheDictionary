using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CacheTest
{
    /// <summary>
    /// Pull up a Base Test class with all utils thrown in there
    /// </summary>
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

        #region Constructor Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NegativeCapacity_ArgumentOutOfRangeExceptionThrown()
        {
            var cache = new CacheDictionary<int, int>(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]  
        public void Constructor_NegativeCapacityWithStrategy_ArgumentOutOfRangeExceptionThrown()
        {
            var cache = new CacheDictionary<int, int>(-1, CachePurgeStatergy.LRU);
        }

        [TestMethod]
        public void Constructor_ZeroCapacity_CacheCreated()
        {
            var cache = new CacheDictionary<int, int>(0);
            Assert.AreEqual(0, cache.CacheCapacity);
            Assert.AreEqual(0, cache.Count);
        }

        [TestMethod]
        public void Constructor_DefaultStrategy_LRUStrategyUsed()
        {
            var cache = new CacheDictionary<int, int>(2);
            cache.Add(1, 1);
            cache.Add(2, 2);
            cache.Add(3, 3); // Should remove key 1 (LRU)
            
            Assert.IsFalse(cache.ContainsKey(1));
            Assert.IsTrue(cache.ContainsKey(2));
            Assert.IsTrue(cache.ContainsKey(3));
        }

        [TestMethod]
        public void Constructor_ExplicitLRUStrategy_LRUStrategyUsed()
        {
            var cache = new CacheDictionary<int, int>(2, CachePurgeStatergy.LRU);
            cache.Add(1, 1);
            cache.Add(2, 2);
            cache.Add(3, 3); // Should remove key 1 (LRU)
            
            Assert.IsFalse(cache.ContainsKey(1));
            Assert.IsTrue(cache.ContainsKey(2));
            Assert.IsTrue(cache.ContainsKey(3));
        }

        [TestMethod]
        public void Constructor_ExplicitMRUStrategy_MRUStrategyUsed()
        {
            var cache = new CacheDictionary<int, int>(2, CachePurgeStatergy.MRU);
            cache.Add(1, 1);
            cache.Add(2, 2);
            cache.Add(3, 3); // Should remove key 2 (MRU)
            
            Assert.IsTrue(cache.ContainsKey(1));
            Assert.IsFalse(cache.ContainsKey(2));
            Assert.IsTrue(cache.ContainsKey(3));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_InvalidStrategy_InvalidOperationExceptionThrown()
        {
            // Use an invalid enum value by casting
            var invalidStrategy = (CachePurgeStatergy)999;
            var cache = new CacheDictionary<int, int>(5, invalidStrategy);
        }

        #endregion

        #region Edge Case Tests

        [TestMethod]
        public void ZeroCapacityCache_AddItem_ItemNotAdded()
        {
            var cache = new CacheDictionary<int, int>(0);
            cache.Add(1, 1);
            Assert.AreEqual(0, cache.Count);
            Assert.IsFalse(cache.ContainsKey(1));
        }

        [TestMethod]
        public void SingleCapacityCache_AddTwoItems_FirstItemRemoved()
        {
            var cache = new CacheDictionary<int, int>(1);
            cache.Add(1, 1);
            Assert.AreEqual(1, cache.Count);
            Assert.IsTrue(cache.ContainsKey(1));
            
            cache.Add(2, 2);
            Assert.AreEqual(1, cache.Count);
            Assert.IsFalse(cache.ContainsKey(1));
            Assert.IsTrue(cache.ContainsKey(2));
        }

        [TestMethod]
        public void EmptyCache_RemoveNonExistentKey_ReturnsFalse()
        {
            Assert.IsFalse(_cache.Remove(999));
            Assert.AreEqual(0, _cache.Count);
        }

        [TestMethod]
        public void EmptyCache_ContainsKey_ReturnsFalse()
        {
            Assert.IsFalse(_cache.ContainsKey(999));
        }

        [TestMethod]
        public void EmptyCache_TryGetValue_ReturnsFalseAndDefaultValue()
        {
            int value;
            bool result = _cache.TryGetValue(999, out value);
            Assert.IsFalse(result);
            Assert.AreEqual(default(int), value);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void EmptyCache_IndexerGet_KeyNotFoundExceptionThrown()
        {
            var value = _cache[999];
        }

        [TestMethod]
        public void EmptyCache_IndexerSet_ItemAdded()
        {
            _cache[1] = 100;
            Assert.AreEqual(1, _cache.Count);
            Assert.AreEqual(100, _cache[1]);
        }

        [TestMethod]
        public void EmptyCache_Keys_EmptyCollection()
        {
            Assert.AreEqual(0, _cache.Keys.Count);
        }

        [TestMethod]
        public void EmptyCache_Values_EmptyCollection()
        {
            Assert.AreEqual(0, _cache.Values.Count);
        }

        [TestMethod]
        public void EmptyCache_GetEnumerator_NoItems()
        {
            int count = 0;
            foreach (var kvp in _cache)
            {
                count++;
            }
            Assert.AreEqual(0, count);
        }

        #endregion

        #region ICollection Interface Tests

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CopyTo_Always_NotImplementedExceptionThrown()
        {
            FillCache();
            var array = new KeyValuePair<int, int>[10];
            _cache.CopyTo(array, 0);
        }

        [TestMethod]
        public void Count_EmptyCache_ReturnsZero()
        {
            Assert.AreEqual(0, _cache.Count);
        }

        [TestMethod]
        public void Count_AfterAddingItems_ReturnsCorrectCount()
        {
            _cache.Add(1, 1);
            Assert.AreEqual(1, _cache.Count);
            
            _cache.Add(2, 2);
            Assert.AreEqual(2, _cache.Count);
        }

        [TestMethod]
        public void Count_AfterClear_ReturnsZero()
        {
            FillCache();
            Assert.AreEqual(5, _cache.Count);
            
            _cache.Clear();
            Assert.AreEqual(0, _cache.Count);
        }

        [TestMethod]
        public void Count_AfterRemove_ReturnsCorrectCount()
        {
            FillCache();
            Assert.AreEqual(5, _cache.Count);
            
            _cache.Remove(0);
            Assert.AreEqual(4, _cache.Count);
        }

        [TestMethod]
        public void IsReadOnly_Always_ReturnsFalse()
        {
            Assert.IsFalse(_cache.IsReadOnly);
        }

        #endregion

        #region Different Data Types Tests

        [TestMethod]
        public void StringKeys_AddAndRetrieve_WorksCorrectly()
        {
            var stringCache = new CacheDictionary<string, int>(3);
            stringCache.Add("one", 1);
            stringCache.Add("two", 2);
            stringCache.Add("three", 3);
            
            Assert.AreEqual(1, stringCache["one"]);
            Assert.AreEqual(2, stringCache["two"]);  
            Assert.AreEqual(3, stringCache["three"]);
            Assert.AreEqual(3, stringCache.Count);
        }

        [TestMethod]
        public void StringKeysLRU_CapacityExceeded_OldestKeyRemoved()
        {
            var stringCache = new CacheDictionary<string, int>(2);
            stringCache.Add("first", 1);
            stringCache.Add("second", 2);
            stringCache.Add("third", 3); // Should remove "first"
            
            Assert.IsFalse(stringCache.ContainsKey("first"));
            Assert.IsTrue(stringCache.ContainsKey("second"));
            Assert.IsTrue(stringCache.ContainsKey("third"));
        }

        [TestMethod]
        public void ObjectValues_AddAndRetrieve_WorksCorrectly()
        {
            var objectCache = new CacheDictionary<int, object>(3);
            objectCache.Add(1, "string value");
            objectCache.Add(2, 42);
            objectCache.Add(3, new object());
            
            Assert.AreEqual("string value", objectCache[1]);
            Assert.AreEqual(42, objectCache[2]);
            Assert.IsNotNull(objectCache[3]);
            Assert.AreEqual(3, objectCache.Count);
        }

        #endregion

        #region Null Value Tests

        [TestMethod]
        public void NullValues_AddAndRetrieve_WorksCorrectly()
        {
            var nullableCache = new CacheDictionary<int, string>(3);
            nullableCache.Add(1, null);
            nullableCache.Add(2, "not null");
            nullableCache.Add(3, null);
            
            Assert.IsNull(nullableCache[1]);
            Assert.AreEqual("not null", nullableCache[2]);
            Assert.IsNull(nullableCache[3]);
            Assert.AreEqual(3, nullableCache.Count);
        }

        [TestMethod]
        public void NullValues_TryGetValue_ReturnsCorrectly()
        {
            var nullableCache = new CacheDictionary<int, string>(2);
            nullableCache.Add(1, null);
            
            string value;
            bool result = nullableCache.TryGetValue(1, out value);
            Assert.IsTrue(result);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void NullValues_Contains_WorksCorrectly()
        {
            var nullableCache = new CacheDictionary<int, string>(2);
            nullableCache.Add(1, null);
            
            Assert.IsTrue(nullableCache.Contains(new KeyValuePair<int, string>(1, null)));
            Assert.IsFalse(nullableCache.Contains(new KeyValuePair<int, string>(1, "not null")));
        }

        [TestMethod]
        public void NullValues_IndexerSet_WorksCorrectly()
        {
            var nullableCache = new CacheDictionary<int, string>(2);
            nullableCache[1] = null;
            
            Assert.IsNull(nullableCache[1]);
            Assert.AreEqual(1, nullableCache.Count);
        }

        #endregion

        #region Advanced Purging Behavior Tests

        [TestMethod]
        public void LRUCache_AccessPattern_CorrectPurgingOrder()
        {
            var cache = new CacheDictionary<int, int>(3, CachePurgeStatergy.LRU);
            cache.Add(1, 1); // 1 (MRU)
            cache.Add(2, 2); // 2 (MRU), 1 (LRU)
            cache.Add(3, 3); // 3 (MRU), 2, 1 (LRU)
            
            // Access 1, making it MRU: 1 (MRU), 3, 2 (LRU) 
            var value = cache[1];
            
            // Add 4, should remove 2 (LRU)
            cache.Add(4, 4);
            
            Assert.IsTrue(cache.ContainsKey(1));
            Assert.IsFalse(cache.ContainsKey(2)); // Should be removed
            Assert.IsTrue(cache.ContainsKey(3));
            Assert.IsTrue(cache.ContainsKey(4));
        }

        [TestMethod]
        public void MRUCache_AccessPattern_CorrectPurgingOrder()
        {
            var cache = new CacheDictionary<int, int>(3, CachePurgeStatergy.MRU);
            cache.Add(1, 1); // 1 (MRU)
            cache.Add(2, 2); // 2 (MRU), 1 (LRU)
            cache.Add(3, 3); // 3 (MRU), 2, 1 (LRU)
            
            // Access 1, making it MRU: 1 (MRU), 3, 2 (LRU)
            var value = cache[1];
            
            // Add 4, should remove 1 (MRU)
            cache.Add(4, 4);
            
            Assert.IsFalse(cache.ContainsKey(1)); // Should be removed (was MRU)
            Assert.IsTrue(cache.ContainsKey(2));
            Assert.IsTrue(cache.ContainsKey(3));
            Assert.IsTrue(cache.ContainsKey(4));
        }

        [TestMethod]
        public void LRUCache_TryGetValueUpdatesPurgeOrder()
        {
            var cache = new CacheDictionary<int, int>(2, CachePurgeStatergy.LRU);
            cache.Add(1, 1); // 1 (MRU)
            cache.Add(2, 2); // 2 (MRU), 1 (LRU)
            
            int value;
            cache.TryGetValue(1, out value); // 1 (MRU), 2 (LRU)
            
            cache.Add(3, 3); // Should remove 2 (LRU)
            
            Assert.IsTrue(cache.ContainsKey(1));
            Assert.IsFalse(cache.ContainsKey(2)); // Should be removed
            Assert.IsTrue(cache.ContainsKey(3));
        }

        [TestMethod]
        public void MRUCache_TryGetValueUpdatesPurgeOrder()
        {
            var cache = new CacheDictionary<int, int>(2, CachePurgeStatergy.MRU);
            cache.Add(1, 1); // 1 (MRU)
            cache.Add(2, 2); // 2 (MRU), 1 (LRU)
            
            int value;
            cache.TryGetValue(1, out value); // 1 (MRU), 2 (LRU)
            
            cache.Add(3, 3); // Should remove 1 (MRU)
            
            Assert.IsFalse(cache.ContainsKey(1)); // Should be removed
            Assert.IsTrue(cache.ContainsKey(2));
            Assert.IsTrue(cache.ContainsKey(3));
        }

        #endregion

        #region Internal Consistency Tests

        [TestMethod]
        public void CacheCapacity_AfterConstruction_MatchesPassedValue()
        {
            var cache1 = new CacheDictionary<int, int>(10);
            Assert.AreEqual(10, cache1.CacheCapacity);
            
            var cache2 = new CacheDictionary<int, int>(1, CachePurgeStatergy.MRU);
            Assert.AreEqual(1, cache2.CacheCapacity);
        }

        [TestMethod]
        public void Count_NeverExceedsCapacity()
        {
            var cache = new CacheDictionary<int, int>(3);
            
            for (int i = 0; i < 10; i++)
            {
                cache.Add(i, i);
                Assert.IsTrue(cache.Count <= cache.CacheCapacity);
            }
            
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(3, cache.CacheCapacity);
        }

        [TestMethod]
        public void KeysAndValuesCount_MatchesCacheCount()
        {
            FillCache();
            Assert.AreEqual(_cache.Count, _cache.Keys.Count);
            Assert.AreEqual(_cache.Count, _cache.Values.Count);
            
            _cache.Remove(0);
            Assert.AreEqual(_cache.Count, _cache.Keys.Count);
            Assert.AreEqual(_cache.Count, _cache.Values.Count);
            
            _cache.Clear();
            Assert.AreEqual(_cache.Count, _cache.Keys.Count);
            Assert.AreEqual(_cache.Count, _cache.Values.Count);
        }

        [TestMethod]
        public void EnumeratorCount_MatchesCacheCount()
        {
            FillCache();
            int enumeratedCount = 0;
            foreach (var kvp in _cache)
            {
                enumeratedCount++;
            }
            Assert.AreEqual(_cache.Count, enumeratedCount);
            
            _cache.Remove(2);
            enumeratedCount = 0;
            foreach (var kvp in _cache)
            {
                enumeratedCount++;
            }
            Assert.AreEqual(_cache.Count, enumeratedCount);
        }

        #endregion
    }
}
