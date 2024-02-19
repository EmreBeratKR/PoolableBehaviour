using System.Collections.Generic;
using UnityEngine;

namespace EmreBeratKR.ObjectPool
{
    public static class ObjectPool
    {
        private static readonly Dictionary<int, PoolStack> POOL_STACKS = new();
        private static readonly Dictionary<int, int> PREFAB_IDS = new();


        public static T Get<T>(T prefab)
            where T : Object
        {
            return Get(prefab, Vector3.zero, Quaternion.identity);
        }
        
        public static T Get<T>(T prefab, Transform parent)
            where T : Object
        {
            return Get(prefab, Vector3.zero, Quaternion.identity, parent);
        }
        
        public static T Get<T>(T prefab, Vector3 position)
            where T : Object
        {
            return Get(prefab, position, Quaternion.identity);
        }

        public static T Get<T>(
            T prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
            where T : Object
        {
            var prefabID = GetInstanceID(prefab);

            if (TryGetObjectFromPoolWithPrefabID(prefabID, out T obj))
            {
                var objTransform = GetTransform(obj);
                objTransform.position = position;
                objTransform.rotation = rotation;
                objTransform.parent = parent;
                
                if (obj is IPoolObjectOnGetFromPool call)
                {
                    call.OnGetFromPool();
                }
                
                return obj;
            }

            return InstantiateAndPutInPool(prefab, prefabID, position, rotation, parent);
        }

        public static void Release<T>(T obj)
            where T : Object
        {
            var instanceID = GetInstanceID(obj);

            if (!PREFAB_IDS.ContainsKey(instanceID))
            {
                throw ObjectPoolExceptions.ObjectDoesNotExistInPool(obj);
            }
        
            var prefabID = PREFAB_IDS[instanceID];
            var gameObject = GetGameObject(obj);
            
            POOL_STACKS[prefabID].Push(obj);
            gameObject.SetActive(false);

            if (obj is IPoolObjectOnReleasedToPool call)
            {
                call.OnReleasedToPool();
            }
        }

        public static void Fill<T>(T prefab, int count)
            where T : Object
        {
            var prefabID = GetInstanceID(prefab);
            
            for (var i = 0; i < count; i++)
            {
                var obj = InstantiateAndPutInPool(prefab, prefabID, Vector3.zero, Quaternion.identity);
                Release(obj);
            }
        }

        public static void Clear(Object prefab)
        {
            var instanceID = GetInstanceID(prefab);
            
            ClearByInstanceID(instanceID);
        }
        

        private static void ClearByInstanceID(int prefabID)
        {
            if (!POOL_STACKS.ContainsKey(prefabID)) return;

            var poolStack = POOL_STACKS[prefabID];

            foreach (var obj in poolStack.allObjects)
            {
                PREFAB_IDS.Remove(GetInstanceID(obj));
                Destroy(obj);
            }
            
            poolStack.Clear();
        }
        
        private static bool TryGetObjectFromPoolWithPrefabID<T>(int prefabID, out T obj)
            where T : Object
        {
            if (!POOL_STACKS.ContainsKey(prefabID))
            {
                obj = null;
                return false;
            }

            if (!POOL_STACKS[prefabID].TryPop(out var poolObj))
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
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
            where T : Object
        {
            var obj = Object.Instantiate(prefab, position, rotation, parent);
            var instanceID = GetInstanceID(obj);

            if (obj is IPoolObjectOnInstantiated instantiatedCall)
            {
                instantiatedCall.OnInstantiated();
            }

            if (obj is IPoolObjectOnGetFromPool getCall)
            {
                getCall.OnGetFromPool();
            }

            if (!POOL_STACKS.ContainsKey(prefabID))
            {
                POOL_STACKS[prefabID] = new PoolStack();
            }
            
            POOL_STACKS[prefabID].allObjects.Add(obj);
            PREFAB_IDS[instanceID] = prefabID;

            return obj;
        }

        private static int GetInstanceID(Object obj)
        {
            return obj.GetInstanceID();
        }

        private static void Destroy(Object obj)
        {
            if (obj is GameObject gameObject)
            {
                Object.Destroy(gameObject);
            }
            
            else if (obj is Component component)
            {
                Object.Destroy(component.gameObject);
            }
        }

        private static GameObject GetGameObject(Object obj)
        {
            return obj switch
            {
                GameObject gameObject => gameObject,
                Component component => component.gameObject,
                _ => default
            };
        }

        private static Transform GetTransform(Object obj)
        {
            return obj switch
            {
                GameObject gameObject => gameObject.transform,
                Component component => component.transform,
                _ => default
            };
        }
    }
}