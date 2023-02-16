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

        public static void Fill<T>(this T component, int count)
            where T : UnityEngine.Component
        {
            ObjectPool.Fill(component, count);
        }
    }
}