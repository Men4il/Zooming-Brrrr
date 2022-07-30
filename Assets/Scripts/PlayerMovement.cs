using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private CharacterController Controller;
    private PlayerControls input;
    
    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    float rayLength;
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject playerCapsule;
    
    private Vector3 PlayerMoveInput;
    [SerializeField] private float bulletForce = 20f;
    
    void Start()
    {
        input = new PlayerControls();
        input.Enable();

        input.Player.Move.performed += ctx =>
        {
            PlayerMoveInput = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
        };
        input.Player.Move.canceled += ctx =>
        {
            PlayerMoveInput = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
        };
        input.Player.Fire.started += Shoot;
        
        Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        LookAtPos();
    }

    void LookAtPos()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask)) {
            //transform.position = raycastHit.point;
        }

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);
            Debug.DrawLine(ray.origin, pointToLook, Color.cyan);

            playerCapsule.transform.LookAt(new Vector3(pointToLook.x, playerCapsule.transform.position.y, pointToLook.z));
        }
    }

    private void Move()
    {
        //Vector3 MoveVec = transform.TransformDirection(PlayerMoveInput); (Uncomment to move in view direction)

        Controller.Move(PlayerMoveInput * (_speed * Time.deltaTime));
    }

    private void Shoot(InputAction.CallbackContext ctx)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
