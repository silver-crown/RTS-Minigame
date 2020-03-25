using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeVisualizer : MonoBehaviour
{   
    [Header ("Settings")]
    public bool m_Activated = false;
    public int m_RangeReach = 4;
    public GameObject m_NodePrefab;
    public Vector3 m_NodeWorldPositionOffSet;
    public float m_UpdateInterval = 0.5f;

    protected float _intervalTimeLeft ;
    protected GridObject _gridObject;
    protected GridMovement _gridMovement;
    protected bool _triggerOnMovementEnd;
    protected Transform _visualPathHolder;
    protected List<GridTile> _currentRange;
    protected string _holderName = "Visual Path Holder";
    protected Health _ownerHealth;

    private void OnDisable() {
        if (_gridMovement != null) {
            _gridMovement.OnMovementEnd -= MovementEnded;
        }
    }

    private void Awake() {
        _gridObject = GetComponent<GridObject>();
        _gridMovement = GetComponent<GridMovement>();
        if (_gridMovement != null) {
            _gridMovement.OnMovementEnd += MovementEnded;
            _triggerOnMovementEnd = true;
        }
        _intervalTimeLeft = m_UpdateInterval;
    }

    private void Start() {
        if (_triggerOnMovementEnd && m_Activated) {
            CalculateNewRange();
        }
    }

    private void Update() {
        if (m_Activated) {
            if (_triggerOnMovementEnd) {
                return;
            }

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

            CalculateNewRange();
        } else {
            ClearVisualPath();
        }
    }

    private void CalculateNewRange() {
        var newRange = RangeAlgorithms.SearchByGridPosition(_gridObject.m_CurrentGridTile, m_RangeReach);
        if (newRange != _currentRange) {
            ClearVisualPath();
            _currentRange = newRange;
            PopulateVisualRange();
        }
    }

    private void PopulateVisualRange() {
        if (_visualPathHolder == null) {
            _visualPathHolder = new GameObject(_holderName).transform;
        }

        for (int i = 0; i < _currentRange.Count; i++) {
            var newNode = Instantiate(m_NodePrefab, _currentRange[i].m_WorldPosition + m_NodeWorldPositionOffSet, Quaternion.identity).transform;
            newNode.parent = _visualPathHolder;
        }
    }

    private void ClearVisualPath() {
        if (_visualPathHolder != null) {
            DestroyImmediate(_visualPathHolder.gameObject);
            _currentRange.Clear();
        }
    }

    protected void OnDestroy() {
        ClearVisualPath();
    }

    // Callback for the movement ended on GridMovement, used to execute queued input
    protected virtual void MovementEnded(GridMovement movement, GridTile fromGridPos, GridTile toGridPos) {
        if (m_Activated) {
            CalculateNewRange();
        }
    }
}