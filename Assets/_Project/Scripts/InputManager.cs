using UnityEngine;
using UnityEngine.InputSystem;
using LRS;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private PlayerInput playerInput;
    
    public InputAction MoveAction { private set; get; }
    public InputAction PlaceTower { private set; get; }
    public InputAction LookAction { private set; get; }
    public InputAction CancelAction { private set; get; }
    public InputAction Zoom { private set; get; }
    public InputAction PauseAction { private set; get; }

    protected override void Awake()
    {
        base.Awake();
        if (playerInput == null && TryGetComponent(out PlayerInput component))
        {
            playerInput = component;
        }
        else
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }
        
        MoveAction = playerInput.actions["Move"];
        PlaceTower = playerInput.actions["PlaceTower"];
        LookAction = playerInput.actions["Look"];
        Zoom = playerInput.actions["Zoom"];
        CancelAction = playerInput.actions["Cancel"];
        PauseAction = playerInput.actions["Pause"];
    }
}
