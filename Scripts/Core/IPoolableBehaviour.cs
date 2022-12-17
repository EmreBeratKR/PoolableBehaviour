using UnityEngine;

namespace EmreBeratKR.PB
{
    public interface IPoolableBehaviour<T> 
        where T : MonoBehaviour, IPoolableBehaviour<T>
    {
        bool IsActive { get; set; }
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        void SetParent(Transform parent);
        void Destroy();
        void Release();
        void Inject(BehaviourPool<T> pool);
        void OnReset();
        void OnBeforeInitialized();
        void OnAfterInitialized();
    }
}