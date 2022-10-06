using System;
using UnityEngine;

public class BoosterMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve _moveSpeedCurve;
    
    private Player _player;
    private Booster _booster;
    private float _moveSpeed = 1f;
    private float _currentCooldown;

    private void OnEnable()
    {
        _moveSpeed = 1f;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _booster = gameObject.GetComponent<Booster>();
    }

    private void Update()
    {
        if (_currentCooldown <= 0)
        {
            _currentCooldown = 0;
            if (_booster.GetIsMagneted())
            {
                Move();
            }
        }

        _currentCooldown -= Time.deltaTime;
    }

    public void Move()
    {
        _moveSpeed += Time.deltaTime * _moveSpeedCurve.Evaluate(_moveSpeed);
        Vector3 movement = _player.transform.position - transform.position;
        transform.Translate(movement * _moveSpeed * Time.deltaTime);
    }
    
}
