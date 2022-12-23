using EmreBeratKR.PB;
using UnityEngine;

namespace Test
{
    public class SeparateBallSpawner : MonoBehaviour
    {
        [SerializeField] private PoolableBehaviourSpawner poolableSpawner;
        [SerializeField] private int iterationCount;
        [SerializeField] private bool spawn;
        
        
        private void Update()
        {
            if (!spawn) return;
            
            for (int i = 0; i < iterationCount; i++)
            {
                poolableSpawner.Spawn();
            }
        }
        
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}