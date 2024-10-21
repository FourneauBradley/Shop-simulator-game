using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CashRegisterManager : MonoBehaviour
{
    public PlacableObjectManager productZoneRenderer;
    private List<CustomerInQueue> customers= new List<CustomerInQueue>();
    private List<Product> products = new List<Product>();
    private float totalCost=0f;
    void Start()
    {
        
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject.CompareTag("QueuePos"))
            {
                customers.Add(new CustomerInQueue(child.transform,null));
            }
        }
    }
    void Update()
    {
        
    }
    private void NextCustomer()
    {
        print("Next");
        CustomerScript customer=customers[0].customer;
        totalCost = 0;
        foreach (ProductWithQuantity productWithQuantity in customer.GetProducts()) {
            print(productWithQuantity.product.nameProduct+" "+productWithQuantity.quantity);
            for (int i = 0; i < productWithQuantity.quantity; i++) {
                Product p = productWithQuantity.product;
                if (!customer.RemoveProduct(p)) continue;
                Renderer rend=productZoneRenderer.GetComponent<Renderer>();
                Renderer rendPro=p.GetComponent<Renderer>();
                Vector3 spawnPosition;
                /*if (products.Count < 0)
                {
                 spawnPosition=productZoneRenderer.transform.position-new Vector3(-rend.bounds.extents.x+rendPro.bounds.extents.x,rend.bounds.extents.y-rendPro.bounds.extents.y, rend.bounds.extents.z-rendPro.bounds.extents.z);
                }
                else
                {

                }
                spawnPosition= productZoneRenderer.transform.position-new Vector3(-rend.bounds.extents.x+rendPro.bounds.extents.x,rend.bounds.extents.y-rendPro.bounds.extents.y, rend.bounds.extents.z-rendPro.bounds.extents.z);
                */
                spawnPosition= new Vector3(
                    Random.Range(rend.bounds.min.x, rend.bounds.max.x),
                    Random.Range(rend.bounds.min.y, rend.bounds.max.y),
                    Random.Range(rend.bounds.min.z, rend.bounds.max.z)
                );
                Product instantiateObject =Instantiate(p,spawnPosition, Quaternion.Euler(p.angleOrientation));
                instantiateObject.GetComponent<Rigidbody>().isKinematic = true;
                var pUtility=instantiateObject.AddComponent<ProductUtility>().cashRegisterManager=this;
                products.Add(instantiateObject);


            }
        }
    }
    public void ScanProduct(ProductUtility productUtility)
    {
        Product p=productUtility.GetComponent<Product>();
        products.Remove(p);
        totalCost += ProductManager.Instance.FindProductByName(p.nameProduct).price;
        Destroy(p.gameObject);
        if (products.Count <= 0) {
            print("donne l'argent " + totalCost);
        }
    }
    public bool CanQueueUp(CustomerScript script)
    {
        CustomerInQueue firstPlace = GetFirstPlace();
        if (firstPlace == null) return false;
        script.SetNextPos(firstPlace.queuePos.gameObject);
        firstPlace.customer=script;
        if(firstPlace.queuePos==customers[0].queuePos) NextCustomer();
        return true;
    }
    public bool HavePlaceInTheQueue()
    {

        CustomerInQueue firstPlace = GetFirstPlace();
        if (firstPlace == null) return false;
        return true;
    }
    public CustomerInQueue GetFirstPlace()
    {
        foreach (CustomerInQueue queue in customers)
        {
            if(queue.customer == null)
            {
                return queue;
            }
        }
        return null;
    }
    public bool AddProductsToCheckout(List<Product> products)
    {
        return true;
    }
}
public class CustomerInQueue
{
    public Transform queuePos;
    public CustomerScript customer;
    public CustomerInQueue(Transform queuePos, CustomerScript customer) {
        this.queuePos = queuePos;
        this.customer = customer;
    }
}
