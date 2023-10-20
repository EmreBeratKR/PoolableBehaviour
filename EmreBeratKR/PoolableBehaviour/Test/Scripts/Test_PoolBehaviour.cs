using UnityEngine;

namespace EmreBeratKR.ObjectPool.Test
{
    public class Test_PoolBehaviour : PoolBehaviour
    {
        public override void OnInstantiated()
        {
            Debug.Log("instantiated");
        }

        public override void OnGetFromPool()
        {
            Debug.Log("Get from pool");
        }

        public override void OnReleasedToPool()
        {
            Debug.Log("Release to pool");
        }
    }
}