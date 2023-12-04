using System;
using UnityEngine;
using UnityEngine.InputSystem;
using LRS;

public class InputManager : Singleton<InputManager>
{
    public static PlayerInput PlayerInput => _playerInput ? _playerInput : FindObjectOfType<PlayerInput>();
    private static PlayerInput _playerInput;
    
    public InputAction MoveAction { private set; get; }
    public InputAction PlaceTower { private set; get; }
    public InputAction LookAction { private set; get; }
    public InputAction CancelAction { private set; get; }
    public InputAction Zoom { private set; get; }
    public InputAction PauseAction { private set; get; }

    protected override void Awake()
    {
        base.Awake();
        MoveAction = PlayerInput.actions["Move"];
        PlaceTower = PlayerInput.actions["PlaceTower"];
        LookAction = PlayerInput.actions["Look"];
        Zoom = PlayerInput.actions["Zoom"];
        CancelAction = PlayerInput.actions["Cancel"];
        PauseAction = PlayerInput.actions["Pause"];
    }
}
