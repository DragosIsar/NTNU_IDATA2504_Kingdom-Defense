using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float speed = 5.0f;
    public bool cameraLookEnabled;

    private float _rotationX;

    private void Update()
    {
        if (cameraLookEnabled)
        {
            // Camera Rotation (Mouse Look)
            float mouseX = InputManager.Instance.LookAction.ReadValue<Vector2>().x;
            float mouseY = InputManager.Instance.LookAction.ReadValue<Vector2>().y;
        
            float rotationY = transform.localEulerAngles.y + mouseX * sensitivity;
            _rotationX += mouseY * sensitivity;
            _rotationX = Mathf.Clamp(_rotationX, -90, 90);
            transform.localEulerAngles = new Vector3(-_rotationX, rotationY, 0);
        }

        // Camera Movement
        float horizontal = InputManager.Instance.MoveAction.ReadValue<Vector2>().x;
        float vertical = InputManager.Instance.MoveAction.ReadValue<Vector2>().y;

        Vector3 moveDirection = new(horizontal, 0, vertical);
        transform.Translate(moveDirection * (speed * Time.deltaTime));
    }
}
