using UnityEngine;

namespace TileSystem
{
    public class WaypointTile : Tile
    {
        public void UpdateTilePos(Vector2Int tilePos)
        {
            TilePos = tilePos;
        }
    }
}
