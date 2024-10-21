using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    [SerializeField] public List<MenuEntry> menus = new List<MenuEntry>();
    public void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public MenuEntry GetMenu(MenuEnum menu)
    {
        return menus.FirstOrDefault(m => m.menuEnum == menu);
    }
    public void ShowMenu(MenuEnum menuEnum, GameObject dataObj)
    {
        var menu = GetMenu(menuEnum);
        menu.menu.ShowMenu(dataObj);
        menu.menu.gameObject.SetActive(true);
        PlayerMovement.Instance.setControll(false);
        PlayerCamera.Instance.setCursor(false);
    }
    public void HideMenu(MenuEnum menuEnum)
    {
        var menu = GetMenu(menuEnum);
        menu.menu.HideMenu();
        menu.menu.gameObject.SetActive(false);
        PlayerMovement.Instance.setControll(true);
        PlayerCamera.Instance.setCursor(true);
    }
    public void HideAllMenu()
    {
        bool isHide=false;
        foreach(MenuEntry menuEntry in menus)
        {
            if (IsVisible(menuEntry.menuEnum))
            {
                if (menuEntry.menu.HideMenu())
                {
                    isHide = true;
                    menuEntry.menu.gameObject.SetActive(false);
                }
            }
        }
        if (isHide) { 
        
            PlayerMovement.Instance.setControll(true);
            PlayerCamera.Instance.setCursor(true);
        }
    }
    public bool IsVisible(MenuEnum menuEnum)
    {
        var menu = GetMenu(menuEnum);
        return menu.menu.gameObject.activeInHierarchy ? true : false;
    }

}
[System.Serializable]
public class MenuEntry
{
    public Menu menu;
    public MenuEnum menuEnum;
}