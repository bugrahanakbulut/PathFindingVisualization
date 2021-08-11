using UnityEngine;

namespace TileSystem
{
    public interface ITilePositionProvider
    {
        Vector2Int TilePos { get; }
    }
}