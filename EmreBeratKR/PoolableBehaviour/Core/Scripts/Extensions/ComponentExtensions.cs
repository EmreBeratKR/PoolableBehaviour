namespace EmreBeratKR.ObjectPool
{
    public static class ComponentExtensions
    {
        public static T Get<T>(this T component)
            where T : UnityEngine.Component
        {
            return ObjectPool.Get(component);
        }
        
        public static T Get<T>(this T component, UnityEngine.Transform parent)
            where T : UnityEngine.Component
        {
            return ObjectPool.Get(component, parent);
        }
        
        public static T Get<T>(this T component, UnityEngine.Vector3 position)
            where T : UnityEngine.Component
        {
            return ObjectPool.Get(component, position);
        }

        public static T Get<T>(
            this T component, 
            UnityEngine.Vector3 position,
            UnityEngine.Quaternion rotation,
            UnityEngine.Transform parent = null)
            where T : UnityEngine.Component
        {
            return ObjectPool.Get(component, position, rotation, parent);
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

        public static void Clear<T>(this T component)
            where T : UnityEngine.Component
        {
            ObjectPool.Clear(component);
        }
    }
}