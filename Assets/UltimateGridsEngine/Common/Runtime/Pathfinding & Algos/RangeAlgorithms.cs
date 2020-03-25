using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public static class RangeAlgorithms {

    public static List<GridTile> SearchByGridPosition(GridTile start, int reach, bool ignoresHeight = false, bool includeStartingTile = false) {
        List<GridTile> closed = new List<GridTile>();
        List<GridTile> tilesInRange = new List<GridTile>();
        SimplePriorityQueue<GridTile> frontier = new SimplePriorityQueue<GridTile>();
        frontier.Enqueue(start, 0);

        GridTile current = start;

        while (frontier.Count > 0) {
            current = frontier.Dequeue();
            if (!tilesInRange.Contains(current)) {
                tilesInRange.Add(current);
                closed.Add(current);
            }

            foreach (GridTile next in GridManager.Instance.Neighbors(current, ignoresHeight, defaultSixDirections)) {

                if (closed.Contains(next))
                    continue;

                float priority = Heuristic(next, start);
                if (priority <= reach) {
                    frontier.Enqueue(next, priority);
                } 
            }
        }

        // remove the starting tile if required
        if (!includeStartingTile) {
            if (tilesInRange.Contains(start)) {
                tilesInRange.Remove(start);
            }
        }

        return tilesInRange;
    }

    public static List<GridTile> HexSearchByGridPosition(GridTile start, int reach, bool ignoresHeight = false, bool includeStartingTile = false) {
        List<GridTile> closed = new List<GridTile>();
        List<GridTile> tilesInRange = new List<GridTile>();
        SimplePriorityQueue<GridTile> frontier = new SimplePriorityQueue<GridTile>();
        frontier.Enqueue(start, 0);

        GridTile current = start;

        while (frontier.Count > 0) {
            current = frontier.Dequeue();
            if (!tilesInRange.Contains(current)) {
                tilesInRange.Add(current);
                closed.Add(current);
            }

            foreach (GridTile next in GridManager.Instance.Neighbors(current, ignoresHeight, defaultSixDirections)) {

                if (closed.Contains(next))
                    continue;

                float priority = HexDistance(next, start);
                if (priority <= reach) {
                    frontier.Enqueue(next, priority);
                } 
            }
        }

        // remove the starting tile if required
        if (!includeStartingTile) {
            if (tilesInRange.Contains(start)) {
                tilesInRange.Remove(start);
            }
        }

        return tilesInRange;
    }
    
    public static float Heuristic(GridTile a, GridTile b) {
        return Mathf.Abs(a.m_GridPosition.x - b.m_GridPosition.x) + Mathf.Abs(a.m_GridPosition.y - b.m_GridPosition.y);
    }

    public static float HexCubeDistance(Vector3Int a, Vector3Int b) {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }

    public static Vector3Int OddRToCubeCoordinattes(Vector2Int gridPosition) {
        var x = gridPosition.x - (gridPosition.y - (gridPosition.y&1)) / 2;
        var z = gridPosition.y;
        var y = -x-z;
        return new Vector3Int(x, y, z);
    }

    public static float HexDistance(GridTile a, GridTile b) {
        var aCube = OddRToCubeCoordinattes(a.m_GridPosition);
        var bCube = OddRToCubeCoordinattes(b.m_GridPosition);
        return HexCubeDistance(aCube, bCube);
    }

    public static List<Vector2Int> defaultSixDirections = new List<Vector2Int>() {
        new Vector2Int( -1, 0 ), // left
        new Vector2Int( 0, 1 ),  // top
        new Vector2Int( 1, 0 ),  // right
        new Vector2Int( 0, -1 ), // bottom
        // Diagonals
        new Vector2Int( 1, 1 ),  
        new Vector2Int( 1, -1 ), 
        new Vector2Int( -1, 1 ),  
        new Vector2Int( -1, -1 ), 
        
    };
}