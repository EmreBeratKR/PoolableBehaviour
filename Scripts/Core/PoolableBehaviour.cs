using UnityEngine;

namespace EmreBeratKR.PB
{
    public abstract class PoolableBehaviour<T> : MonoBehaviour, IPoolableBehaviour<T>, IPoolableBehaviour
        where T : MonoBehaviour, IPoolableBehaviour<T>, IPoolableBehaviour
    {
        protected virtual IBehaviourPool<T> Pool { get; set; }
        

        public virtual void Release()
        {
            Pool.ReleaseObject(this as T);
        }

        public virtual void Inject(IBehaviourPool<T> pool)
        {
            Pool = pool;
        }

        public virtual void OnReset()
        {
            
        }

        public virtual void OnBeforeInitialized()
        {
            
        }

        public virtual void OnAfterInitialized()
        {
            
        }
    }
}