using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject utilityPoint;
    [SerializeField] public static float distanceToDetect = 3f;
    [SerializeField] public GameObject boxToSpawn;
    private UtilityGrabbable utilityGrabbable;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            MenuManager.Instance.HideAllMenu();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            GameObject obj=Instantiate(boxToSpawn);
            BoxGrabbable box= obj.GetComponent<BoxGrabbable>();
            box.AddProduct(ProductManager.Instance.FindProductByName("Banane"),3);
        }
        if (Input.GetMouseButtonDown(0) && utilityGrabbable!=null)
        {
            utilityGrabbable.UtilityFirstAction();
        }
        if(Input.GetMouseButtonDown(1)  && utilityGrabbable != null)
        {
            utilityGrabbable.UtilitySecondAction();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!utilityGrabbable) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, distanceToDetect))
                {
                   
                    if (hit.transform.TryGetComponent(out utilityGrabbable))
                    {
                        utilityGrabbable.Grab(utilityPoint.transform);
                    }
                }
                if (Physics.Raycast(ray, out hit, PlayerController.distanceToDetect, 8))
                {
                    InteractableBase obj=hit.transform.GetComponent<InteractableBase>();
                    if (obj != null) {
                        obj.Action();
                    }
                }
            }
            else
            {
                utilityGrabbable.Drop();
                utilityGrabbable = null;
            }
            
        }
    }
}
