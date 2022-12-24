using UnityEngine;

namespace EmreBeratKR.PoolableBehaviour
{
    public interface IBehaviourPool
    {
        int CountAll { get; }
        int CountActive { get; }
        int CountInactive { get; }
        void ChangeCapacity(int capacity);
        void Clear();
    }
    
    public interface IBehaviourPool<T> : IBehaviourPool
    {
        T GetObject(T prefab);
        T GetObject(T prefab, Transform parent);
        T GetObject(T prefab, Vector3 position);
        T GetObject(T prefab, Vector3 position, Quaternion rotation);
        T GetObject(T prefab, Vector3 position, Quaternion rotation, Transform parent);
        void ReleaseObject(T obj);
    }
}