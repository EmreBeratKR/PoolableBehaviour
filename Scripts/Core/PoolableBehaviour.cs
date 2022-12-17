using System;
using UnityEngine;
using UnityEngine.Events;

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

    public sealed class PoolableBehaviour : PoolableBehaviour<PoolableBehaviour>
    {
        public Callbacks callbacks;


        public override void OnReset()
        {
            callbacks.onReset?.Invoke();
        }

        public override void OnBeforeInitialized()
        {
            callbacks.onBeforeInitialized?.Invoke();
        }

        public override void OnAfterInitialized()
        {
            callbacks.onAfterInitialized?.Invoke();
        }


        [Serializable]
        public class Callbacks
        {
            public UnityEvent onReset;
            public UnityEvent onBeforeInitialized;
            public UnityEvent onAfterInitialized;
        }
    }
}