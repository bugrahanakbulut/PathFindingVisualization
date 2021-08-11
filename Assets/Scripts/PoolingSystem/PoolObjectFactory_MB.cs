using System;
using System.Collections.Generic;
using UnityEngine;

namespace BLUE.PoolingSystem
{
    public class PoolObjectFactory_MB : PoolObjectFactory
    {
        private Dictionary<int, Transform> _poolCarrierMap;
        
        /// <summary>
        /// Create instance of referenced pool object
        /// </summary>
        /// <param name="poolObject">Pool object reference to create instance.</param>
        /// <param name="poolIndex">Index of the pool carrier</param>
        /// <returns>Created pool object instance.</returns>
        public override PoolObject CreateInstanceOf(PoolObject poolObject, int poolIndex = 0)
        {
            Transform poolCarrier = GetPoolCarrierByIndex(poolIndex);
            
            GameObject instance = Instantiate(poolObject.gameObject, Vector3.zero, Quaternion.identity, poolCarrier);

            PoolObject poolObj = instance.GetComponent<PoolObject>();

            return poolObj;
        }

        /// <summary>
        /// Tries to destroy pool object immediately.
        /// </summary>
        /// <param name="poolObject">Pool object to destroy</param>
        /// <returns></returns>
        public override bool DestroyInstance(PoolObject poolObject)
        {
            try
            {
                DestroyImmediate(poolObject.gameObject);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);

                return false;
            }
        }

        private Transform GetPoolCarrierByIndex(int index)
        {
            if (_poolCarrierMap == null)
            {
                _poolCarrierMap = new Dictionary<int, Transform>();
            }

            if (_poolCarrierMap.ContainsKey(index))
            {
                return _poolCarrierMap[index];
            }
            
            GameObject newPoolCarr = new GameObject();
            newPoolCarr.transform.parent = transform;
            newPoolCarr.name = index.ToString();
            
            _poolCarrierMap.Add(index, newPoolCarr.transform);

            return _poolCarrierMap[index];
        }
    }
}