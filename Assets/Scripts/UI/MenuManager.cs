using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JobApplicationGame
{

    /// <summary>
    /// A struct that acts like a hack to show some sort of dictionay on unity editor.
    /// </summary>
    [System.Serializable]
    public struct MenuInfo
    {
        public string nombre;
        public MenuState menu;
    };

    /// <summary>
    /// Class in charge of managing the menu system
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// The current menu that we are in.
        /// </summary>
        public MenuState CurrentMenu;
        /// <summary>
        /// some sort of dictionary for menus
        /// </summary>
        public MenuInfo[] menusList;

        /// <summary>
        /// The stack with the menus. When we open a menu we stack it here, when we close it we unstack it
        /// </summary>
        private Stack<MenuState> currentMenuTree = new Stack<MenuState>();

        /// <summary>
        /// Lazy singleton
        /// </summary>
        private static MenuManager _instance = null;
        public static MenuManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (MenuManager)FindObjectOfType(typeof(MenuManager));
                return _instance;
            }
        }

        public void Awake()
        {
            if (_instance == null) _instance = this;
        }

        public void Start()
        {
            //if (CurrentMenu != null)
            //      	ShowMenu(CurrentMenu);

            //PushMenu("MainMenu");
        }

        //TODO check this out and maybe remove
        //public void ShowMenu(MenuState menu)
        //{
        //    if (CurrentMenu != null)
        //    {
        //        CurrentMenu.Close();
        //        //CurrentMenu.IsActive = false;
        //    }

        //    CurrentMenu = menu;
        //    CurrentMenu.Open();
        //    //CurrentMenu.IsActive = true;
        //}

        public void CloseCurrent()
        {
            CurrentMenu.Close();
        }

        /// <summary>
        /// Pop last menu added to the stack (when we press on back in some menu for instance)
        /// </summary>
        public void PopMenu()
        {
            if (currentMenuTree.Count > 0)
                currentMenuTree.Pop().Close();
            if (currentMenuTree.Count > 0)
                currentMenuTree.Peek().Open();
        }

        /// <summary>
        /// Push a new menu to the stack.
        /// </summary>
        /// <param name="menuName">The name of the menu to add to the stack</param>
        public void PushMenu(string menuName)
        {
            MenuState menu = FindByName(menuName);

            if (currentMenuTree.Count > 0)
                currentMenuTree.Peek().Close();

            currentMenuTree.Push(menu);
            menu.Open();
        }
        /// <summary>
        /// Empties the menus stack.
        /// </summary>
        public void PopAll()
        {
            for (int i = currentMenuTree.Count; i > 0; i--)
            {
                currentMenuTree.Pop().Close();
            }

            currentMenuTree.Clear();
        }

        /// <summary>
        /// Gets the specific.
        /// </summary>
        /// <param name="name">name of the menu to find</param>
        /// <returns>Menu with the named passed</returns>
        public MenuState FindByName(string name)
        {
            foreach (MenuInfo iMenuInfo in menusList)
            {
                if (iMenuInfo.nombre == name)
                    return iMenuInfo.menu;
            }
            return null;
        }
    }
}