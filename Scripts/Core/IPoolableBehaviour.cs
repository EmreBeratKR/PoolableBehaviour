using UnityEngine;

namespace EmreBeratKR.PB
{
    public interface IPoolableBehaviour
    {
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