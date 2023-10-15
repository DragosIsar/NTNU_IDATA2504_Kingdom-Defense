using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;  // Mouse sensitivity
    public float speed = 5.0f;       // Camera movement speed

    private float rotationX = 0;

    void Update()
    {
        // Camera Rotation (Mouse Look)
        float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
        rotationX += Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);

        // Camera Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
