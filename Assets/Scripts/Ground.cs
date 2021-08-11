using System.Collections;
using BLUE.PoolingSystem;
using TileSystem;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Pool_MB _tilePool = null;

    [SerializeField] private BlockPlacementController _blockPlacementController = null;

    [SerializeField] private int _rowCount = 0;

    [SerializeField] private int _colCount = 0;

    private Tile[][] _ground;

    public Tile[][] GroundTiles => _ground;

    
    private void Awake()
    {
        InitGround();

        _blockPlacementController.OnBlockPlaced += OnBlockPlaced;
        _blockPlacementController.OnBlockRemoved += OnBlockRemoved;
    }

    private void OnDestroy()
    {
        _blockPlacementController.OnBlockPlaced -= OnBlockPlaced;
        _blockPlacementController.OnBlockRemoved -= OnBlockRemoved;
    }

    private void InitGround()
    {
        _ground = new Tile[_rowCount][];

        for (int i = 0; i < _rowCount; i++)
        {
            _ground[i] = new Tile[_colCount];
        }
        
        StartCoroutine(GroundInitializationProgress());
    }
    
    private void OnBlockPlaced(BlockTile block)
    {
        Vector2Int tilePos = block.TilePos;

        _ground[tilePos.x][tilePos.y] = block;
    }
    
    private void OnBlockRemoved(BlockTile block)
    {
        Vector2Int tilePos = block.TilePos;

        _ground[tilePos.x][tilePos.y] = block.StandingTile;
    }

    private IEnumerator GroundInitializationProgress()
    {
        for (int i = 0; i < _rowCount ; i++)
        {
            for (int j = 0; j < _colCount; j++)
            {
                PoolObject_MB tilePoolObj = _tilePool.AllocatePoolObject() as PoolObject_MB;

                Tile tile = tilePoolObj.GetComponent<Tile>();
                
                tile.InitTile(new Vector2Int(i, j));

                tile.transform.position = new Vector3(j - _colCount / 2, 0, (_rowCount / 2) - i);
                
                _ground[i][j] = tile;

                yield return null;
            }
        }
    }
}
