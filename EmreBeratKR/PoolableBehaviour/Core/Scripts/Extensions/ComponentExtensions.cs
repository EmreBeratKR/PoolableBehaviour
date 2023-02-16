namespace EmreBeratKR.ObjectPool
{
    public static class ComponentExtensions
    {
        public static T Get<T>(this T component)
            where T : UnityEngine.Component
        {
            return ObjectPool.Get(component);
        }

        public static void Release<T>(this T component)
            where T : UnityEngine.Component
        { 
            ObjectPool.Release(component);
        }
    }
}