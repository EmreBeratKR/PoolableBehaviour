using System.Collections.Generic;
using UnityEngine;

namespace EmreBeratKR.ObjectPool
{
    public class PoolStack
    {
        public readonly Stack<Object> inActiveObjects = new();
        public readonly List<Object> allObjects = new();


        public void Push(Object obj)
        {
            inActiveObjects.Push(obj);
        }

        public bool TryPop(out Object obj)
        {
            return inActiveObjects.TryPop(out obj);
        }

        public int Count()
        {
            return allObjects.Count;
        }

        public void Clear()
        {
            inActiveObjects.Clear();
            allObjects.Clear();
        }
    }
}