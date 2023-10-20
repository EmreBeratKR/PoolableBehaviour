using System.Collections.Generic;
using UnityEngine;

namespace EmreBeratKR.ObjectPool.Test
{
    public class Test_ObjectPool : MonoBehaviour
    {
        private List<GameObject> objs = new();
        private Test_PoolBehaviour obj;

        private void Start()
        {
            obj = new GameObject("Prefab")
                .AddComponent<Test_PoolBehaviour>();
            DontDestroyOnLoad(obj);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var newObj = obj.gameObject.Get();
                objs.Add(newObj.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (objs.Count > 0)
                {
                    objs[0].Release();
                    objs.RemoveAt(0);
                }
            }
        }
    }
}