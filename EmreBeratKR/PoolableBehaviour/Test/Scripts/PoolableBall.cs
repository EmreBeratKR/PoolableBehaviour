using UnityEngine;
using Random = UnityEngine.Random;

namespace EmreBeratKR.PoolableBehaviour.Test
{
    public class PoolableBall : PoolableBehaviour<PoolableBall>
    {
        [SerializeField] private Rigidbody body;


        private float m_SpawnTime;


        private void Update()
        {
            if (Time.time - m_SpawnTime > 1f)
            {
                Release();
            }
        }


        public override void OnAfterInitialized()
        {
            body.velocity = Random.insideUnitSphere.normalized * Random.Range(10f, 30f);
            m_SpawnTime = Time.time;
        }
    }
}