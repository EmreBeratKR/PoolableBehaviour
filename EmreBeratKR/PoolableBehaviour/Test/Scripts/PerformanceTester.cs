using UnityEngine;

namespace EmreBeratKR.PoolableBehaviour.Test
{
    public class PerformanceTester : MonoBehaviour
    {
        [Header("References")]
        public BallSpawner monoSpawner;
        public SeparateBallSpawner separateSpawner;
        public PoolableBallSpawner poolableSpawner;
        
        [Header("Settings")]
        public TestType testType;
        public float testDuration;
        
        [Header("Progress")]
        public float timeLeft;
        
        [Header("Results")]
        public string averageCpuTime;
        public string averageFps;
        
        
        private int m_FrameCount;
        private float m_StartTime;
        private bool m_TestStarted;


        private void Update()
        {
            var elapsedSeconds = Time.time - m_StartTime;
            timeLeft = Mathf.Clamp(testDuration * 60f - elapsedSeconds, 0f, testDuration * 60f);

            if (timeLeft <= 0f)
            {
                monoSpawner.SetActive(false);
                separateSpawner.SetActive(false);
                poolableSpawner.SetActive(false);
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && !m_TestStarted)
            {
                switch (testType)
                {
                    case TestType.Normal:
                        monoSpawner.SetActive(true);
                        break;
                    
                    case TestType.ObjectPoolSeperated:
                        separateSpawner.SetActive(true);
                        break;
                    
                    case TestType.ObjectPool:
                        poolableSpawner.SetActive(true);
                        break;
                }

                m_TestStarted = true;
                m_StartTime = Time.time;
            }
            
            if (!m_TestStarted) return;
            
            m_FrameCount++;
            
            averageCpuTime = $"{elapsedSeconds / m_FrameCount} ms";
            averageFps = $"{m_FrameCount / elapsedSeconds} fps";
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 200f, 50f), "MonoBehaviour"))
            {
                ResetFpsCounter();
                
                monoSpawner.SetActive(true);
                separateSpawner.SetActive(false);
                poolableSpawner.SetActive(false);
            }
            
            if (GUI.Button(new Rect(0f, 50f, 200f, 50f), "SeparateBehaviour"))
            {
                ResetFpsCounter();
                
                monoSpawner.SetActive(false);
                separateSpawner.SetActive(true);
                poolableSpawner.SetActive(false);
            }
            
            if (GUI.Button(new Rect(0f, 100f, 200f, 50f), "PoolableBehaviour"))
            {
                ResetFpsCounter();
                
                monoSpawner.SetActive(false);
                separateSpawner.SetActive(false);
                poolableSpawner.SetActive(true);
            }
        }


        private void ResetFpsCounter()
        {
            m_FrameCount = 0;
            m_StartTime = Time.time;
        }
        
        
        public enum TestType
        {
            Normal,
            ObjectPoolSeperated,
            ObjectPool
        }
    }
}