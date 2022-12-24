using UnityEngine;
using Random = UnityEngine.Random;

namespace EmreBeratKR.PoolableBehaviour.Test
{
    public class SeparateBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private PoolableBehaviour poolable;


        private float m_SpawnTime;


        private void Awake()
        {
            poolable.callbacks.onAfterInitialized.AddListener(OnAfterInitialized);
        }

        private void OnDestroy()
        {
            poolable.callbacks.onAfterInitialized.RemoveListener(OnAfterInitialized);
        }


        private void Update()
        {
            if (Time.time - m_SpawnTime > 1f)
            {
                poolable.Release();
            }
        }
        

        public void OnAfterInitialized()
        {
            body.velocity = Random.insideUnitSphere.normalized * Random.Range(10f, 30f);
            m_SpawnTime = Time.time;
        }
    }
}