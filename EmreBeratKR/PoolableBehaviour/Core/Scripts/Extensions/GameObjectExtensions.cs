namespace EmreBeratKR.ObjectPool
{
    public static class GameObjectExtensions
    {
        public static UnityEngine.GameObject Get(this UnityEngine.GameObject gameObject)
        {
            return ObjectPool.Get(gameObject);
        }
        
        public static UnityEngine.GameObject Get(this UnityEngine.GameObject gameObject, UnityEngine.Transform parent)
        {
            return ObjectPool.Get(gameObject, parent);
        }

        public static UnityEngine.GameObject Get(this UnityEngine.GameObject gameObject, UnityEngine.Vector3 position)
        {
            return ObjectPool.Get(gameObject, position);
        }
        
        public static UnityEngine.GameObject Get(
            this UnityEngine.GameObject gameObject, 
            UnityEngine.Vector3 position,
            UnityEngine.Quaternion rotation,
            UnityEngine.Transform parent = null)
        {
            return ObjectPool.Get(gameObject, position, rotation, parent);
        }
        
        public static void Release(this UnityEngine.GameObject gameObject)
        {
            ObjectPool.Release(gameObject);
        }

        public static void Fill(this UnityEngine.GameObject gameObject, int count)
        {
            ObjectPool.Fill(gameObject, count);
        }

        public static void Clear(this UnityEngine.GameObject gameObject)
        {
            ObjectPool.Clear(gameObject);
        }
    }
}