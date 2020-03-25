using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.UI
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField]
        private BaseCanvas _baseCanvas;

        private void Reset()
        {
            _baseCanvas = GetComponent<BaseCanvas>();
        }
    }
}
