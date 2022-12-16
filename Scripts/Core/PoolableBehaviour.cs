using UnityEngine;

namespace EmreBeratKR.PB
{
    public abstract class PoolableBehaviour<T> : MonoBehaviour, IPoolableBehaviour<T> 
        where T : MonoBehaviour, IPoolableBehaviour<T>
    {
        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }


        protected virtual BehaviourPool<T> Pool { get; set; }


        public virtual void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public virtual void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public virtual void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public virtual void Release()
        {
            Pool.ReleaseObject(this as T);
        }

        public virtual void Inject(BehaviourPool<T> pool)
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