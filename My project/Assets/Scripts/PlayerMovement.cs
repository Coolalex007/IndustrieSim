using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float mouseSensitivityHor = 2f;
    public float mouseSensitivityVer = 2f;
    public Transform t;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speed = 5.0f;
        transform.position += new Vector3(horizontal,0,vertical) * speed * Time.deltaTime;
        if (!Input.GetMouseButtonDown(1))
        {
            RotateCamera();
        }
        else
        {
            Debug.Log("AAAA");
        }
 
    }

    public void RotateCamera()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivityHor;
        
        float verticalRotation = Input.GetAxis("Mouse Y") * mouseSensitivityVer;

        t.Rotate(verticalRotation * mouseSensitivityVer, horizontalRotation * mouseSensitivityHor, 0);
    }
}
