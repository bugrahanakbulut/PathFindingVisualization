using System;
using UnityEngine;

namespace PoolingSystem
{
    public abstract class PoolObject : MonoBehaviour
    {
        public int PoolIndex { get; private set; }

        #region Events

        public Action<PoolObject> OnAllocated { get; set; }
    
        public Action<PoolObject> OnDeallocated { get; set; }

        #endregion

        protected virtual void AllocateCustomActions() { }
        
        protected virtual void DeallocateCustomActions() { }
        
        /// <summary>
        /// Init pool object with id.
        /// </summary>
        /// <param name="poolIndex"></param>
        public void InitPoolObject(int poolIndex)
        {
            PoolIndex = poolIndex;
        }

        /// <summary>
        /// Allocate pool object
        /// </summary>
        public void Allocate()
        {
            AllocateCustomActions();
            
            OnAllocated?.Invoke(this);
        }

        /// <summary>
        /// Deallocate pool object
        /// </summary>
        public void Deallocate()
        {
            DeallocateCustomActions();

            OnDeallocated?.Invoke(this);
        }
    }
}
