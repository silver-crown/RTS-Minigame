using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Progress.InputSystem
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField]
        private List<MenuState> _menus;

        [SerializeField]
        public MenuState currentMenu;

        protected override void OnAwake()
        {
            if(_menus == null)
                _menus = new List<MenuState>();
        }

        protected override void OnShutdown()
        {
            
        }

        public T GetMenuState<T>(bool errorIfNull = true)
            where T : MenuState
        {
            var type = typeof(T);
            T menuOfType = null;

            foreach(var menu in _menus)
            {
                if(type == menu.GetType())
                {
                    menuOfType = (T)menu;
                }
            }

            if(errorIfNull && menuOfType == null)
            {
                Debug.LogError("There were no menus of type " + type.Name, this);
            }

            return menuOfType;
        }

        public bool TryGetMenuState<T>(out T menu, bool errorIfNull = false)
    where T : MenuState
        {
            menu = GetMenuState<T>();

            return (menu != null);
        }

        /// <summary>
        /// Adds the param menu to MenuManager's list of menus
        /// </summary>
        /// <param name="menu"></param>
        public void RegisterMenu(MenuState menu)
        {
            if(_menus.Contains(menu))
            {
                Debug.LogError("Menu is already registered: " + menu.GetType().Name, menu);
                return;
            }

            _menus.Add(menu);

            if (currentMenu == null)
                currentMenu = menu;
        }

        /// <summary>
        /// Removes the param menu from MenuManager's list of menus
        /// </summary>
        /// <param name="menu"></param>
        public void RemoveMenu(MenuState menu)
        {
            if (_menus.Contains(menu))
            {
                _menus.Remove(menu);
            }
            else
            {
                Debug.LogError("Cannot remove menu: menu was not on the list of menus: " + menu.GetType().Name, menu);
            }
        }
    }
}
