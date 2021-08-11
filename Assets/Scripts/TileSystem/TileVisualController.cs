using BLUE.PoolingSystem;
using UnityEngine;

namespace TileSystem
{
    public class TileVisualController : MonoBehaviour
    {
        [SerializeField] private ScaleTween _scaleTween = null;

        [SerializeField] private PoolObject _poolObject = null;
    
        private void Awake()
        {
            _poolObject.OnAllocated += OnPoolObjectAllocated;
            _poolObject.OnDeallocated += OnPoolObjectDeallocated;
        }

        private void OnDestroy()
        {
            _poolObject.OnAllocated -= OnPoolObjectAllocated;
            _poolObject.OnDeallocated -= OnPoolObjectDeallocated;
        }

        private void OnPoolObjectAllocated(PoolObject poolObject)
        {
            _scaleTween.Play();
        }

        private void OnPoolObjectDeallocated(PoolObject poolObject)
        {
            _scaleTween.PlayReverse();
        }
    }
}
