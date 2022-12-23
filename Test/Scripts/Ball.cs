using UnityEngine;
using Random = UnityEngine.Random;

namespace Test
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;

        
        private float m_SpawnTime;


        private void Update()
        {
            if (Time.time - m_SpawnTime > 1f)
            {
                Destroy(gameObject);
            }
        }
        

        private void Start()
        {
            body.velocity = Random.insideUnitSphere.normalized * Random.Range(10f, 30f);
            m_SpawnTime = Time.time;
        }
    }
}
