using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EmreBeratKR.PB
{
    [Serializable]
    public class BehaviourPool<T> 
        where T : MonoBehaviour, IPoolableBehaviour<T>
    {
        private const int InfinityCapacity = -1;
        
        
        public int CountAll => CountActive + CountInactive;
        public int CountActive => m_ActiveObjects.Count;
        public int CountInactive => m_InactiveObjects.Count;


        private bool IsFull => !IsInfinityCapacity && CountAll >= m_CurrentCapacity;
        private bool HasInactiveObject => m_InactiveObjects.Count > 0;
        private bool IsInfinityCapacity => m_CurrentCapacity == InfinityCapacity;


        private List<T> m_ActiveObjects = new List<T>();
        private List<T> m_InactiveObjects = new List<T>();
        private int m_CurrentCapacity;
        private int m_InitialCapacity;

        
        public BehaviourPool(int capacity = InfinityCapacity)
        {
            ChangeCapacity(capacity);
        }


        public T GetObject(T prefab)
        {
            return GetObject(prefab, Vector3.zero, Quaternion.identity);
        }

        public T GetObject(T prefab, Vector3 position)
        {
            return GetObject(prefab, position, Quaternion.identity);
        }
        
        public T GetObject(T prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (HasInactiveObject)
            {
                var firstInactiveObject = m_InactiveObjects[0];
                firstInactiveObject.OnBeforeInitialized();
                firstInactiveObject.SetPosition(position);
                firstInactiveObject.SetRotation(rotation);
                firstInactiveObject.SetParent(parent);
                m_InactiveObjects.RemoveAt(0);
                firstInactiveObject.IsActive = true;
                m_ActiveObjects.Add(firstInactiveObject);
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
            m_ActiveObjects.Add(newObject);
            newObject.OnAfterInitialized();
            return newObject;
        }

        public void ReleaseObject(T obj)
        {
            obj.OnReset();
            obj.IsActive = false;
            m_ActiveObjects.Remove(obj);
            m_InactiveObjects.Add(obj);
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
            foreach (var activeObject in m_ActiveObjects)
            {
                Object.Destroy(activeObject);
            }

            foreach (var inactiveObject in m_InactiveObjects)
            {
                Object.Destroy(inactiveObject);
            }
            
            m_ActiveObjects.Clear();
            m_InactiveObjects.Clear();
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