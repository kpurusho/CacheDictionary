C# Dictionary with capability to retain only n items in it, where n is the capacity passed as argument to constructor. 
Which n items to retain is decided based on CachePurgeStatergy - MRU or LRU.
Least Recently Used (LRU): discards the least recently used items first. 
Most Recently Used (MRU): discards, in contrast to LRU, the most recently used items first. 

Item that get affected by following operations are marked Most Recently used 

1. addition of new KVP (Key Value Pair)
2. Accessing a value
3. Updating a value
