using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public static ProductManager Instance { get; private set; }
    public List<Product> products = new List<Product>() ;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        foreach(var item in products)
        {
            print(item.nameProduct);
        }  
    }
    public List<Product> GetProducts()
    {
        return products;
    }
    public Product FindProductByName(string name)
    {
        return products.Find(product => product.nameProduct == name);
    }
    public int FindIndProductByName(string name)
    {
        return products.FindIndex(product => product.nameProduct == name);
    }
    public void ReplaceProduit(Product product)
    {
        if (product == null) return;
        int ind=FindIndProductByName(product.nameProduct);
        if (ind < 0) return; 
        products[ind].price=product.price;
        products[ind].purchasePrice=product.purchasePrice;
    }
}
