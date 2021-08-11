namespace TileSystem
{
    public class GroundSelectableTile : Tile, 
        ISelectableTile
    {
        public Tile GetTile()
        {
            return this;
        }
    }
}
