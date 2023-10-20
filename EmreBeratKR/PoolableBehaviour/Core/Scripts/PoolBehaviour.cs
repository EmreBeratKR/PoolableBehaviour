using UnityEngine;

namespace EmreBeratKR.ObjectPool
{
    public abstract class PoolBehaviour : MonoBehaviour,
        IOnInstantiated,
        IOnGetFromPool,
        IOnReleasedToPool
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