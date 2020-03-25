using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.InputSystem
{
    public abstract class MenuState : MonoBehaviour
    {
        public static event System.Action<MenuState> OnClosing = delegate { };
        public static event System.Action<MenuState> OnClose = delegate { };

        private void Awake()
        {
            MenuManager.Instance.RegisterMenu(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
