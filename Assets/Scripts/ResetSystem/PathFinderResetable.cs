using PathSystem;
using UnityEngine;

namespace ResetSystem
{
    public class PathFinderResetable : Resetable
    {
        [SerializeField] private PathFinder[] _pathFinders = null;
        
        public override void Reset()
        {
            foreach (PathFinder pathFinder in _pathFinders)
            {
                pathFinder.StopPathFinding();
            }
        }
    }
}
