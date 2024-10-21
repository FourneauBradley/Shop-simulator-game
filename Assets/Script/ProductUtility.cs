using UnityEngine;

public class ProductUtility : MonoBehaviour
{
    public CashRegisterManager cashRegisterManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray,out RaycastHit hit, 2f,LayerMask.GetMask("Item")))
        {
            if (hit.collider.gameObject == this.gameObject) {
                cashRegisterManager.ScanProduct(this);
            }
        }
    }
}
