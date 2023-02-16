namespace EmreBeratKR.ObjectPool
{
    public static class ObjectPool
    {
        private static readonly System.Collections.Generic.Dictionary<int, System.Collections.Generic.Stack<UnityEngine.GameObject>> Pools = new();
        private static readonly System.Collections.Generic.Dictionary<int, int> PrefabIDs = new();


        public static T Get<T>(T prefab)
            where T : UnityEngine.Component
        {
            var prefabID = GetInstanceID(prefab);

            if (TryGetObjectFromPoolWithPrefabID(prefabID, out T obj))
            {
                if (obj is IPoolBehaviour poolObject)
                {
                    poolObject.OnGetFromPool();
                }
                
                return obj;
            }

            return InstantiateAndPutInPool(prefab, prefabID);
        }

        public static UnityEngine.GameObject Get(UnityEngine.GameObject prefab)
        {
            return Get(prefab.transform).gameObject;
        }

        public static void Release<T>(T obj)
            where T : UnityEngine.Component
        {
            var instanceID = GetInstanceID(obj);

            if (!PrefabIDs.ContainsKey(instanceID))
            {
                throw ObjectPoolExceptions.ObjectDoesNotExistInPool(obj);
            }
        
            var prefabID = PrefabIDs[instanceID];

            if (!Pools.ContainsKey(prefabID))
            {
                Pools[prefabID] = new System.Collections.Generic.Stack<UnityEngine.GameObject>();
            }

            var gameObject = obj.gameObject;
            Pools[prefabID].Push(gameObject);
            gameObject.SetActive(false);

            if (obj is IPoolBehaviour poolObject)
            {
                poolObject.OnReleasedToPool();
            }
        }

        public static void Release(UnityEngine.GameObject gameObject)
        {
            Release(gameObject.transform);
        }

        public static void Fill<T>(T prefab, int count)
            where T : UnityEngine.Component
        {
            var prefabID = GetInstanceID(prefab);
            
            for (var i = 0; i < count; i++)
            {
                var obj = InstantiateAndPutInPool(prefab, prefabID);
                Release(obj);
            }
        }

        public static void Fill(UnityEngine.GameObject prefab, int count)
        {
            Fill(prefab.transform, count);
        }

        public static void Clear<T>(T prefab)
            where T : UnityEngine.Component
        {
            var prefabID = GetInstanceID(prefab);

            if (!Pools.ContainsKey(prefabID)) return;

            var pool = Pools[prefabID];

            foreach (var gameObject in pool)
            {
                PrefabIDs.Remove(gameObject.GetInstanceID());
                UnityEngine.Object.Destroy(gameObject);
            }
            
            pool.Clear();
        }

        public static void Clear(UnityEngine.GameObject prefab)
        {
            Clear(prefab.transform);
        }
        

        private static bool TryGetObjectFromPoolWithPrefabID<T>(int prefabID, out T obj)
            where T : UnityEngine.Component
        {
            if (!Pools.ContainsKey(prefabID))
            {
                obj = null;
                return false;
            }

            if (!Pools[prefabID].TryPop(out var gameObject))
            {
                obj = null;
                return false;
            }

            obj = gameObject.GetComponent<T>();
            gameObject.SetActive(true);
            return true;
        }

        private static T InstantiateAndPutInPool<T>(T prefab, int prefabID)
            where T : UnityEngine.Component
        {
            var obj = UnityEngine.Object.Instantiate(prefab);

            if (obj is IPoolBehaviour poolObj)
            {
                poolObj.OnInstantiated();
                poolObj.OnGetFromPool();
            }
            
            var instanceID = GetInstanceID(obj);
            PrefabIDs[instanceID] = prefabID;

            return obj;
        }

        private static int GetInstanceID<T>(T prefab)
            where T : UnityEngine.Component
        {
            return GetInstanceID(prefab.gameObject);
        }

        private static int GetInstanceID(UnityEngine.GameObject gameObject)
        {
            return gameObject.GetInstanceID();
        }
    }
}