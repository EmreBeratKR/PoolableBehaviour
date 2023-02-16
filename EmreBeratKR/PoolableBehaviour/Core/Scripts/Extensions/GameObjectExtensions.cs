namespace EmreBeratKR.ObjectPool
{
    public static class GameObjectExtensions
    {
        public static UnityEngine.GameObject Get(this UnityEngine.GameObject gameObject)
        {
            return ObjectPool.Get(gameObject);
        }

        public static void Release(this UnityEngine.GameObject gameObject)
        {
            ObjectPool.Release(gameObject);
        }

        public static void Fill(this UnityEngine.GameObject gameObject, int count)
        {
            ObjectPool.Fill(gameObject, count);
        }
    }
}