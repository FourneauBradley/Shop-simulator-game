using UnityEngine;
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Collider))]

public class UtilityGrabbable : MonoBehaviour
{
    private Rigidbody rb;
    private Collider coll;
    private Transform objectGrabPoint;
    private void Awake()
    {
        this.rb = GetComponent<Rigidbody> ();
        this.coll = GetComponent<Collider> ();
    }
    public void Grab(Transform objectGrabPoint)
    {
        this.objectGrabPoint=objectGrabPoint;
        rb.useGravity =false ;
        coll.enabled = false;
    }
    public void Drop()
    {
        objectGrabPoint = null;
        rb.useGravity = true;
        coll.enabled = true;
    }
    private void FixedUpdate()
    {
        if (objectGrabPoint != null) {
            transform.rotation = objectGrabPoint.rotation;

            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPoint.position, Time.deltaTime * 20f);
            rb.MovePosition(newPosition);
        }
    }
    public virtual void UtilityFirstAction()
    {
        //a def
        

    }
    public virtual void UtilitySecondAction()
    {
        //a def


    }
}
