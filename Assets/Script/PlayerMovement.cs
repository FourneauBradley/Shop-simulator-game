using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grouned;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

    }
    public void setControll(bool value)
    {
        enabled = value;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
       
    }
    private void FixedUpdate()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.y);
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limetedVed = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limetedVed.x,rb.linearVelocity.y,limetedVed.z);
        }
    }
    void Update()
    {
        grouned = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f,whatIsGround);
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (grouned) {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
        
    }
}
