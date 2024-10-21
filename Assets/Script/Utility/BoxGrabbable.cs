using TMPro;
using UnityEngine;

public class BoxGrabbable : UtilityGrabbable
{
    private Product product;
    private int productNumber;
    public LayerMask maskToDetect;
    public TextMeshPro text;


    BoxGrabbable(Product product, int productNumber)
    {
        this.product = product;
        this.productNumber = productNumber;
        UpdateText();
    }
    public void AddProduct(Product p, int number)
    {
        if (p == null || number <= 0) return; ;
        if (product == null || productNumber <= 0)
        {
            product = p;
            productNumber += number;
            UpdateText();
        }
    }
    private PlacableObjectManager Check()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, PlayerController.distanceToDetect, maskToDetect))
        {
            PlacableObjectManager shelf = hit.transform.GetComponent<PlacableObjectManager>();
            if (shelf != null)
            {
                return shelf;

            }
        }
        return null;
    }
    public override void UtilityFirstAction()
    {
        if (product != null && productNumber > 0)
        {
            PlacableObjectManager shelf = Check();
            if (shelf == null) return;
            if (shelf.AddProduct(product))
            {
                productNumber--;
                UpdateText();
            }
        }
    }
    public override void UtilitySecondAction()
    {
        PlacableObjectManager shelf = Check();
        if (shelf == null) return;
        if (productNumber <= 0)
        {
            Product p = ProductManager.Instance.FindProductByName(shelf.GetLastProduct().nameProduct);
            if (p)
            {
                product = p;
                productNumber = 1;
                shelf.RemoveProduct(1);
            }
        }
        else
        {
            if (shelf.IsSameProduct(product))
            {
                if (shelf.RemoveProduct(1)==1)
                {
                    productNumber++;
                }
            }
        }
        UpdateText();


    }
    private void UpdateText()
    {
        text.text = product.nameProduct + " " + productNumber;
    }
}
