using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class ShelfPlacable : PlacableObjectManager,InteractableBase
{
    [SerializeField] public GameObject priceLabel;
    public void Action()
    {
        if (products.Count <= 0) return;
        Product p = ProductManager.Instance.FindProductByName(products[0].nameProduct);
        products[0].price = p.price;
        products[0].purchasePrice = p.purchasePrice;
        MenuManager.Instance.ShowMenu(MenuEnum.PRICE_MENU_MANAGER, products[0].gameObject);
    }
    private void Start()
    {

        string[] productNames = { "Banane" ,"Barre"};

        string selectedProductName = productNames[Random.Range(0, productNames.Length)];


        Product selectedProduct = ProductManager.Instance.FindProductByName(selectedProductName);


        int randomQuantity = Random.Range(1, 4);


        for (int i = 0; i < randomQuantity; i++)
        {
            AddProduct(selectedProduct);
        }



    }
    void Update()
    {
        if (priceLabel.activeInHierarchy && products.Count>0)
        {
            print(ProductManager.Instance.FindProductByName(products[0].nameProduct).price.ToString("0.00") + "$");
            var ui = priceLabel.GetComponent<UIDocument>().rootVisualElement;
            ui.Q<Label>("PriceLbl").text = ProductManager.Instance.FindProductByName(products[0].nameProduct).price.ToString("0.00") + "$";
            print(ui.Q<Label>("PriceLbl").text);
        }
    }
    public void ShowPriceLbl()
    {
        priceLabel.SetActive(true);
        var ui = priceLabel.GetComponent<UIDocument>().rootVisualElement;
        ui.Q<Label>("NameLbl").text = products[0].nameProduct;
        ui.Q<Label>("PriceLbl").text = ProductManager.Instance.FindProductByName(products[0].nameProduct).price.ToString("0.00") + "$";
    }
    public void HidePriceLbl()
    {
        priceLabel.SetActive(false);
    }
    public override bool AddProduct(Product productToSpawn)
    {
        if (productToSpawn == null || (products.Count > 0 && !IsSameProduct(productToSpawn))) return false;
        bool isAdd=base.AddProduct(productToSpawn);
        if(isAdd) ShowPriceLbl();
        return isAdd;
    }
    public override int RemoveProduct(int count)
    {
        int nb=base.RemoveProduct(count);
        if (products.Count <= 0)
        {
            HidePriceLbl();
        }
        return nb;
    }

}
