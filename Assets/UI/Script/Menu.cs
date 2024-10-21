using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Menu : MonoBehaviour
{
    private void Start()
    {
        GetComponent<UIDocument>().gameObject.SetActive(false);

    }
    public virtual void ShowMenu(GameObject dataObj)
    {

    }
    public virtual bool HideMenu()
    {
        return true;
    }
}