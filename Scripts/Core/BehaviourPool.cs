using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EmreBeratKR.PB
{
    [Serializable]
    public class BehaviourPool<T> : IBehaviourPool<T>, IBehaviourPool
        where T : MonoBehaviour, IPoolableBehaviour<T>, IPoolableBehaviour
    {
        private const int InfinityCapacity = -1;


        public int CountAll => m_Objects.Count;

        public int CountActive
        {
            get
            {
                var count = 0;

                for (var i = 0; i < m_Objects.Count; i++)
                {
                    var obj = m_Objects[i];
                    
                    if (!obj.gameObject.activeSelf) continue;

                    count++;
                }

                return count;
            }
        }

        public int CountInactive
        {
            get
            {
                var count = 0;
                
                for (var i = 0; i < m_Objects.Count; i++)
                {
                    var obj = m_Objects[i];
                    
                    if (obj.gameObject.activeSelf) continue;

                    count++;
                }

                return count;
            }
        }


        private bool IsFull => !IsInfinityCapacity && CountAll >= m_CurrentCapacity;
        private bool IsInfinityCapacity => m_CurrentCapacity == InfinityCapacity;


        private List<T> m_Objects = new List<T>();
        private int m_CurrentCapacity;
        private int m_InitialCapacity;

        
        public BehaviourPool(int capacity = InfinityCapacity)
        {
            ChangeCapacity(capacity);
        }


        public T GetObject(T prefab)
        {
            return GetObject(prefab, null);
        }
        
        public T GetObject(T prefab, Transform parent)
        {
            return GetObject(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public T GetObject(T prefab, Vector3 position)
        {
            return GetObject(prefab, position, Quaternion.identity);
        }
        
        public T GetObject(T prefab, Vector3 position, Quaternion rotation)
        {
            return GetObject(prefab, position, rotation, null);
        }
        
        public T GetObject(T prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (TryGetFirstInactiveObject(out var firstInactiveObject))
            {
                firstInactiveObject.OnBeforeInitialized();
                var firstInactiveObjectTransform = firstInactiveObject.transform;
                firstInactiveObjectTransform.position = position;
                firstInactiveObjectTransform.rotation = rotation;
                firstInactiveObjectTransform.SetParent(parent);
                firstInactiveObject.gameObject.SetActive(true);
                firstInactiveObject.OnAfterInitialized();
                return firstInactiveObject;
            }

            if (IsFull)
            {
                IncreaseCapacity();
            }

            var newObject = Object.Instantiate(prefab, position, rotation, parent);
            newObject.OnBeforeInitialized();
            newObject.Inject(this);
            m_Objects.Add(newObject);
            newObject.OnAfterInitialized();
            return newObject;
        }

        public void ReleaseObject(T obj)
        {
            obj.OnReset();
            obj.gameObject.SetActive(false);
        }

        public void ChangeCapacity(int capacity)
        {
            if (capacity <= 0 && capacity != InfinityCapacity)
            {
                throw new InvalidPoolCapacityException();
            }
            
            m_CurrentCapacity = capacity;
            m_InitialCapacity = capacity;
        }

        public void Clear()
        {
            foreach (var obj in m_Objects)
            {
                Object.Destroy(obj.gameObject);
            }
            
            m_Objects.Clear();
        }


        private bool TryGetFirstInactiveObject(out T firstInactiveObject)
        {
            for (var i = 0; i < m_Objects.Count; i++)
            {
                var obj = m_Objects[i];
                    
                if (obj.gameObject.activeSelf) continue;

                firstInactiveObject = obj;
                return true;
            }

            firstInactiveObject = null;
            return false;
        }
        
        private void IncreaseCapacity()
        {
            var oldCapacity = m_CurrentCapacity;
            m_CurrentCapacity += m_InitialCapacity;
            var newCapacity = m_CurrentCapacity;
            
            Debug.LogWarning($"Pool Capacity has Increased automatically from {oldCapacity} to {newCapacity}, consider setting a larger capacity.");
        }
    }
}