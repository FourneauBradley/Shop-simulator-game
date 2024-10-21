using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerScript : MonoBehaviour
{
    public GameObject market;
    public GameObject test;
    public int minProduct = 2;
    public int maxProduct = 2;
    public int minQuantityPerProduct = 2;
    public int maxQuantityPerProduct = 6;
    private List<ProductWithQuantity> productsWant = new List<ProductWithQuantity>();
    private List<ProductWithQuantity> myProducts = new List<ProductWithQuantity>();
    private GameObject nextPos;
    private NavMeshAgent agent;
    private Vector3 posStart;
    private bool isQueuingUp = false;

    public bool RemoveProduct(Product p)
    {
        ProductWithQuantity pw = myProducts.Find(pwq => pwq.product.name == p.name);
        if (pw != null)
        {
            pw.quantity--;
            if (pw.quantity <= 0) {
                productsWant.Remove(pw);
            }
            return true;
        }
        return false; 
    }
    public List<ProductWithQuantity> GetProducts()
    {
        foreach (ProductWithQuantity p in myProducts) {
            print(p.product.nameProduct);
        }
        return myProducts;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        posStart = transform.position;
        nextPos = market;
        List<Product> products = ProductManager.Instance.GetProducts();
        int productCount = products.Count;
        int randomProductNumber = Random.Range(minProduct, maxProduct + 1);
        if (randomProductNumber > productCount) randomProductNumber = productCount;
        for (int i = 1; i <= randomProductNumber; i++)
        {
            bool canAdd = true;
            Product p = products[Random.Range(0, productCount)];
            int whileCount = 0;
            while (productsWant.Find(pwq => pwq.product.name == p.name) != null)
            {
                p = products[Random.Range(0, productCount)];
                whileCount++;
                if (whileCount >= 1000)
                {
                    canAdd = false;
                    break;
                }
            }
            if (canAdd)
            {
                int randomQuantity = Random.Range(minQuantityPerProduct, maxQuantityPerProduct + 1);
                ProductWithQuantity productWithQuantity = new ProductWithQuantity(randomQuantity, p);
                productsWant.Add(productWithQuantity);
            }
        }
        foreach (ProductWithQuantity p in productsWant)
        {
           // print(p.product.nameProduct + " " + p.quantity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isQueuingUp)
        {

            if (productsWant.Count > 0)
            {

                if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
                {

                    //agent.destination = nextPos.transform.position;
                }
                else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {

                    OnDestinationReached();
                }
            }
            else
            {

                GameObject[] cashRegisterObjects = GetGameObjectByTag("CashRegister");
                List<(CashRegisterEntryPos,CashRegisterManager)> entryPosList = new List<(CashRegisterEntryPos,CashRegisterManager)>();

                foreach (GameObject obj in cashRegisterObjects)
                {
                    CashRegisterManager cashRegister=obj.GetComponent<CashRegisterManager>();
                    if (cashRegister.HavePlaceInTheQueue())
                    {
                        CashRegisterEntryPos entryPos = cashRegister.GetComponentInChildren<CashRegisterEntryPos>();
                        if (entryPos != null)
                        {
                            entryPosList.Add((entryPos, cashRegister));
                        }
                        else
                        {
                            Debug.LogWarning("No CashRegisterEntryPos found in: " + cashRegister.name);
                        }
                    }
                    
                }
                entryPosList.Sort((a, b) =>
                {
                    float distanceToA = Vector3.Distance(transform.position, a.Item1.transform.position);
                    float distanceToB = Vector3.Distance(transform.position, b.Item1.transform.position);
                    return distanceToA.CompareTo(distanceToB);
                });
                agent.destination = entryPosList[0].Item1.transform.position;
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (entryPosList[0].Item2.CanQueueUp(this))
                    {
                        isQueuingUp = true;
                    }
                }
            }
        }
        else
        {
            agent.destination=nextPos.transform.position;
        }
    }
    /*private void Update()
    {
        if (!agent.hasPath) {
            Collider collider = test.GetComponent<Collider>();
            if (collider == null)
            {
                Debug.LogWarning("No collider found on the GameObject.");
            }

            // Générer un point aléatoire en fonction du collider
            Vector3 randomPoint = new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

            // Vérifier si le point est à l'intérieur du collider
            if (collider.ClosestPoint(randomPoint) == randomPoint)
            {
                agent.destination=randomPoint;
            }
        }
    }*/

    private void OnDestinationReached()
    {
        ProductWithQuantity pCurrent = productsWant[0];
        PlacableObjectManager[] shelf = nextPos.GetComponentsInChildren<ShelfPlacable>();
        if (shelf != null && shelf.Length > 0)
        {
            for (int i = 0; i < shelf.Length && pCurrent.quantity > 0; i++)
            {
                Product shelfProduct = shelf[i].GetLastProduct();
                if (shelfProduct != null && shelfProduct.nameProduct == pCurrent.product.nameProduct)
                {
                    int quantityToRemove=shelf[i].RemoveProduct(pCurrent.quantity);
                    pCurrent.quantity -= quantityToRemove;
                    TakeProduct(pCurrent, quantityToRemove);
                }
            }
            if (pCurrent.quantity <= 0)
            {
                productsWant.RemoveAt(0);
            }
        }
        NextDestination();
    }
    private void TakeProduct(ProductWithQuantity pCurrent , int quantityToAdd)
    {
       ProductWithQuantity p= myProducts.Find(pw=>pw.product.nameProduct==pCurrent.product.nameProduct);
        if (p != null)
        {
            p.quantity += quantityToAdd;
        }
        else
        {
            myProducts.Add(new ProductWithQuantity(quantityToAdd,pCurrent.product));
        }

    }
    private void NextDestination()
    {
        nextPos = null;
        if (productsWant.Count <= 0) return;

        ProductWithQuantity pCurrent = productsWant[0];
        GameObject[] pointObjects = GetGameObjectByTag("shelf");
        bool found = false;

        foreach (GameObject shelfFull in pointObjects)
        {
            if (found) break;

            PlacableObjectManager[] shelf = shelfFull.GetComponentsInChildren<PlacableObjectManager>();
            if (shelf != null && shelf.Length > 0)
            {
                for (int i = 0; i < shelf.Length; i++)
                {
                    Product shelfProduct = shelf[i].GetLastProduct();
                    if (shelfProduct != null && shelfProduct.nameProduct == pCurrent.product.nameProduct)
                    {
                        nextPos = shelfFull;
                        found = true;
                        break;
                    }
                }
            }
        }
        if (nextPos != null)
        {
            agent.destination = nextPos.transform.position;
        }
        if (!found)
        {
            //print("pfff pas de " + pCurrent.product.nameProduct);
            productsWant.RemoveAt(0);
            NextDestination();
        }
    }
    public bool SetNextPos(GameObject obj)
    {
        if (obj != null)
        {
            nextPos = obj;
            return true;
        }
        return false;
    }
    public GameObject[] GetGameObjectByTag(string tag)
    {
        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag(tag);
        System.Array.Sort(pointObjects, (a, b) =>
        {
            float distanceToA = Vector3.Distance(transform.position, a.transform.position);
            float distanceToB = Vector3.Distance(transform.position, b.transform.position);
            return distanceToA.CompareTo(distanceToB);
        });
        return pointObjects;
    }
}
public class ProductWithQuantity
{
    public int quantity;
    public Product product;
    public ProductWithQuantity(int quantity, Product product)
    {
        this.quantity = quantity;
        this.product = product;
    }
}
