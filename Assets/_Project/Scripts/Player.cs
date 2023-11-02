using System;
using UnityEngine;
using static GameManager.Tags;

public enum PlayerState
{
    None,
    TowerPlacement,
}

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float minZoom = 1f;

    private Camera _camera;
    [SerializeField] private PlayerState playerState = PlayerState.TowerPlacement;
    
    private Tower _selectedTower;

    private void Awake()
    {
        _camera = Camera.main ? Camera.main : GetComponentInChildren<Camera>();
        _selectedTower = GameManager.Instance.towers[0];
    }
    
    private void Update()
    {
        MoveCamera();
        ZoomCamera();

        switch (playerState)
        {
            case PlayerState.None:
                break;
            case PlayerState.TowerPlacement:
                PlaceTowers();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void PlaceTowers()
    {
        if (!InputManager.Instance.PlaceTower.triggered) return;
        if (LevelManager.Instance.TryPlaceTower(_selectedTower, MousePositionOnGround()))
        {
            //playerState = PlayerState.None;
        }
    }
    
    private Vector3 MousePositionOnGround()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LevelManager.Instance.proTowerPlacementLayerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
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
