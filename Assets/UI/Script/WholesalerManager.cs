using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WholesalerManager : Menu
{
    [SerializeField] public VisualTreeAsset productTemplate;
    [SerializeField] public BoxGrabbable boxToSpawn;
    private VisualElement ui;
    private VisualElement allProductsView;
    private VisualElement myProductsView;
    private TextField searchBar;
    private Label nameWholesaler;
    private Button clearBtn;
    private Button checkOutBtn;
    private List<(Product product, VisualElement productView)> productList = new List<(Product, VisualElement)>();
    private List<(Product product, VisualElement productView)> myProductList = new List<(Product, VisualElement)>();

    private void OnEnable()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        allProductsView = ui.Q<ScrollView>("ProductsList");
        myProductsView = ui.Q<ScrollView>("MyProductsList");
        searchBar = ui.Q<TextField>("ResearchBarInput");
        nameWholesaler = ui.Q<Label>("NameWholesalerLbl");
        clearBtn = ui.Q<Button>("ClearBtn");
        checkOutBtn = ui.Q<Button>("PurchaseBtn");
        InitializeData();
    }
    private void InitializeData()
    {
        var test=ProductManager.Instance;
        foreach (Product product in ProductManager.Instance.GetProducts())
        {
            VisualElement productView = CreateProductTemplate(product, false);
            allProductsView.Add(productView);
            productList.Add((product, productView));
        }
        clearBtn.RegisterCallback<ClickEvent>(evt =>
        {
            searchBar.value = "";
        });
        checkOutBtn.RegisterCallback<ClickEvent>(evt =>
        {
            CheckOut();
        });

        searchBar.RegisterValueChangedCallback(evt =>
        {
            string searchValue = evt.newValue.ToLower();

            foreach (var p in productList)
            {
                p.productView.style.display = p.product.nameProduct.ToLower().StartsWith(searchValue) ? DisplayStyle.Flex : DisplayStyle.None;
            }
        });
    }
    private void AddProduct(Product product, IntegerField quantityInput)
    {
        int quantity = quantityInput.value;
        if (quantity <= 0) return;
        var pView = myProductList.Find(p => p.product.nameProduct == product.nameProduct);
        if (pView.product)
        {
            pView.productView.Q<IntegerField>("QuantityInput").value += quantity;
            pView.productView.Q<IntegerField>("QuantityInput").value = pView.productView.Q<IntegerField>("QuantityInput").value;
        }
        else
        {
            VisualElement productView = CreateProductTemplate(product, true);
            myProductsView.Add(productView);
            myProductList.Add((product, productView));
            productView.Q<IntegerField>("QuantityInput").value = quantity;
        }
    }
    private VisualElement CreateProductTemplate(Product product, bool myProduit)
    {
        VisualElement productView = productTemplate.CloneTree();
        if (myProduit)
        {

            productView.AddToClassList("MyProductsList-items");
        }
        else
        {
            productView.AddToClassList("ProductsList-items");
        }

        allProductsView.Q<VisualElement>("unity-content-container").AddToClassList("ProductsList");
        allProductsView.Q<VisualElement>("unity-content-container").style.justifyContent = Justify.FlexStart;
        productView.Q<Label>("NameLbl").text = product.nameProduct;
        productView.Q<Label>("PriceLbl").text = product.purchasePrice.ToString() + "$";
        IntegerField input = productView.Q<IntegerField>("QuantityInput");
        input.value = 1;
        productView.Q<Button>("AddBtn").RegisterCallback<ClickEvent>(evt => { AddProduct(product, input); });
        return productView;
    }
    private void CheckOut()
    {
        foreach (var product in myProductList) {
            int quantity=product.productView.Q<IntegerField>("QuantityInput").value;
            if (quantity > 0)
            {
                for (int i = 1; i <= quantity; i++) { 
                    BoxGrabbable box= Instantiate(boxToSpawn);
                    box.AddProduct(product.product,10);
                }
            }
        }
    }

    public override void ShowMenu(GameObject dataObj)
    {
        Wholesaler whole=dataObj.GetComponent<Wholesaler>();
        if (whole == null) return;
        nameWholesaler.text = whole.wholersalerName;
    }
    public override bool HideMenu()
    {
        return true;
    }
}

