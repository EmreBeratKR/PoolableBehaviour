namespace EmreBeratKR.ObjectPool
{
    public class ObjectPoolExceptions : System.Exception 
    {
        private ObjectPoolExceptions(string message) : base(message){}
        
        
        public static ObjectPoolExceptions ObjectDoesNotExistInPool(UnityEngine.Object obj)
        {
            var message = $"{obj} does not exist in the pool!";
            return new ObjectPoolExceptions(message);
        }
    }
}