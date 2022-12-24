using UnityEngine;

namespace EmreBeratKR.PoolableBehaviour
{
    public interface IPoolableBehaviour
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
        int ID { get; }
        void Release();
        void OnReset();
        void OnBeforeInitialized();
        void OnAfterInitialized();
    }
    
    public interface IPoolableBehaviour<T> : IPoolableBehaviour
        where T : MonoBehaviour, IPoolableBehaviour, IPoolableBehaviour<T> 
    {
        void Inject(IBehaviourPool<T> pool);
    }
}