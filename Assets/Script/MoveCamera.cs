using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    public Transform camerePos;
    
    
    private void Start()
    {
        transform.rotation = camerePos.rotation;
    }

    
    void Update()
    {
        transform.position = camerePos.position;
    }
}
