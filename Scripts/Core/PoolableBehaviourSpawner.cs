using UnityEngine;

namespace EmreBeratKR.PB
{
    public abstract class PoolableBehaviourSpawner<T> : BasePoolableBehaviourSpawner<T>
        where T : MonoBehaviour, IPoolableBehaviour, IPoolableBehaviour<T>
    {
        [SerializeField] private T prefab;
        [SerializeField] private Transform parent;
        [SerializeField, Min(0)] private int prefillCount;
        [SerializeField, Min(-1)] private int capacity = Constant.InfinityCapacity;


        protected override T Prefab => prefab;
        protected override Transform Parent => parent;
        protected override int PrefillCount => prefillCount;
        protected override int Capacity => capacity;
    }

    public sealed class PoolableBehaviourSpawner : PoolableBehaviourSpawner<PoolableBehaviour>
    {
        
    }
}