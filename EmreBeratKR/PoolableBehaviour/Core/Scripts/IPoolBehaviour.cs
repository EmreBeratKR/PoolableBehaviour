namespace EmreBeratKR.ObjectPool
{
    public interface IPoolBehaviour
    {
        void OnInstantiated();
        void OnGetFromPool();
        void OnReleasedToPool();
    }
}