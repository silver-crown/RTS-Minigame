using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Progress.InputSystem
{
    public class SymbolsController : MonoBehaviour
    {
        [SerializeField]
        private string _configName;

        [SerializeField]
        private SymbolsUI _symbolsUI;

        [SerializeField]
        public GameObject SymbolsPrefab;

        [SerializeField]
        private GameObject _symbolsPrefabInstance;

        [SerializeField]
        private SymbolsOverlay _symbolsOverlay;

        [SerializeField]
        private Collider unitCollider;

        // Start is called before the first frame update
        void Start()
        {
            if (_configName != null || _symbolsOverlay != null)
            {
                _symbolsOverlay.AddToSymbolConfig(_configName, this);
            }
            else
            {
                Debug.LogError("Either symbol config or symbol overlay could not be found");
            }
        }

        private void OnDestroy()
        {
            //TODO - deregisteration
            /*
            var overlay = GetOverlay();
            if (overlay != null)
                overlay.RemoveUI(this);
                */
        }

        public SymbolsUI GetUI()
        {
            return _symbolsUI;
        }

        public virtual SymbolsUI InstantiateUI(Transform parent)
        {
            if (_symbolsUI != null)
            {
                return _symbolsUI;
            }

            _symbolsPrefabInstance = Instantiate(SymbolsPrefab, parent);
            _symbolsUI = _symbolsPrefabInstance.GetComponent<SymbolsUI>();
            _symbolsUI.SetController(this);

            return _symbolsUI;
        }

        /// <summary>
        /// Changes symbol to the passed in image.
        /// </summary>
        /// <param name="image"></param>
        public void ChangeSymbol(Image image)
        {
            if (image != null)
            {
                _symbolsUI.ChangeUI(image);
            }
        }

        public Bounds GetBounds()
        {
            return unitCollider.bounds;
        }
    }
}
