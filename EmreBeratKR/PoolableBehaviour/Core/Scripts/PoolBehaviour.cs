using UnityEngine;

namespace EmreBeratKR.ObjectPool
{
    public abstract class PoolBehaviour : MonoBehaviour,
        IPoolObjectOnInstantiated,
        IPoolObjectOnGetFromPool,
        IPoolObjectOnReleasedToPool
    {
        public virtual void OnInstantiated() {}
        public virtual void OnGetFromPool() {}
        public virtual void OnReleasedToPool() {}
        

        public void Release()
        {
            ObjectPool.Release(this);
        }
    }
}