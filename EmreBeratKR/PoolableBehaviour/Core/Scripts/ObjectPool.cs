namespace EmreBeratKR.ObjectPool
{
    public static class ObjectPool
    {
        private static readonly System.Collections.Generic.Dictionary<int, ObjectPoolStack<UnityEngine.GameObject>> Pools = new();
        private static readonly System.Collections.Generic.Dictionary<int, int> PrefabIDs = new();


        public static T Get<T>(T prefab)
            where T : UnityEngine.Component
        {
            return Get(prefab, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity);
        }
        
        public static T Get<T>(T prefab, UnityEngine.Transform parent)
            where T : UnityEngine.Component
        {
            return Get(prefab, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, parent);
        }
        
        public static T Get<T>(T prefab, UnityEngine.Vector3 position)
            where T : UnityEngine.Component
        {
            return Get(prefab, position, UnityEngine.Quaternion.identity);
        }

        public static T Get<T>(
            T prefab,
            UnityEngine.Vector3 position,
            UnityEngine.Quaternion rotation,
            UnityEngine.Transform parent = null)
            where T : UnityEngine.Component
        {
            var prefabID = GetInstanceID(prefab);

            if (TryGetObjectFromPoolWithPrefabID(prefabID, out T obj))
            {
                var objTransform = obj.transform;
                objTransform.position = position;
                objTransform.rotation = rotation;
                objTransform.parent = parent;
                
                if (obj is IPoolBehaviour poolObject)
                {
                    poolObject.OnGetFromPool();
                }
                
                return obj;
            }

            return InstantiateAndPutInPool(prefab, prefabID, position, rotation, parent);
        }

        public static UnityEngine.GameObject Get(UnityEngine.GameObject prefab)
        {
            return Get(prefab.transform, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity).gameObject;
        }
        
        public static UnityEngine.GameObject Get(UnityEngine.GameObject prefab, UnityEngine.Transform parent)
        {
            return Get(prefab.transform, parent).gameObject;
        }
        
        public static UnityEngine.GameObject Get(UnityEngine.GameObject prefab, UnityEngine.Vector3 position)
        {
            return Get(prefab.transform, position, UnityEngine.Quaternion.identity).gameObject;
        }
        
        public static UnityEngine.GameObject Get(
            UnityEngine.GameObject prefab,
            UnityEngine.Vector3 position,
            UnityEngine.Quaternion rotation,
            UnityEngine.Transform parent = null)
        {
            return Get(prefab.transform, position, rotation, parent).gameObject;
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
                Pools[prefabID] = new ObjectPoolStack<UnityEngine.GameObject>();
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
                var obj = InstantiateAndPutInPool(prefab, prefabID, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity);
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

        private static T InstantiateAndPutInPool<T>(
            T prefab, 
            int prefabID,
            UnityEngine.Vector3 position,
            UnityEngine.Quaternion rotation,
            UnityEngine.Transform parent = null)
            where T : UnityEngine.Component
        {
            var obj = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);

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