using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Progress.InputSystem
{
    public class BarContainer : MonoBehaviour
    {
        [SerializeField]
        private List<BarController> _bars = new List<BarController>();

        private void Reset()
        {
            _bars = GetComponents<BarController>().ToList();
        }

        public TBar GetController<TBar>()
            where TBar : BarController
        {
            var bar = _bars.Find(b => b.GetType() == typeof(TBar));

            if (bar == null)
                return null;

            return (TBar)bar;
        }
    }
}
