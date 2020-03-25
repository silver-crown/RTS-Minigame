using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Progress.UI
{
    public abstract class BaseCanvas : MonoBehaviour
    {
        [SerializeField]
        protected Canvas _canvas;

        [SerializeField]
        protected CanvasGroup _canvasGroup;

        public void Reset()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
        }
    }
}
