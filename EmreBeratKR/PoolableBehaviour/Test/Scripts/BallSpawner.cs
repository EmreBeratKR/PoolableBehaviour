using UnityEngine;

namespace EmreBeratKR.PoolableBehaviour.Test
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Ball ballPrefab;
        [SerializeField] private int iterationCount;
        [SerializeField] private bool spawn;


        private void Update()
        {
            if (!spawn) return;
            
            for (int i = 0; i < iterationCount; i++)
            {
                Instantiate(ballPrefab);
            }
        }


        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
