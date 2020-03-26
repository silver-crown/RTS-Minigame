using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : ChessPiece {

    protected bool _hasMoved = false;
    protected GridMovement _gridMovement;

    protected override void Awake() {
        base.Awake();
        _gridMovement = GetComponent<GridMovement>();
    }

    protected virtual void OnEnable() {
        _gridMovement.OnMovementEnd += Moved;
    }

    protected override void OnDisable() {
        base.OnDisable();
        _gridMovement.OnMovementEnd -= Moved;
    }

    // Callback for the movement ended on GridMovement, used to execute queued input
    protected virtual void Moved(GridMovement movement, GridTile fromGridPos, GridTile toGridPos) {
        _hasMoved = true;
    }

    public override List<Vector2Int> MoveLocations(Vector2Int gridPosition) {
        var locations = new List<Vector2Int>();
        
        // 1 square forward
        int forwardDirection = ChessGameManager.Instance._currentPlayer.forward;
        Vector2Int forward = new Vector2Int(gridPosition.x, gridPosition.y + forwardDirection);
        if (GridManager.GetGridTileAtPosition(forward))
            if (!GridManager.GetGridObjectAtPosition(forward)) {
                locations.Add(forward);
            }

        // 2 squares forward
        if (!_hasMoved) {
            int forward2Direction = ChessGameManager.Instance._currentPlayer.forward * 2;
            Vector2Int forward2 = new Vector2Int(gridPosition.x, gridPosition.y + forward2Direction);
            if (GridManager.GetGridTileAtPosition(forward2))
                if (!GridManager.GetGridObjectAtPosition(forward2)) {
                    locations.Add(forward2);
                }
        }

        // Attack
        if (m_Attack != null) {
            var tempPositions = m_Attack.GetAllTargetPositions();
            foreach(Vector2Int pos in tempPositions) {
                if (GridManager.GetGridObjectAtPosition(pos)) {
                   locations.Add(pos);
                }
            }
        }

        return locations;
    }
}
