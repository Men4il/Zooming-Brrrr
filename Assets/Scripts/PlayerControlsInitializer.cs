using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsInitializer : MonoBehaviour
{
    private PlayerControls _input;
    private Action<InputAction.CallbackContext> _inputMovement;

    public Vector2 MoveInput { get; private set; }
    public bool IsShooting { get; private set; }

    private void SetMovementInput(InputAction.CallbackContext ctx) => 
        MoveInput = ctx.ReadValue<Vector2>();

    private void SetShootingTrue(InputAction.CallbackContext ctx) =>
        IsShooting = _input.Player.Fire.triggered;
    private void Awake()
    {
        _input = new PlayerControls();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Move.performed += SetMovementInput;
        _input.Player.Move.canceled += SetMovementInput;
        _input.Player.Fire.performed += SetShootingTrue;
        _input.Player.Fire.canceled += SetShootingTrue;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Move.performed -= SetMovementInput;
        _input.Player.Move.canceled -= SetMovementInput;
        _input.Player.Fire.performed -= SetShootingTrue;
        _input.Player.Fire.canceled -= SetShootingTrue;
    }
}
