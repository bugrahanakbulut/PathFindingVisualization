using System;
using System.Collections.Generic;
using UnityEngine;

namespace BLUE.PoolingSystem
{
    [Serializable]
    public class PoolSettings
    {
        [SerializeField] private PoolObject _referenceObject = null;
        public PoolObject ReferenceObject => _referenceObject;
        

        [SerializeField] private PoolObjectFactory _poolObjectFactory = null;
        public PoolObjectFactory PoolObjectFactory => _poolObjectFactory;

        public PoolSettings(PoolObject referenceObject, PoolObjectFactory poolObjectFactory)
        {
            _referenceObject = referenceObject;
            _poolObjectFactory = poolObjectFactory;
        }
    }
    
    public abstract class Pool<T> : MonoBehaviour
        where T : PoolObject
    {
        private Dictionary<PoolSettings, List<PoolObject>> _unavailablePoolObjectsMap;
        private Dictionary<PoolSettings, List<PoolObject>> _availablePoolObjectsMap;
        
        public int PoolCount => _settingsColl.Count;
        
        private List<PoolSettings> _settingsColl = null;
        public List<PoolSettings> SettingsColl => _settingsColl;

        /// <summary>
        /// Initialize pools with settings which include reference pool object and corresponding factory
        /// If user want to create multiple pool it can pass setting list as parameter
        /// After initialization user can access pool same order with settings list
        /// </summary>
        /// <param name="poolSettingsColl"></param>
        public void InitPool(List<PoolSettings> poolSettingsColl)
        {
            foreach (PoolSettings settings in poolSettingsColl)
            {
                InitPool(settings);
            }
        }
        
        /// <summary>
        /// Initializes pool with single setting
        /// </summary>
        /// <param name="poolSettings"></param>
        public void InitPool(PoolSettings poolSettings)
        {
            if (_settingsColl == null)
            {
                _settingsColl = new List<PoolSettings>();
                _unavailablePoolObjectsMap = new Dictionary<PoolSettings, List<PoolObject>>();
                _availablePoolObjectsMap = new Dictionary<PoolSettings, List<PoolObject>>();
            }
            
            if (!_settingsColl.Contains(poolSettings))
            {
                _settingsColl.Add(poolSettings);
            }
            
            if (!_unavailablePoolObjectsMap.ContainsKey(poolSettings))
            {
                _unavailablePoolObjectsMap.Add(poolSettings, new List<PoolObject>());
            }
            if (!_availablePoolObjectsMap.ContainsKey(poolSettings))
            {
                _availablePoolObjectsMap.Add(poolSettings, new List<PoolObject>());
            }
        }

        /// <summary>
        /// Allocates pool objects. Creates if there is no available objects
        /// If there are multiple pool user can access pool with same order settings list
        /// which was passed in initialization
        /// </summary>
        /// <param name="index">Index of referenced pool if there are multiple pools.</param>
        /// <returns>After allocated it returns new pool object.</returns>
        public PoolObject AllocatePoolObject(int index = 0)
        {
            PoolSettings refSettings = _settingsColl[index];

            PoolObject poolObj = null;
            
            if (_availablePoolObjectsMap[refSettings].Count == 0)
            {
                poolObj = refSettings.PoolObjectFactory.CreateInstanceOf(refSettings.ReferenceObject, index);
                poolObj.OnDeallocated += OnPoolObjectDeallocated;
                poolObj.InitPoolObject(index);

                _unavailablePoolObjectsMap[refSettings].Add(poolObj);
                
                poolObj.Allocate();

                return poolObj;
            }

            poolObj = _availablePoolObjectsMap[refSettings][0];
            poolObj.OnDeallocated += OnPoolObjectDeallocated;
            
            _availablePoolObjectsMap[refSettings].RemoveAt(0);
            _unavailablePoolObjectsMap[refSettings].Add(poolObj);
            
            poolObj.Allocate();
            
            return poolObj;
        }
        
        /// <summary>
        /// Deallocates referenced pool object.
        /// </summary>
        /// <param name="poolObject"></param>
        public void DeallocatePoolObject(PoolObject poolObject)
        {
            poolObject.Deallocate();
        }

        /// <summary>
        /// Deallocates all pool objects in the pool.
        /// </summary>
        /// <param name="index">Index of referenced pool if there are multiple pools.</param>
        public void FreePool(int index = 0)
        {
            PoolSettings refSettings = _settingsColl[index];

            List<PoolObject> poolObjects = new List<PoolObject>(_unavailablePoolObjectsMap[refSettings]);

            foreach (PoolObject poolObject in poolObjects)
            {
                poolObject.Deallocate();
            }
        }

        /// <summary>
        /// Destroys all allocated and deallocated pools and pool objects
        /// </summary>
        public void ResetPool()
        {
            foreach (PoolSettings poolSettings in _settingsColl)
            {
                ErasePool(poolSettings);
            }
            
            InitPool(_settingsColl);
        }
        
        private void OnPoolObjectDeallocated(PoolObject poolObject)
        {
            poolObject.OnDeallocated -= OnPoolObjectDeallocated;

            _unavailablePoolObjectsMap[_settingsColl[poolObject.PoolIndex]].Remove(poolObject);
            
            _availablePoolObjectsMap[_settingsColl[poolObject.PoolIndex]].Add(poolObject);
        }

        private void ErasePool(PoolSettings settings)
        {
            foreach (PoolObject poolObject in _unavailablePoolObjectsMap[settings])
            {
                settings.PoolObjectFactory.DestroyInstance(poolObject);
            }
            
            _unavailablePoolObjectsMap[settings].Clear();

            _unavailablePoolObjectsMap.Remove(settings);

            foreach (PoolObject poolObject in _availablePoolObjectsMap[settings])
            {
                settings.PoolObjectFactory.DestroyInstance(poolObject);
            }
            
            _availablePoolObjectsMap[settings].Clear();

            _availablePoolObjectsMap.Remove(settings);
        }
    }
}
