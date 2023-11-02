using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float minZoom = 1f;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main ? Camera.main : GetComponentInChildren<Camera>();
    }
    
    private void Update()
    {
        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        // Camera Movement
        float horizontal = InputManager.Instance.MoveAction.ReadValue<Vector2>().x;
        float vertical = InputManager.Instance.MoveAction.ReadValue<Vector2>().y;

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).ToIso();
        moveDirection = transform.TransformDirection(moveDirection);
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
    }

    private void ZoomCamera()
    {
        float zoom = InputManager.Zoom;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - zoom, minZoom, maxZoom);
    }
}
