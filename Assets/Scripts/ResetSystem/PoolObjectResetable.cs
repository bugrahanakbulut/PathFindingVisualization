using PoolingSystem;
using UnityEngine;

namespace ResetSystem
{
    public class PoolObjectResetable : Resetable
    {
        [SerializeField] private PoolObject_MB _poolObject = null;
        
        public override void Reset()
        {
            _poolObject.Deallocate();   
        }
    }
}