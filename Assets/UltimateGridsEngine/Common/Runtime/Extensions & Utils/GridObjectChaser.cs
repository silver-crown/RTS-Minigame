using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectChaser : MonoBehaviour {

    [Header ("Sight/Trigger Range")]
    [Range(0, 10)]
	public int m_SightRange = 4;

    [Header ("Target Settings")]
    public GridObject m_TargetToChase;
    //[HideInInspector]
    public GridObject m_CurrentTarget;

    [Header ("Update Interval")]
    public float m_UpdateInterval = 0.3f;

    protected float _intervalTimeLeft ;
    protected GridObject _gridObject;
    protected GridMovement _gridMovement;
    [Header ("Attack")]
    public bool TryToAttackWhenPossible;
    public Attack m_Attack;

	protected virtual void Awake() {
		_gridObject = GetComponent<GridObject>();
        _gridMovement = GetComponent<GridMovement>();
        _intervalTimeLeft = m_UpdateInterval;
	}

    protected virtual void Update() {
        // Interval timer
        if (_intervalTimeLeft > 0f) {
            _intervalTimeLeft = _intervalTimeLeft - Time.deltaTime;
            if (_intervalTimeLeft <= 0f) {
                // Set the timer
                _intervalTimeLeft = m_UpdateInterval;
            } else {
                return;
            }
        }

        GetTargetInRange();
        ChaseTarget();
    }

    public virtual void GetTargetInRange() {
        // Reset the current target
        if (m_CurrentTarget != null) {
            ResetTarget();
        }

        var tilesInRange = RangeAlgorithms.SearchByGridPosition(_gridObject.m_CurrentGridTile, m_SightRange);
        // Chase any target which comes in rage
        if (m_TargetToChase == null) {
            foreach (GridTile tile in tilesInRange) {
                var gridObjectAtTile = GridManager.GetGridObjectAtPosition(tile.m_GridPosition);
                if (gridObjectAtTile != null && gridObjectAtTile != _gridObject) {
                    SetTarget(gridObjectAtTile);
                    break;
                }
            }
        } else { // Chase the chosen target
            foreach (GridTile tile in tilesInRange) {
                var gridObjectAtTile = GridManager.GetGridObjectAtPosition(tile.m_GridPosition);
                if (gridObjectAtTile != null && gridObjectAtTile != _gridObject && gridObjectAtTile == m_TargetToChase) {
                    SetTarget(gridObjectAtTile);
                    break;
                }
            }
        }
	}

    public virtual void SetTarget(GridObject targetGridObject) {
		m_CurrentTarget = targetGridObject;
	}

    public virtual void ResetTarget() {
        m_CurrentTarget = null;
    }

    protected virtual void ChaseTarget() {
        if (m_CurrentTarget == null) {
            GetTargetInRange();
        }

        if (m_CurrentTarget == null) {
            return;
        }

        // Attack
        if (TryToAttackWhenPossible) {
            var targetPosition = m_CurrentTarget.m_GridPosition;
            var gridObjectAtPosition = GridManager.GetGridObjectAtPosition(targetPosition);
            if (gridObjectAtPosition != null && m_Attack != null) {
                var healthComponents = m_Attack.GetVictimsFromTriggerAtPosition(targetPosition);
                if (healthComponents != null && healthComponents.Count > 0) {
                    var attkResult = m_Attack.TryAttack(targetPosition);
                    if (attkResult == AttackResult.Success) {
                        return;
                    }
                }
        }
        }
        
        // Movement
        if (_gridObject.m_GridPosition.GridDistance(m_CurrentTarget.m_GridPosition) > 1) {
            // Calculate a path and try moving towards the target
            var pathToTarget = AStar.Search(_gridObject.m_CurrentGridTile, m_CurrentTarget.m_CurrentGridTile);
            if (pathToTarget != null && pathToTarget.Count > 0) {
                _gridMovement.TryMoveTo(pathToTarget[0]);
            }
        } else { // Rotation
            var targetDirection = m_CurrentTarget.m_GridPosition - _gridObject.m_GridPosition;
            var rotated = _gridMovement.TryRotateTo(targetDirection);
        }
    }
}
