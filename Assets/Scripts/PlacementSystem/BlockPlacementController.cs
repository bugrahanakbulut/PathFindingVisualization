using System;
using BLUE.PoolingSystem;
using TileSystem;
using UnityEngine;

public class BlockPlacementController : MonoBehaviour
{
    [SerializeField] private TileSelector_ISelectable _blockPlaceSelector = null;

    [SerializeField] private TileSelector_IDeletable _blockDeletionSelector = null;
    
    [SerializeField] private Pool_MB _blockPool = null;

    [SerializeField] private float _yOffset = 0;
    
    public Action<BlockTile> OnBlockPlaced { get; set; }
    
    public Action<BlockTile> OnBlockRemoved { get; set; }
    
    private void Awake()
    {
        _blockPlaceSelector.OnTileSelected += OnTileSelected;
        _blockDeletionSelector.OnTileSelected += OnTileSelectedDeletable;
    }

    private void OnDestroy()
    {
        _blockPlaceSelector.OnTileSelected -= OnTileSelected;
        _blockDeletionSelector.OnTileSelected -= OnTileSelectedDeletable;
    }

    private void OnTileSelected(ISelectableTile selectableTile, Vector3 position, Vector2Int tilePos)
    {
        PoolObject_MB poolObject = _blockPool.AllocatePoolObject() as PoolObject_MB;

        poolObject.transform.position = new Vector3(position.x, position.y + _yOffset, position.z);

        BlockTile tile = poolObject.GetComponent<BlockTile>();

        tile.StandingTile = selectableTile.GetTile();
        
        if (tile != null)
        {
            tile.InitTile(tilePos);
            
            OnBlockPlaced?.Invoke(tile as BlockTile);
        }
    }
    
    private void OnTileSelectedDeletable(IDeletableTile deletableTile, Vector3 position, Vector2Int tilePos)
    {
        Tile tile = deletableTile.GetTile();

        PoolObject_MB poolObject = tile.GetComponent<PoolObject_MB>();

        if (poolObject != null)
        {
            poolObject.Deallocate();
            
            OnBlockRemoved?.Invoke(tile as BlockTile);
        }
    }
}
