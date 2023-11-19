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
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float zoomSpeed = .1f;
    [SerializeField] private PlayerState playerState = PlayerState.None;
    [SerializeField] private HUD hud;

    private float _maxZoom;
    private Camera _camera;
    
    private Tower _selectedTower;


    private void Awake()
    {
        _camera = Camera.main ? Camera.main : GetComponentInChildren<Camera>();
        _selectedTower = GameManager.Instance.towers[0];
        _maxZoom = _camera.orthographicSize;
        hud ??= GetComponentInChildren<HUD>();
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
        if (InputManager.Instance.PlaceTower.triggered)
        {
            if (GameManager.CursorAboveUI) return;
            if (LevelManager.Instance.TryPlaceTower(_selectedTower, MousePositionOnGround()))
            {
                SetPlayerState(PlayerState.None);
            }
        }
        
        if (InputManager.Instance.CancelAction.triggered)
        {
            SetPlayerState(PlayerState.None);
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
        float zoom = InputManager.Instance.Zoom.ReadValue<Vector2>().y;
        zoom *= zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - zoom, minZoom, _maxZoom);
    }
    
    public void SetPlayerState(PlayerState state)
    {
        playerState = state;
        
        switch (playerState)
        {
            case PlayerState.None:
                hud.SwitchOffAllToggles();
                break;
            case PlayerState.TowerPlacement:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
