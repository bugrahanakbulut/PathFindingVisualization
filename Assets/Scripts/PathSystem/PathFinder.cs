using TileSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PathSystem
{
    public enum EPathFinder
    {
        None = 0,
        BFS = 1,
        AStar = 2,
        Dijkstra = 3
    }
    
    public abstract class PathFinder : MonoBehaviour
    {
        public abstract EPathFinder GetPathFinderType();
        
        public abstract void FindPath(Tile[][] ground, Tile source, Tile destination);

        public abstract void StopPathFinding();
    }
}
