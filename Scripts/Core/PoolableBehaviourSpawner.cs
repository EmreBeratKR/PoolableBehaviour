using UnityEngine;

namespace EmreBeratKR.PB
{
    public abstract class PoolableBehaviourSpawner<T> : MonoBehaviour
        where T : MonoBehaviour, IPoolableBehaviour<T>
    {
        protected virtual BehaviourPool<T> Pool { get; set; }
        protected abstract T Prefab { get; }


        protected virtual void Awake()
        {
            Pool = new BehaviourPool<T>();
        }


        public T Spawn()
        {
            return Spawn(Vector3.zero, Quaternion.identity);
        }
        
        public T Spawn(Vector3 position)
        {
            return Spawn(position, Quaternion.identity);
        }
        
        public T Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var newObject = Pool.GetObject(Prefab, position, rotation, parent);
            return newObject;
        }
    }
}