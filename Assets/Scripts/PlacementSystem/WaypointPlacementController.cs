using TileSystem;
using UnityEngine;

public class WaypointPlacementController : MonoBehaviour
{
    [SerializeField] private TileSelector_ISelectable _tileSelector = null;

    [SerializeField] private float _yOffset = 0;

    [SerializeField] private WaypointTile _tile = null;
    
    private void Awake()
    {
        _tile.InitTile(new Vector2Int(-1, -1));

        _tileSelector.OnTileSelected += OnTileSelected;
    }

    private void OnDestroy()
    {
        _tileSelector.OnTileSelected -= OnTileSelected;
    }

    private void OnTileSelected(ISelectableTile selectableTile, Vector3 pos, Vector2Int tilePos)
    {
        _tile.UpdateTilePos(tilePos);
        
        _tile.transform.position = new Vector3(pos.x, pos.y + _yOffset, pos.z);
    }
}
