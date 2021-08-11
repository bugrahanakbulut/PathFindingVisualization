using UnityEngine;

namespace BLUE.PoolingSystem
{
    public abstract class PoolObjectFactory : MonoBehaviour
    {
        public abstract PoolObject CreateInstanceOf(PoolObject poolObject, int poolIndex = 0);

        public abstract bool DestroyInstance(PoolObject poolObject);
    }
}