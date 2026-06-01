using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float mouseSensitivityHor = 2f;
    public float mouseSensitivityVer = 2f;
    public Transform t;
    public PlayerInteract player;

    public void UpdateMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speed = 5.0f;
        transform.position += new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
    }

    void Update()
    {
        if (!player.canMove || !Input.GetMouseButton(1))
        {
            return;
        }

        RotateCamera();
    }

    public void RotateCamera()
    {
        if (player.clickedObject == null || Camera.main == null)
        {
            return;
        }

        Transform cameraTransform = Camera.main.transform;
        Vector3 target = player.clickedObject.transform.position;
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivityHor;
        float verticalRotation = Input.GetAxis("Mouse Y") * mouseSensitivityVer;

        cameraTransform.RotateAround(target, Vector3.up, horizontalRotation);
        cameraTransform.RotateAround(target, cameraTransform.right, verticalRotation);

        Vector3 targetToCamera = cameraTransform.position - target;
        float verticalAngle = Vector3.Angle(Vector3.up, targetToCamera);
        if (verticalAngle < 5f || verticalAngle > 175f)
        {
            cameraTransform.RotateAround(target, cameraTransform.right, -verticalRotation);
        }

        cameraTransform.LookAt(target);
    }
}
