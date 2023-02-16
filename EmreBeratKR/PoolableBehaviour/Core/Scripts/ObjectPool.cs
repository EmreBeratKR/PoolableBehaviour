namespace EmreBeratKR.ObjectPool
{
    public static class ObjectPool
    {
        private static readonly System.Collections.Generic.Dictionary<int, System.Collections.Generic.Stack<UnityEngine.GameObject>> Pools = new();
        private static readonly System.Collections.Generic.Dictionary<int, int> PrefabIDs = new();


        public static T Get<T>(T prefab)
            where T : UnityEngine.Component
        {
            var prefabID = prefab.GetInstanceID();

            if (TryGetObjectFromPoolWithPrefabID(prefabID, out T obj))
            {
                if (obj is IPoolBehaviour poolObject)
                {
                    poolObject.OnGetFromPool();
                }
                
                return obj;
            }
        
            obj = UnityEngine.Object.Instantiate(prefab);

            if (obj is IPoolBehaviour poolObj)
            {
                poolObj.OnInstantiated();
                poolObj.OnGetFromPool();
            }
            
            var instanceID = obj.GetInstanceID();
            PrefabIDs[instanceID] = prefabID;

            return obj;
        }

        public static UnityEngine.GameObject Get(UnityEngine.GameObject prefab)
        {
            return Get(prefab.transform).gameObject;
        }

        public static void Release<T>(T obj)
            where T : UnityEngine.Component
        {
            var instanceID = obj.GetInstanceID();

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
    }
}