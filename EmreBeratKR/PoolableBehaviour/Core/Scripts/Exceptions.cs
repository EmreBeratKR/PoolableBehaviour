using UnityEngine;

namespace EmreBeratKR.PoolableBehaviour
{
    public class InvalidPoolCapacityException : UnityException
    {
        public InvalidPoolCapacityException()
            : base("Invalid Pool Capacity!")
        {
            HResult = -2147467261;
        }
    }
}