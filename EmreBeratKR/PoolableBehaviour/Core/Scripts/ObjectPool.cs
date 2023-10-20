namespace EmreBeratKR.ObjectPool
{
    public static class ObjectPool
    {
        private static readonly System.Collections.Generic.Dictionary<int, ObjectPoolStack<UnityEngine.Object>> Pools = new();
        private static readonly System.Collections.Generic.Dictionary<int, int> PrefabIDs = new();


        public static T Get<T>(T prefab)
            where T : UnityEngine.Object
        {
            return Get(prefab, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity);
        }
        
        public static T Get<T>(T prefab, UnityEngine.Transform parent)
            where T : UnityEngine.Object
        {
            return Get(prefab, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, parent);
        }
        
        public static T Get<T>(T prefab, UnityEngine.Vector3 position)
            where T : UnityEngine.Object
        {
            return Get(prefab, position, UnityEngine.Quaternion.identity);
        }

        public static T Get<T>(
            T prefab,
            UnityEngine.Vector3 position,
            UnityEngine.Quaternion rotation,
            UnityEngine.Transform parent = null)
            where T : UnityEngine.Object
        {
            var prefabID = GetInstanceID(prefab);

            if (TryGetObjectFromPoolWithPrefabID(prefabID, out T obj))
            {
                var objTransform = GetTransform(obj);
                objTransform.position = position;
                objTransform.rotation = rotation;
                objTransform.parent = parent;
                
                if (TryGetComponent(obj, out IOnGetFromPool call))
                {
                    call.OnGetFromPool();
                }
                
                return obj;
            }

            return InstantiateAndPutInPool(prefab, prefabID, position, rotation, parent);
        }

        public static void Release<T>(T obj)
            where T : UnityEngine.Object
        {
            var instanceID = GetInstanceID(obj);

            if (!PrefabIDs.ContainsKey(instanceID))
            {
                throw ObjectPoolExceptions.ObjectDoesNotExistInPool(obj);
            }
        
            var prefabID = PrefabIDs[instanceID];

            if (!Pools.ContainsKey(prefabID))
            {
                Pools[prefabID] = new ObjectPoolStack<UnityEngine.Object>();
            }

            var gameObject = GetGameObject(obj);
            Pools[prefabID].Push(gameObject);
            gameObject.SetActive(false);

            if (TryGetComponent(obj, out IOnReleasedToPool call))
            {
                call.OnReleasedToPool();
            }
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
            Clear(prefab.gameObject);
        }

        public static void Clear(UnityEngine.GameObject prefab)
        {
            var instanceID = GetInstanceID(prefab);
            
            Clear(instanceID);
        }

        public static void Clear(int prefabID)
        {
            if (!Pools.ContainsKey(prefabID)) return;

            var pool = Pools[prefabID];

            foreach (var obj in pool)
            {
                PrefabIDs.Remove(GetInstanceID(obj));
                Destroy(obj);
            }
            
            pool.Clear();
        }
        

        private static bool TryGetObjectFromPoolWithPrefabID<T>(int prefabID, out T obj)
            where T : UnityEngine.Object
        {
            if (!Pools.ContainsKey(prefabID))
            {
                obj = null;
                return false;
            }

            if (!Pools[prefabID].TryPop(out var poolObj))
            {
                obj = null;
                return false;
            }

            obj = (T) poolObj;
            GetGameObject(obj).SetActive(true);
            return true;
        }

        private static T InstantiateAndPutInPool<T>(
            T prefab, 
            int prefabID,
            UnityEngine.Vector3 position,
            UnityEngine.Quaternion rotation,
            UnityEngine.Transform parent = null)
            where T : UnityEngine.Object
        {
            var obj = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);

            if (TryGetComponent(obj, out IOnInstantiated instantiatedCall))
            {
                instantiatedCall.OnInstantiated();
            }

            if (TryGetComponent(obj, out IOnGetFromPool getCall))
            {
                getCall.OnGetFromPool();
            }
            
            var instanceID = GetInstanceID(obj);
            PrefabIDs[instanceID] = prefabID;

            return obj;
        }

        private static int GetInstanceID(UnityEngine.Object obj)
        {
            return obj.GetInstanceID();
        }

        private static void Destroy(UnityEngine.Object obj)
        {
            if (obj is UnityEngine.GameObject gameObject)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
            
            else if (obj is UnityEngine.Component component)
            {
                UnityEngine.Object.Destroy(component.gameObject);
            }
        }

        private static UnityEngine.GameObject GetGameObject(UnityEngine.Object obj)
        {
            return obj switch
            {
                UnityEngine.GameObject gameObject => gameObject,
                UnityEngine.Component component => component.gameObject,
                _ => default
            };
        }

        private static UnityEngine.Transform GetTransform(UnityEngine.Object obj)
        {
            return obj switch
            {
                UnityEngine.GameObject gameObject => gameObject.transform,
                UnityEngine.Component component => component.transform,
                _ => default
            };
        }

        private static bool TryGetComponent<T>(UnityEngine.Object obj, out T component)
        {
            if (obj is UnityEngine.GameObject gameObject)
            {
                return gameObject.TryGetComponent(out component);
            }

            if (obj is UnityEngine.Component comp)
            {
                return comp.TryGetComponent(out component);
            }

            component = default;
            return false;
        }
    }
}