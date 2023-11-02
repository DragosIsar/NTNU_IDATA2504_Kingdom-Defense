using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private PlayerInput playerInput;
    
    public InputAction MoveAction { private set; get; }
    public InputAction FireAction { private set; get; }
    public InputAction LookAction { private set; get; }

    public static float Zoom => Input.GetAxis("Mouse ScrollWheel");

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
        FireAction = playerInput.actions["Fire"];
        LookAction = playerInput.actions["Look"];
    }
}
