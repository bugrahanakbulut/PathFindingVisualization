using TileSystem;
using TMPro;
using UnityEngine;

namespace ResetSystem
{
    public class WaypointResetable : Resetable
    {
        [SerializeField] private Vector3 _resetPos = new Vector3(-30, 0, 0);

        [SerializeField] private Tile _tile = null;
        
        public override void Reset()
        {
            _tile.InitTile(new Vector2Int(-1, -1));

            transform.position = _resetPos;
        }
    }
}