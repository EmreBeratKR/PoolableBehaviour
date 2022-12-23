using EmreBeratKR.PB;
using UnityEngine;

namespace Test
{
    public class PoolableBallSpawner : PoolableBehaviourSpawner<PoolableBall>
    {
        [SerializeField] private int iterationCount;
        [SerializeField] private bool spawn;
        
        
        private void Update()
        {
            if (!spawn) return;
            
            for (int i = 0; i < iterationCount; i++)
            { 
                Spawn();
            }
        }
        
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}