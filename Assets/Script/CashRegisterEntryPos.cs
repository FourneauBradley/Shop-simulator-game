using UnityEngine;

public class CashRegisterEntryPos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "customer")
        {
            print("customer");
            CustomerScript customer=other.GetComponent<CustomerScript>();
            GetComponentInParent<CashRegisterManager>().CanQueueUp(customer);

        }
    }
}
