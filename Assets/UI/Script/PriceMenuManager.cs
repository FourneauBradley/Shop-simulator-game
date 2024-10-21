using UnityEngine;
using UnityEngine.UIElements;


public class PriceMenuManager : Menu
{
    private VisualElement ui;

    private Label purchasePrice;
    private Label MarketPrice;
    private FloatField input;
    private Button quitButton;
    private Product product;
    
    private void OnEnable()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        if (product == null) return;
        ui.Q<Label>("Title").text = product.nameProduct;
        purchasePrice = ui.Q<Label>("PurchasePriceValueLbl");
        purchasePrice.text = product.purchasePrice.ToString() + "$";
        MarketPrice = ui.Q<Label>("MarketPriceValueLbl");
        MarketPrice.text = product.marketPrice.ToString() + "$";
        input = ui.Q<FloatField>("CurrentPriceInput");
        input.value = product.price;
        input.RegisterValueChangedCallback(evt => { print(evt.newValue); });
        ui.Q<Label>("ProfitValueLbl").text = (product.price - product.purchasePrice).ToString() + "$";
        //css for negatif profit
        quitButton = ui.Q<Button>("LeaveBtn");
        quitButton.clicked += () => MenuManager.Instance.HideMenu(MenuEnum.PRICE_MENU_MANAGER);

    }
    public override bool HideMenu()
    {
        product.price = input.value;
        ProductManager.Instance.ReplaceProduit(product);
        product = null;
        return true;
    }

    public override void ShowMenu(GameObject dataObj)
    {
        Product p=dataObj.GetComponent<Product>();
        if (p == null) return; ;
        product = p;
       
    }
}
