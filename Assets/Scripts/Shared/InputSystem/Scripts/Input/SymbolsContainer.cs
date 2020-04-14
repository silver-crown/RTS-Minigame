using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Progress.InputSystem
{
    [System.Serializable]
    public struct SymbolSlot
    {
        public string ConfigName;
        public SymbolsUI.Direction direction;
        public SymbolsUI ui;

        public SymbolSlot(string paramName, SymbolsUI.Direction paramDirection, SymbolsUI paramUI)
        {
            ConfigName = paramName;
            direction = paramDirection;
            ui = paramUI;
        }
    }

    public class SymbolsContainer : MonoBehaviour
    {
        [SerializeField]
        private List<SymbolSlot> _symbolSlots = new List<SymbolSlot>();
        [SerializeField]
        private SymbolsOverlay _symbolsOverlay;
        [SerializeField]
        private Collider unitCollider;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < _symbolSlots.Count; i++)
            {
                if (_symbolSlots[i].ConfigName != null && _symbolsOverlay != null)
                {
                    GameObject symbolObject = _symbolsOverlay.AddToSymbolConfig(_symbolSlots[i].ConfigName);
                    if (symbolObject != null)
                    {
                        SymbolsUI symbol = symbolObject.GetComponent<SymbolsUI>();
                        if (symbol != null)
                        {
                            symbol.SetContainer(this);
                            SymbolSlot slot = new SymbolSlot(_symbolSlots[i].ConfigName, _symbolSlots[i].direction, symbol);
                            _symbolSlots[i] = slot;
                        }
                        else
                            Debug.LogError("Symbol prefab was missing SymbolsUI component when returned to the SymbolsContainer");
                    }
                }
                else
                {
                    Debug.LogError("Either symbol config or symbol overlay could not be found");
                }
            }
        }

        public Bounds GetBounds()
        {
            return unitCollider.bounds;
        }

        public SymbolsUI.Direction GetDirection(SymbolsUI paramUI)
        {
            SymbolSlot slot = _symbolSlots.Find(x => x.ui.Equals(paramUI));
            if (slot.Equals(null))
            {
                Debug.LogError("No slot was found for this UI, returning default direction right");
                return SymbolsUI.Direction.Right;
            }
            return slot.direction;
        }

        //private void Reset()
        //{
        //_symbols = GetComponents<SymbolsController>().ToList();
        //}

        //public Symbols GetController<Symbols>()
        //    where Symbols : SymbolsController
        //{
        //    var symbol = _symbols.Find(b => b.GetType() == typeof(Symbols));

        //    if (symbol == null)
        //        return null;

        //    return (Symbols)symbol;
        //}
    }
}
