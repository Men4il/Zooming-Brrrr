using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float _damage = 100f;
    [SerializeField] private float _startHealth;
    
    private byte _id;
    private float _explosionCooldown;
    private float _currentExplosionCooldown;
    private float _health;

    public UnityEvent<Enemy> OnDeath;
    public bool IsExploded;
    public bool IsHittedWithChainRecently;

    private void Start()
    {
        _id = 1;
    }

    private void OnEnable()
    {
        _health = _startHealth;
    }

    private void Update()
    {
        ExplosionCooldownCheck();
    }

    private void ExplosionCooldownCheck()
    {
        if (_currentExplosionCooldown <= 0)
        {
            _currentExplosionCooldown = _explosionCooldown;

            if (IsExploded)
            {
                IsExploded = false;
            }
        }

        _currentExplosionCooldown -= Time.deltaTime;
    }

    public byte ID
    {
        get => _id;
        set => _id = value;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            OnDeath?.Invoke(this);
        }
    }

    public float GetHealth()
    {
        return _health;
    }

    public void SetHealth(float health)
    {
        _health = health;
    }
    
    public float GetDamage()
    {
        return _damage;
    }

    public void SetExplosionCooldown(float cooldown)
    {
        _explosionCooldown = cooldown;
        _currentExplosionCooldown = cooldown;
    }
}
