using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.InputSystem
{
    public class ObsoleteSymbolsController : MonoBehaviour
    {
        private Dictionary<SymbolsController, SymbolsUI> _symbolsBars = new Dictionary<SymbolsController, SymbolsUI>();

        private void Awake()
        {
            //SymbolsController.OnSymbolsAdded += AddSymbolsBar;
            //SymbolsController.OnSymbolsRemoved += RemoveSymbolsBar;
        }

        private void OnDestroy()
        {
            //SymbolsController.OnSymbolsAdded -= AddSymbolsBar;
            //SymbolsController.OnSymbolsRemoved -= RemoveSymbolsBar;
        }

        /// <summary>
        /// Creates a symbol bar and assigns it to the passed in Symbols object.
        /// </summary>
        /// <param name="symbols"></param>
        private void AddSymbolsBar(SymbolsController symbols)
        {
            if (_symbolsBars.ContainsKey(symbols) == false)
            {
                var symbolsBar = Instantiate(symbols.SymbolsPrefab, transform);
                var ui = symbolsBar.GetComponent<SymbolsUI>();
                _symbolsBars.Add(symbols, ui);
                ui.SetController(symbols);
            }
        }

        /// <summary>
        /// Removes and destroys the symbol bar associated with the passed in Symbols object, if one exists.
        /// </summary>
        /// <param name="symbols"></param>
        private void RemoveSymbolsBar(SymbolsController symbols)
        {
            if (_symbolsBars.ContainsKey(symbols))
            {
                Destroy(_symbolsBars[symbols].gameObject);
                _symbolsBars.Remove(symbols);
            }
        }
    }
}
