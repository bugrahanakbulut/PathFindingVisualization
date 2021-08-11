using System.Collections.Generic;
using UnityEngine;

namespace BLUE.PoolingSystem
{
    public class Pool_MB : Pool<PoolObject_MB>
    {
        [SerializeField] private List<PoolSettings> _poolSettingsColl = null;
        
        [SerializeField] private bool _initOnAwake = false;

        private void Awake()
        {
            if (_initOnAwake)
            {
                InitPool(_poolSettingsColl);
            }
        }
    }
}