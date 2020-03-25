using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessQueen : ChessPiece {
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint) {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(BishopDirections);
        directions.AddRange(RookDirections);

        // Movement
        foreach (Vector2Int dir in directions) {
            for (int i = 1; i < 8; i++) {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                if (GridManager.GetGridTileAtPosition(nextGridPoint) == null || GridManager.GetGridObjectAtPosition(nextGridPoint)) {
                    break;
                } else {
                    locations.Add(nextGridPoint);
                }
            }
        }

        // Attack
        if (m_Attack != null) {
            var tempPositions = m_Attack.GetAllTargetPositions();
            foreach(Vector2Int pos in tempPositions) {
                if (GridManager.GetGridTileAtPosition(pos)) {
                    if (GridManager.GetGridObjectAtPosition(pos)) {
                        if (!locations.Contains(pos)) {
                            locations.Add(pos);
                        }
                    }
                }
            }
        }

        return locations;
    }
}
