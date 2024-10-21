using UnityEngine;

public class Product : MonoBehaviour
{
    
    public string nameProduct;
   
    public string descriptionProduct;
   
    public float price;
   
    public float purchasePrice;
    
    public float marketPrice;
    
    public Vector3 angleOrientation;
    public Product(string nameProduct, string descriptionProduct, float price, float purchasePrice, float marketPrice)
    {
        this.nameProduct = nameProduct;
        this.descriptionProduct = descriptionProduct;
        this.price = price;
        this.purchasePrice = purchasePrice;
        this.marketPrice = marketPrice;
    }
    public Product(string nameProduct, string descriptionProduct, float price, float purchasePrice, float marketPrice, Vector3 angleOrientation)
    {
        this.nameProduct = nameProduct;
        this.descriptionProduct = descriptionProduct;
        this.price = price;
        this.purchasePrice = purchasePrice;
        this.marketPrice = marketPrice;
        this.angleOrientation = angleOrientation;
    }
    private void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        //print(currentRotation+" "+Quaternion.Euler(currentRotation));
        /*currentRotation.y = angleOrientation.y;  // Par exemple, changer uniquement l'axe Y à 90°
        transform.rotation = Quaternion.Euler(currentRotation);*/

    }
}
