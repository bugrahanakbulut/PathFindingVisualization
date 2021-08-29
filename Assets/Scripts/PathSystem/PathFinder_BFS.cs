using System.Collections;
using System.Collections.Generic;
using TileSystem;
using UnityEngine;

namespace PathSystem
{
    public class PathFinder_BFS : PathFinder
    {
        private IEnumerator _pathFindingRoutine;

        private IEnumerator _traverseRoutine;
        
        private readonly Vector2Int[] _directions = new[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0)
        };
        
        public override EPathFinder GetPathFinderType()
        {
            return EPathFinder.BFS;
        }

        public override void FindPath(Tile[][] ground, Tile source, Tile destination)
        {
            _pathFindingRoutine = PathFindingProgress(ground, source, destination);
            
            StartCoroutine(_pathFindingRoutine);
        }

        public override void StopPathFinding()
        {
            if (_pathFindingRoutine != null)
            {
                StopCoroutine(_pathFindingRoutine);
            }

            if (_traverseRoutine != null)
            {
                StopCoroutine(_traverseRoutine);
            }
        }

        private IEnumerator PathFindingProgress(Tile[][] ground, Tile source, Tile destination)
        {
            if (!source.CheckTilePositionIsValid() || !destination.CheckTilePositionIsValid())
            {
                yield break;
            }
            
            int rowCount = ground.Length;
            int colCount = ground[0].Length;
            
            bool[][] isVisited = new bool[rowCount][];
            
            Tile[][] parents = new Tile[rowCount][];
            
            for (int i = 0; i < rowCount; i++)
            {
                isVisited[i] = new bool[colCount];
                parents[i] = new Tile[colCount];
            }
            
            parents[source.TilePos.x][source.TilePos.y] = null;
            
            Queue<Tile> tileQueue = new Queue<Tile>();
            
            tileQueue.Enqueue(source);

            while (tileQueue.Count != 0)
            {
                Tile curTile = tileQueue.Dequeue();
                PathObject pathObj = curTile.GetComponent<PathObject>();
                Vector2Int curPos = curTile.TilePos;
                
                if (isVisited[curPos.x][curPos.y])
                {
                    continue;
                }
                
                isVisited[curPos.x][curPos.y] = true;
                
                pathObj.PathObjectSelected();
                
                yield return null;
                
                if (curTile.TilePos == destination.TilePos)
                {
                    Debug.Log("Destination Tile Found.");
                    
                    destination.GetComponent<PathObject>().PathObjectSelected();

                    _traverseRoutine = TraversePath(parents, destination);
                    
                    StartCoroutine(_traverseRoutine);
                
                    yield break;
                }

                foreach (Vector2Int direction in _directions)
                {
                    if (CheckPositionIsValid(isVisited, ground, curPos.x + direction.x, curPos.y + direction.y))
                    {
                        tileQueue.Enqueue(ground[curPos.x + direction.x][curPos.y + direction.y]);

                        parents[curPos.x + direction.x][curPos.y + direction.y] = curTile;
                    }
                }
                
                pathObj.PathObjectDiscovered();
            }
        }

        private bool CheckPositionIsValid(bool[][] isVisited, Tile[][] ground, int row, int col)
        {
            bool isRowValid = row >= 0 && row < ground.Length;
            bool isColValid = col >= 0 && col < ground[0].Length;

            if (!isRowValid || !isColValid)
            {
                return false;
            }

            if (isVisited[row][col])
            {
                return false;
            }

            if (ground[row][col] is BlockTile)
            {
                return false;
            }

            return true;
        }

        private IEnumerator TraversePath(Tile[][] parents, Tile destination)
        {
            Tile tmp = destination;

            while (tmp != null)
            {
                tmp.GetComponent<PathObject>().PathObjectIncludedPath();
                
                tmp = parents[tmp.TilePos.x][tmp.TilePos.y];

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}