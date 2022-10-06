using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _playerCapsule;

    private Vector3 _playerMovement;
    private PlayerControlsInitializer _initializer;
    private CharacterController _controller;
    private Player _player;
    
    private Plane _groundPlane = new Plane(Vector3.up, Vector3.zero);
    private float _rayLength;
    
    void Start()
    {
        _player = GetComponent<Player>();
        _initializer = GetComponent<PlayerControlsInitializer>();
        _playerMovement = _initializer.MoveInput;
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        LookAtPos();
    }

    void LookAtPos()
    {
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (_groundPlane.Raycast(ray, out _rayLength))
        {
            var pointToLook = ray.GetPoint(_rayLength);
            Debug.DrawLine(ray.origin, pointToLook, Color.cyan);

            _playerCapsule.transform.LookAt(new Vector3(pointToLook.x, _playerCapsule.transform.position.y, pointToLook.z));
        }
    }

    private void Move()
    {
        _playerMovement = new Vector3(_initializer.MoveInput.x, 0, _initializer.MoveInput.y);
        _controller.Move(_playerMovement * (_player.GetMovementSpeed() * Time.deltaTime));
    }
}
