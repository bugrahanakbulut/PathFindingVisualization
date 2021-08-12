using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathSystem;
using TileSystem;
using UnityEngine;

public class PathFinder_AStar : PathFinder
{
    private int[][] _gCosts;
    private int[][] _hCosts;
    private Tile[][] _parents;

    private IEnumerator _pathFindingRoutine;
    
    private readonly Vector2Int[] _directions = new[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0)
    };
    
    public override EPathFinder GetPathFinderType()
    {
        return EPathFinder.AStar;
    }

    public override void FindPath(Tile[][] ground, Tile source, Tile destination)
    {
        _pathFindingRoutine = FindPathProgress(ground, source, destination);
        
        StartCoroutine(_pathFindingRoutine);
    }

    public override void StopPathFinding()
    {
        if (_pathFindingRoutine != null)
        {
            StopCoroutine(_pathFindingRoutine);
        }
    }

    private void InitMatrices(int row, int col)
    {
        _gCosts = new int[row][];
        _hCosts = new int[row][]; 
        _parents = new Tile[row][];

        for (int i = 0; i < row; i++)
        {
            _gCosts[i] = new int[col];
            _hCosts[i] = new int[col];
            _parents[i] = new Tile[col];

            for (int j = 0; j < col; j++)
            {
                _gCosts[i][j] = -1;
                _hCosts[i][j] = -1;
            }
        }
    }

    private IEnumerator FindPathProgress(Tile[][] ground, Tile source, Tile destination)
    {
        InitMatrices(ground.Length, ground[0].Length);
        
        List<Tile> openTiles = new List<Tile>();
        List<Tile> closedTiles = new List<Tile>();
        
        openTiles.Add(source);
        
        while (openTiles.Count > 0)
        {
            Tile tile = GetMostPromisingTile(openTiles, source, destination);
            
            tile.GetComponent<PathObject>().PathObjectDiscovered();

            if (tile.TilePos == destination.TilePos)
            {
                Debug.Log("Destination Tile Found.");
                
                Tile tmp = destination;

                while (tmp != null)
                {
                    tmp.GetComponent<PathObject>().PathObjectIncludedPath();

                    tmp = _parents[tmp.TilePos.x][tmp.TilePos.y];
                }
                
                yield break;
            }

            foreach (Vector2Int direction in _directions)
            {
                if (CheckPositionIsValid(ground, tile.TilePos.x + direction.x, tile.TilePos.y + direction.y))
                {
                    Tile closed = closedTiles.FirstOrDefault(i =>
                        i.TilePos == new Vector2Int(tile.TilePos.x + direction.x, tile.TilePos.y + direction.y));
                    
                    Tile open = openTiles.FirstOrDefault(i =>
                        i.TilePos == new Vector2Int(tile.TilePos.x + direction.x, tile.TilePos.y + direction.y));
                    
                    if (closed == null && open == null)
                    {
                        openTiles.Add(ground[tile.TilePos.x + direction.x][tile.TilePos.y + direction.y]);
                        
                        ground[tile.TilePos.x + direction.x][tile.TilePos.y + direction.y].GetComponent<PathObject>().PathObjectSelected();
                        
                        if (_parents[tile.TilePos.x + direction.x][tile.TilePos.y + direction.y] == null)
                        {
                            _parents[tile.TilePos.x + direction.x][tile.TilePos.y + direction.y] = tile;
                        }
                    }
                }
            }

            openTiles.Remove(tile);
            
            closedTiles.Add(tile);
            
            yield return null;
        }

        yield return null;
    }

    private Tile GetMostPromisingTile(List<Tile> openTiles, Tile source, Tile destination)
    {
        Tile tile = null;
        int currentFCost = Int32.MaxValue;

        foreach (Tile openTile in openTiles)
        {
            int fCost = GetFCost(_parents[openTile.TilePos.x][openTile.TilePos.y], openTile, source, destination);

            if (fCost < currentFCost)
            {
                tile = openTile;
                currentFCost = fCost;
            }
        }

        return tile;
    }

    private int GetFCost(Tile parent, Tile cur, Tile source, Tile destination)
    {
        return GetGCost(_gCosts, _parents, parent, source, cur) + GetHCost(_hCosts, cur, destination);
    }

    private int GetGCost(int[][] gCosts, Tile[][] parents, Tile parent, Tile from, Tile to)
    {
        int curCost = gCosts[from.TilePos.x][from.TilePos.y];

        int newCost = 0;
        
        if (parent != null)
        {
            newCost = gCosts[parent.TilePos.x][parent.TilePos.y] + 1;
        }
        
        if (curCost == -1)
        {
            gCosts[from.TilePos.x][from.TilePos.y] = newCost;

            parents[from.TilePos.x][from.TilePos.y] = parent;
        }
        else
        {
            if (newCost < gCosts[from.TilePos.x][from.TilePos.x])
            {
                gCosts[from.TilePos.x][from.TilePos.y] = newCost;

                parents[from.TilePos.x][from.TilePos.y] = parent;
            }
        }

        return gCosts[from.TilePos.x][from.TilePos.y];
    }

    private int GetHCost(int[][] hCosts, Tile from, Tile to)
    {
        if (hCosts[from.TilePos.x][from.TilePos.y] != -1)
        {
            return hCosts[from.TilePos.x][from.TilePos.y];
        }

        int manhattanDistance = Math.Abs(to.TilePos.x - from.TilePos.x) + Math.Abs(to.TilePos.y - from.TilePos.y);

        hCosts[from.TilePos.x][from.TilePos.y] = manhattanDistance;

        return hCosts[from.TilePos.x][from.TilePos.y];
    }

    private bool CheckPositionIsValid(Tile[][] ground, int row, int col)
    {
        bool isRowValid = row >= 0 && row < ground.Length;
        bool isColValid = col >= 0 && col < ground[0].Length;

        if (!isRowValid || !isColValid)
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
