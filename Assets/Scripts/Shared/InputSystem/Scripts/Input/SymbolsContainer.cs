using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Progress.InputSystem
{
    public class SymbolsContainer : MonoBehaviour
    {
        [SerializeField]
        private List<SymbolsController> _symbols = new List<SymbolsController>();

        private void Reset()
        {
            _symbols = GetComponents<SymbolsController>().ToList();
        }

        public Symbols GetController<Symbols>()
            where Symbols : SymbolsController
        {
            var symbol = _symbols.Find(b => b.GetType() == typeof(Symbols));

            if (symbol == null)
                return null;

            return (Symbols)symbol;
        }
    }
}
