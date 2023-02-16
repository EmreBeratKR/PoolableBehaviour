namespace EmreBeratKR.ObjectPool
{
    public static class GameObjectExtensions
    {
        public static UnityEngine.GameObject Get(this UnityEngine.GameObject component)
        {
            return ObjectPool.Get(component);
        }

        public static void Release(this UnityEngine.GameObject component)
        {
            ObjectPool.Release(component);
        }
    }
}