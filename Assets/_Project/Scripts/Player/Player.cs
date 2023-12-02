using System;
using UnityEngine;
using LRS;
using static GameManager.Tags;

public enum PlayerState
{
    None,
    TowerPlacement,
    TowerDetails
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
    
    private Transform _sceneAnchorForMousePosition;

    private void Awake()
    {
        _camera = Camera.main ? Camera.main : GetComponentInChildren<Camera>();
        _maxZoom = _camera.orthographicSize;
        hud ??= GetComponentInChildren<HUD>();
    }
    
    private void Update()
    {
        MoveCamera();
        ZoomCamera();
        CreateAnchorForMousePosition();

        switch (playerState)
        {
            case PlayerState.None:
                SelectTower();
                break;
            case PlayerState.TowerPlacement:
                PlaceTowers();
                break;
            case PlayerState.TowerDetails:
                SelectTower();
                ShowTowerDetails();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        if (InputManager.Instance.PauseAction.triggered)
        {
            hud.TogglePauseMenu();
        }
    }
    
    private void SelectTower()
    {
        if (InputManager.Instance.PlaceTower.triggered)
        {
            if (GameManager.CursorAboveUI) return;
            if (LevelManager.TrySelectTower(MousePositionOnGround(), out Tower tower))
            {
                SetPlayerState(PlayerState.TowerDetails);
                hud.ShowTowerDetails(tower);
            }
        }
    }
    
    private void ShowTowerDetails()
    {
        if (InputManager.Instance.CancelAction.triggered)
        {
            SetPlayerState(PlayerState.None);
        }
    }

    private void PlaceTowers()
    {
        LevelManager.Instance.ShowGhostTower(_sceneAnchorForMousePosition);
        
        if (InputManager.Instance.PlaceTower.triggered)
        {
            if (GameManager.CursorAboveUI) return;
            if (LevelManager.Instance.TryPlaceTower(MousePositionOnGround()))
            {
                SetPlayerState(PlayerState.None);
                LevelManager.Instance.HideGhostTower();
            }
        }
        
        if (InputManager.Instance.CancelAction.triggered)
        {
            SetPlayerState(PlayerState.None);
            LevelManager.Instance.HideGhostTower();
        }
    }
    
    private Vector3 MousePositionOnGround()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) ? hit.point : Vector3.zero;
    }
    
    private void CreateAnchorForMousePosition()
    {
        if (!_sceneAnchorForMousePosition)
        {
            _sceneAnchorForMousePosition = new GameObject("SceneAnchorForMousePosition").transform;
        }
        
        _sceneAnchorForMousePosition.position = MousePositionOnGround();
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
                hud.HideTowerDetails();
                break;
            case PlayerState.TowerPlacement:
                break;
            case PlayerState.TowerDetails:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public void ReloadLevel()
    {
        LevelManager.Instance.ReloadLevel();
    }

    public void LoadMenu()
    {
        LevelManager.Instance.LoadMainMenu();
    }
    
    public void LoadNextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
