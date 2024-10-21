using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlacableObjectManager : MonoBehaviour
{
    public Transform parentToSpawn;
    
    protected List<Product> products = new List<Product>();
    
    public virtual bool AddProduct(Product productToSpawn)
    {
        Renderer spawnZoneRenderer = GetComponent<Renderer>();
        Renderer objectToSpawnRenderer = productToSpawn.GetComponent<Renderer>();
        float angle = transform.rotation.eulerAngles.y;
        float sizeAvailablePerRow= spawnZoneRenderer.bounds.size.z;
        float sizeAvailablePerColumn= spawnZoneRenderer.bounds.size.x;
        if (angle == 0 || angle == 180 || angle == 360)
        {
            sizeAvailablePerRow = spawnZoneRenderer.bounds.size.x;
            sizeAvailablePerColumn = spawnZoneRenderer.bounds.size.z;
        }
        float sizeX = objectToSpawnRenderer.bounds.extents.x;
        float sizeY = objectToSpawnRenderer.bounds.extents.y;
        float sizeZ = objectToSpawnRenderer.bounds.extents.z;

        int productsPerRow = Mathf.FloorToInt(sizeAvailablePerRow / (sizeX*2));
        int productsPerColumn = Mathf.FloorToInt(sizeAvailablePerColumn / (sizeZ * 2));
        if (products.Count<productsPerRow*productsPerColumn && objectToSpawnRenderer.bounds.size.y<spawnZoneRenderer.bounds.size.y)
        {
            
            Vector3 spawnPosition;
            if (products.Count == 0) {
                switch (angle)
                {
                    case 0:
                        spawnPosition = transform.position - new Vector3(spawnZoneRenderer.bounds.extents.x, spawnZoneRenderer.bounds.extents.y, -spawnZoneRenderer.bounds.extents.z)+ new Vector3(sizeX , sizeY/2, -sizeZ/2);
                        break;
                    case 90:
                        spawnPosition = transform.position - new Vector3(-spawnZoneRenderer.bounds.extents.x, spawnZoneRenderer.bounds.extents.y, -spawnZoneRenderer.bounds.extents.z) + new Vector3(-sizeZ / 2, sizeY/2, -sizeX);
                        break;
                    case 180:
                        spawnPosition = transform.position - new Vector3(-spawnZoneRenderer.bounds.extents.x, spawnZoneRenderer.bounds.extents.y, spawnZoneRenderer.bounds.extents.z) + new Vector3(-sizeX, sizeY /2, sizeZ/2);
                        break;
                    case 270:
                        spawnPosition = transform.position - new Vector3(spawnZoneRenderer.bounds.extents.x, spawnZoneRenderer.bounds.extents.y, spawnZoneRenderer.bounds.extents.z) + new Vector3(sizeZ/2, sizeY/2, sizeX);
                        break;
                    default:
                        spawnPosition = transform.position - new Vector3(spawnZoneRenderer.bounds.extents.x, spawnZoneRenderer.bounds.extents.y, -spawnZoneRenderer.bounds.extents.z) + new Vector3(sizeX, sizeY / 2, -sizeZ/2);
                        break;
                }
            }
            else if (products.Count % productsPerRow==0)
            {
                switch (angle)
                {
                    case 0:
                        spawnPosition = products[products.Count - productsPerRow].transform.position - new Vector3(0, 0, sizeZ*2);
                        print(products[products.Count - productsPerRow].transform.position);
                        break;
                    case 90:
                        spawnPosition = products[products.Count - productsPerRow].transform.position - new Vector3(sizeZ*2, 0, 0);
                        break;
                    case 180:
                        spawnPosition = products[products.Count - productsPerRow].transform.position - new Vector3(0, 0, -sizeZ*2);
                        break;
                    case 270:
                        spawnPosition = products[products.Count - productsPerRow].transform.position - new Vector3(-sizeZ*2, 0, 0);
                        break;
                    default:
                        spawnPosition = products[products.Count - productsPerRow].transform.position - new Vector3(0, 0, sizeZ*2);
                        break;
                    }

                }
            else
            {
                    switch (angle)
                {
                    case 0:
                        spawnPosition = products[products.Count - 1].transform.position + new Vector3(sizeX * 2, 0, 0);
                        break;
                    case 90:
                        spawnPosition = products[products.Count - 1].transform.position + new Vector3(0, 0, -sizeX*2);
                        break;
                    case 180:
                        spawnPosition = products[products.Count - 1].transform.position + new Vector3(-sizeX * 2, 0, 0);
                        break;
                    case 270:
                        spawnPosition = products[products.Count - 1].transform.position + new Vector3(0, 0, sizeX * 2);
                        break;
                    default:
                        spawnPosition = products[products.Count - 1].transform.position + new Vector3(sizeX * 2, 0 , 0);
                        break;
                }
            }
            Product instantiateObject = Instantiate(productToSpawn, spawnPosition, Quaternion.Euler(65,55,88), parentToSpawn);
            instantiateObject.GetComponent<Rigidbody>().isKinematic = true;
            products.Add(instantiateObject);
            return true;
        }
        Debug.Log("Full");
        return false;
       
    }
    public Product GetLastProduct()
    {
        if (products.Count<=0) return null;
        return products[products.Count-1];
    }
    public bool IsSameProduct(Product product)
    {
        if (product == null) return false;
        if (products.Count == 0) return false;
        if (products[0].nameProduct!=product.nameProduct) return false;
        return true;
    }
    //return number of products has removed
    public virtual int RemoveProduct(int count)
    {
        if (products.Count <= 0 || count <= 0) return 0;
        int countRemoved = 0;
        for (int i = 0; i < count; i++)
        {
            if (products.Count - 1 < 0) break;
            Product product = products[products.Count - 1];
            if (products.Remove(product))
            {
                Destroy(product.gameObject);
                countRemoved++;
            }
        }
        return countRemoved;
    }
}
