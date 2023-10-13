using System;
using System.Collections.Generic;
using UnityEngine;

namespace EmreBeratKR.ObjectPool.Test
{
    public class Test_ObjectPool : MonoBehaviour
    {
        private List<GameObject> objs = new();
        private GameObject obj;

        private void Start()
        {
            obj = new GameObject("Prefab");
            DontDestroyOnLoad(obj);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var newObj = obj.Get();
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
        }
    }
}