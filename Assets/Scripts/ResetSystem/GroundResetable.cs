using TileSystem;
using UnityEngine;

namespace ResetSystem
{
    public class GroundResetable : Resetable
    {
        [SerializeField] private Ground _ground = null;

        public override void Reset()
        {
            for (int i = 0; i < _ground.GroundTiles.Length; i++)
            {
                for (int j = 0; j < _ground.GroundTiles[0].Length; j++)
                {
                    if (_ground.GroundTiles[i][j] is BlockTile)
                    {
                        _ground.GroundTiles[i][j] = (_ground.GroundTiles[i][j] as BlockTile).StandingTile;
                    } 
                }
            }
        }
    }
}