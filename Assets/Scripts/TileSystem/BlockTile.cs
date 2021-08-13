using System;
using PoolingSystem;
using UnityEngine;

namespace TileSystem
{
    public class BlockTile : Tile,
        IDeletableTile
    {
        [SerializeField] private PoolObject_MB _poolObject = null;

        [SerializeField] private BoxCollider _boxCollider = null;
        
        public Tile StandingTile { get; set; }
        
        public Tile GetTile()
        {
            return this;
        }

        private void Awake()
        {
            _poolObject.OnAllocated += OnAllocated;
            _poolObject.OnDeallocated += OnDeallocated;
        }

        private void OnDestroy()
        {
            _poolObject.OnAllocated -= OnAllocated;
            _poolObject.OnDeallocated -= OnDeallocated;
        }

        private void OnAllocated(PoolObject poolObject)
        {
            _boxCollider.enabled = true;
        }

        private void OnDeallocated(PoolObject poolObject)
        {
            _boxCollider.enabled = false;
        }
    }
}
