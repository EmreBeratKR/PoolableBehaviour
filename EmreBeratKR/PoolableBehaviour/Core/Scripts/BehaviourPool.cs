using System.Collections.Generic;
using UnityEngine;

namespace EmreBeratKR.PoolableBehaviour
{
    public class BehaviourPool<T> : IBehaviourPool<T>, IBehaviourPool
        where T : MonoBehaviour, IPoolableBehaviour<T>, IPoolableBehaviour
    {
        public int CountAll => CountActive + CountInactive;

        public int CountActive => m_ActiveObjects.Count;

        public int CountInactive => m_InactiveObjects.Count;


        private bool IsFull => !IsInfinityCapacity && CountAll >= m_CurrentCapacity;
        private bool IsInfinityCapacity => m_CurrentCapacity == Constant.InfinityCapacity;


        private readonly Dictionary<int, T> m_ActiveObjects = new Dictionary<int, T>();
        private readonly Queue<T> m_InactiveObjects = new Queue<T>();
        private int m_CurrentCapacity;
        private int m_InitialCapacity;

        
        public BehaviourPool(int capacity = Constant.InfinityCapacity)
        {
            ChangeCapacity(capacity);
        }

        public BehaviourPool(T prefab, int prefillCount, Transform parent = null, int capacity = Constant.InfinityCapacity)
        {
           ChangeCapacity(capacity);
           Prefill(prefab, prefillCount, parent);
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
            if (m_InactiveObjects.TryDequeue(out var firstInactiveObject))
            {
                firstInactiveObject.OnBeforeInitialized();
                var firstInactiveObjectTransform = firstInactiveObject.Transform;
                firstInactiveObjectTransform.position = position;
                firstInactiveObjectTransform.rotation = rotation;
                firstInactiveObjectTransform.SetParent(parent);
                firstInactiveObject.GameObject.SetActive(true);
                m_ActiveObjects[firstInactiveObject.ID] = firstInactiveObject;
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
            m_ActiveObjects[newObject.ID] = newObject;
            newObject.OnAfterInitialized();
            return newObject;
        }

        public void ReleaseObject(T obj)
        {
            obj.OnReset();
            obj.GameObject.SetActive(false);
            m_ActiveObjects.Remove(obj.ID);
            m_InactiveObjects.Enqueue(obj);
        }

        public void ChangeCapacity(int capacity)
        {
            if (capacity <= 0 && capacity != Constant.InfinityCapacity)
            {
                throw new InvalidPoolCapacityException();
            }
            
            m_CurrentCapacity = capacity;
            m_InitialCapacity = capacity;
        }

        public void Clear()
        {
            foreach (var (id, obj) in m_ActiveObjects)
            {
                Object.Destroy(obj.GameObject);
            }

            foreach (var obj in m_InactiveObjects)
            {
                Object.Destroy(obj.GameObject);
            }
            
            m_ActiveObjects.Clear();
            m_InactiveObjects.Clear();
        }


        private void Prefill(T prefab, int count, Transform parent)
        {
            for (var i = 0; i < count; i++)
            {
                var newObject = Object.Instantiate(prefab, parent);
                newObject.Inject(this);
                m_InactiveObjects.Enqueue(newObject);
                newObject.GameObject.SetActive(false);
            }
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