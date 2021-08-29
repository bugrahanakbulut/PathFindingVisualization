using System;
using System.Collections;
using System.Collections.Generic;
using TileSystem;
using UnityEngine;

namespace PathSystem
{
    public class PathFinder_Dijkstra : PathFinder
    {
        private readonly Vector2Int[] _directions = new[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0)
        };

        private IEnumerator _pathFindingRoutine = null;
        private IEnumerator _pathVisualizationRoutine = null;
        
        public override EPathFinder GetPathFinderType()
        {
            return EPathFinder.Dijkstra;
        }

        public override void FindPath(Tile[][] ground, Tile source, Tile destination)
        {
            StartPathFindingRoutine(ground, source, destination);
        }

        public override void StopPathFinding()
        {
            StopPathFindingRoutine();
        }

        private void StartPathFindingRoutine(Tile[][] ground, Tile source, Tile destination)
        {
            StopPathFindingRoutine();

            _pathFindingRoutine = PathFindingProgress(ground, source, destination);

            StartCoroutine(_pathFindingRoutine);
        }

        private void StopPathFindingRoutine()
        {
            if (_pathFindingRoutine != null)
            {
                StopCoroutine(_pathFindingRoutine);
            }

            if (_pathVisualizationRoutine != null)
            {
                StopCoroutine(_pathVisualizationRoutine);
            }
        }

        private IEnumerator PathFindingProgress(Tile[][] ground, Tile source, Tile destination)
        {
            bool[,] isVisited = new bool[ground.Length, ground[0].Length];
            Tile[,] parents = new Tile[ground.Length, ground[0].Length];
            int[,] costs = new int[ground.Length, ground[0].Length];

            for (int i = 0; i < ground.Length; i++)
            {
                for (int j = 0; j < ground[0].Length; j++)
                {
                    costs[i, j] = Int32.MaxValue;
                }
            }

            List<Vector2Int> tilesToVisit = new List<Vector2Int>();
            
            tilesToVisit.Add(source.TilePos);
            costs[source.TilePos.x, source.TilePos.y] = 0;
            
            while (tilesToVisit.Count != 0)
            {
                Tile curTile = GetMinimumCostTile(ground, tilesToVisit, costs);
                PathObject pathObject = curTile.GetComponent<PathObject>();

                tilesToVisit.Remove(curTile.TilePos);
                
                pathObject.PathObjectSelected();
                
                if (curTile.TilePos.Equals(destination.TilePos))
                {
                    Debug.Log("Path Found via Dijkstra.");

                    Tile tmp = destination;
                    
                    tmp.GetComponent<PathObject>().PathObjectIncludedPath();

                    while (tmp != null)
                    {
                        tmp = parents[tmp.TilePos.x, tmp.TilePos.y];
                        
                        if (tmp != null)
                        {
                            tmp.GetComponent<PathObject>().PathObjectIncludedPath();
                        }

                        yield return null;
                    }
                    
                    source.GetComponent<PathObject>().PathObjectIncludedPath();
                    
                    break;
                }

                isVisited[curTile.TilePos.x, curTile.TilePos.y] = true;

                foreach (Vector2Int direction in _directions)
                {
                    bool nextPosValidation = 
                        CheckPositionIsValid(ground, curTile.TilePos.x + direction.x, curTile.TilePos.y + direction.y);

                    int curCost = costs[curTile.TilePos.x, curTile.TilePos.y];

                    if (nextPosValidation)
                    {
                        Vector2Int nextPos = new Vector2Int(curTile.TilePos.x + direction.x,
                            curTile.TilePos.y + direction.y);

                        if (costs[nextPos.x, nextPos.y] > curCost + 1)
                        {
                            costs[nextPos.x, nextPos.y] = curCost + 1;
                            parents[nextPos.x, nextPos.y] = curTile;

                            if (!isVisited[nextPos.x, nextPos.y])
                            {
                                tilesToVisit.Add(nextPos);
                            }
                        }
                    }
                }
                
                pathObject.PathObjectDiscovered();

                yield return null;
            }
        }

        private Tile GetMinimumCostTile(Tile[][] ground, List<Vector2Int> tiles, int[,] costs)
        {
            Tile minCostTile = null;
            int minCost = Int32.MaxValue;

            foreach (Vector2Int tilePos in tiles)
            {
                int cost = costs[tilePos.x, tilePos.y];
                
                if (minCost > cost)
                {
                    minCost = cost;
                    minCostTile = ground[tilePos.x][tilePos.y];
                }
            }

            return minCostTile;
        }

        private bool CheckPositionIsValid(Tile[][] ground, int row, int col)
        {
            bool isRowValid = row < ground.Length && row >= 0;
            bool isColValid = col < ground[0].Length && col >= 0;

            if (!(isRowValid && isColValid))
            {
                return false;
            }
            
            if (ground[row][col] is BlockTile)
            {
                return false;
            }
            
            return true;
        }
    }
}