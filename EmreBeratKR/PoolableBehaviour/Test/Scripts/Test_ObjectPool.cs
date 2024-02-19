using System.Collections.Generic;
using UnityEngine;

namespace EmreBeratKR.ObjectPool.Test
{
    public class Test_ObjectPool : MonoBehaviour
    {
        private List<Test_PoolBehaviour> objs = new();
        private Test_PoolBehaviour prefab;

        private void Start()
        {
            prefab = new GameObject("Prefab")
                .AddComponent<Test_PoolBehaviour>();
            DontDestroyOnLoad(prefab);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var newObj = prefab.Get();
                objs.Add(newObj);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (objs.Count > 0)
                {
                    objs[0].Release();
                    objs.RemoveAt(0);
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                prefab.Clear();
            }
        }
    }
}