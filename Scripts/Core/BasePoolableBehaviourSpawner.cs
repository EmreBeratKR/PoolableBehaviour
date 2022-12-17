using UnityEngine;

namespace EmreBeratKR.PB
{
    public abstract class BasePoolableBehaviourSpawner<T> : MonoBehaviour
        where T : MonoBehaviour, IPoolableBehaviour, IPoolableBehaviour<T>
    {
        public int CountAll => m_Pool.CountAll;
        public int CountActive => m_Pool.CountActive;
        public int CountInactive => m_Pool.CountInactive;
        
        
        protected abstract T Prefab { get; }
        protected virtual Transform Parent => null;
        protected virtual int PrefillCount => 0;
        protected virtual int Capacity => BehaviourPool<T>.InfinityCapacity;


        private IBehaviourPool<T> m_Pool;


        protected virtual void Awake()
        {
            m_Pool = new BehaviourPool<T>(Prefab, PrefillCount, Parent, Capacity);
        }


        public T Spawn()
        {
            return Spawn(Vector3.zero, Quaternion.identity);
        }
        
        public T Spawn(Vector3 position)
        {
            return Spawn(position, Quaternion.identity);
        }
        
        public T Spawn(Vector3 position, Quaternion rotation)
        {
            var newObject = m_Pool.GetObject(Prefab, position, rotation, Parent);
            return newObject;
        }

        public void ChangeCapacity(int capacity)
        {
            m_Pool.ChangeCapacity(capacity);
        }

        public void Clear()
        {
            m_Pool.Clear();
        }
    }
}