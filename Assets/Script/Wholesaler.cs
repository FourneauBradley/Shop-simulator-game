using UnityEngine;

public class Wholesaler : MonoBehaviour,InteractableBase
{
    public string wholersalerName;
    public MenuEnum menuToShow= MenuEnum.WHOLESALER_MENU_MANAGER;
    public void Action()
    {
        var instance=MenuManager.Instance;
        if (instance.IsVisible(menuToShow))
        {

        }
        else
        {

            instance.ShowMenu(menuToShow,this.gameObject);
        }
    }

}
