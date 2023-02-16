using UnityEngine;

namespace EmreBeratKR.ObjectPool
{
    public class PoolBehaviour : MonoBehaviour, IPoolBehaviour
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