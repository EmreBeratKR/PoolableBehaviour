using System;
using UnityEngine;
using UnityEngine.Events;

namespace EmreBeratKR.PB
{
    public abstract class PoolableBehaviour<T> : MonoBehaviour, IPoolableBehaviour<T>, IPoolableBehaviour
        where T : MonoBehaviour, IPoolableBehaviour<T>, IPoolableBehaviour
    {
        public GameObject GameObject
        {
            get
            {
                Cache();
                return m_GameObject;
            }
        }

        public Transform Transform
        {
            get
            {
                Cache();
                return m_Transform;
            }
        }

        public int ID
        {
            get
            {
                Cache();
                return m_Id;
            }
        }


        protected virtual IBehaviourPool<T> Pool { get; set; }


        private GameObject m_GameObject;
        private Transform m_Transform;
        private int m_Id;
        private bool m_IsCached;
        

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


        private void Cache()
        {
            if (m_IsCached) return;
            
            m_GameObject = gameObject;
            m_Transform = transform;
            m_Id = GetInstanceID();

            m_IsCached = true;
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