using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.InputSystem
{
    public class Unit : Selectable
    {
        [SerializeField]
        private BarContainer _bars;

        [SerializeField]
        private SymbolsContainer _symbols;
        private bool _selected = false;
        private GridPathfinder pathfinder;
        private GridObject gridObject;

        public BarContainer Bars { get { return _bars; } }
        public SymbolsContainer Symbols { get { return _symbols; } }

        private void Awake()
        {
            HealthController health = _bars.GetController<HealthController>();
            ArmourController armour = _bars.GetController<ArmourController>();

            if (health != null)
            {
                health.Stat = health.MaxValue;
            }

            if (armour != null)
            {
                armour.Stat = armour.MaxValue;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            MenuManager.Instance.GetMenuState<UnitSelectionState>().AddUnitToList(this);
            gridObject = GetComponent<GridObject>();
            if (gridObject != null)
            {
                gridObject.Initialize();
            }
            pathfinder = GetComponent<GridPathfinder>();
        }

        private void Update()
        {
            if (!_selected || pathfinder.m_DrawPath == false || pathfinder.m_DestinationTile != null)
                return;

            GridTile hoveredTile = GridManager.Instance.m_HoveredGridTile;

            if (hoveredTile == null)
                return;

            pathfinder.DeterminePath(gridObject.m_CurrentGridTile, hoveredTile);
        }

        public void SetSelected(bool selected)
        {
            Behaviour halo = (Behaviour)GetComponent("Halo");
            halo.enabled = selected;
            _selected = selected;
        }
    }
}
