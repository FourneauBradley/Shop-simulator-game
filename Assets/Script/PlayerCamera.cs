using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance { get; private set; }
    public float sensX;
    public float sensY;
    public Transform orientation;
    float xRotation;
    float yRotation;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     setCursor(true);   
    }
    public void setCursor(bool isActif)
    {
        if (isActif) { 
            Cursor.lockState = CursorLockMode.Locked;
            enabled = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            enabled = false;
        }
        Cursor.visible = !isActif;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX=Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY=Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        xRotation -=mouseY;
        yRotation += mouseX;
        xRotation=Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); ;
    }
}
